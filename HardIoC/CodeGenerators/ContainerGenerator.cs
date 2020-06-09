using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HardIoC.CodeGenerators.Errors;
using HardIoC.CodeGenerators.Extensions;
using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace HardIoC.CodeGenerators
{
    [Generator]
    internal class ContainerGenerator : ISourceGenerator
    {
        public void Initialize(InitializationContext context) 
        {
            //System.Diagnostics.Debugger.Launch();
        }

        // TODO : Theoretically, we can location all instances of container.Resolve<T>() and generate off of that, can't we?
        public void Execute(SourceGeneratorContext context)
        {
            try
            {
                var registrationSymbols = RegistrationSymbols.FromCompilation(context.Compilation);
                var containerClasses = LocateContainerSymbols(context, registrationSymbols.ContainerSymbol);
                var generator = new ContainerClassContentGenerator(context, registrationSymbols);

                foreach(var containerClass in containerClasses)
                {
                    var hintName = $"Generated.{containerClass.FullyQualifiedName}";
                    var content = generator.GenerateClassString(containerClass);

                    WriteOutDebugFile(hintName, content, context);
                    context.AddSource(hintName, SourceText.From(content, Encoding.UTF8));
                }
            }
            catch (DiagnosticException ex)
            {
                context.ReportDiagnostic(ex.Diagnostic);
            }
            catch (Exception ex)
            {
                var descriptor = new DiagnosticDescriptor(DiagnosticConstants.UnknownExceptionId, "Unexpected error", $"Unknown error during generation: {ex.Message}", DiagnosticConstants.Category, DiagnosticSeverity.Error, true);
                context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
            }
        }

        // TODO : Remove when possible to peek into generated code
        private void WriteOutDebugFile(string hintName, string content, SourceGeneratorContext context)
        {
#if DEBUG
            try
            {
                var tempFile = $"{System.IO.Path.GetTempPath()}{hintName}.cs";
                System.IO.File.WriteAllText(tempFile, content);
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("GDBG", "Write out debug file", tempFile, DiagnosticConstants.Category, DiagnosticSeverity.Warning, true), Location.None));
            }
            catch { throw; }
#endif
        }

        private ContainerClassDescription[] LocateContainerSymbols(SourceGeneratorContext context, INamedTypeSymbol containerSymbol)
        {
            var containerClassGroups = new Dictionary<string, List<ITypeSymbol>>();

            foreach(var tree in context.Compilation.SyntaxTrees)
            {
                var semModel = context.Compilation.GetSemanticModel(tree);
                var partialClasses = GetAllClassesDerivedFromType(tree, semModel, containerSymbol);
                foreach(var classType in partialClasses)
                {
                    var typeName = classType.FullyQualifiedTypeName();
                    if (!containerClassGroups.ContainsKey(typeName))
                        containerClassGroups[typeName] = new List<ITypeSymbol>();

                    containerClassGroups[typeName].Add(classType);
                }
            }

            return containerClassGroups.Select(g => new ContainerClassDescription(g.Value.ToArray())).ToArray();
        }

        private ITypeSymbol[] GetAllClassesDerivedFromType(SyntaxTree syntaxTree, SemanticModel semanticModel, INamedTypeSymbol typeSymbol)
            => syntaxTree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Select(c => semanticModel.GetDeclaredSymbol(c))
                    .OfType<ITypeSymbol>()
                    .Where(c => HasBaseType(c, typeSymbol))
                    .ToArray();

        private bool HasBaseType(ITypeSymbol typeSymbol, INamedTypeSymbol baseTypeSymbol)
        {
            for (var b = typeSymbol.BaseType; b != null; b = b.BaseType)
                if (SymbolEqualityComparer.Default.Equals(b, baseTypeSymbol))
                    return true;

            return false;
        }
    }
}
