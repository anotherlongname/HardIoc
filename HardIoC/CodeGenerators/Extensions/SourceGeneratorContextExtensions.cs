using System.Collections.Generic;
using System.Linq;
using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HardIoC.CodeGenerators.Extensions
{
    internal static class SourceGeneratorContextExtensions
    {
        public static ContainerClassDescription[] LocateContainerSymbols(this SourceGeneratorContext context, INamedTypeSymbol containerSymbol)
        {
            var containerClassGroups = new Dictionary<string, List<ITypeSymbol>>();

            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                var semModel = context.Compilation.GetSemanticModel(tree);
                var partialClasses = GetAllClassesDerivedFromType(tree, semModel, containerSymbol);
                foreach (var classType in partialClasses)
                {
                    var typeName = classType.FullyQualifiedTypeName();
                    if (!containerClassGroups.ContainsKey(typeName))
                        containerClassGroups[typeName] = new List<ITypeSymbol>();

                    containerClassGroups[typeName].Add(classType);
                }
            }

            return containerClassGroups.Select(g => new ContainerClassDescription(g.Value.ToArray())).ToArray();
        }

        private static  ITypeSymbol[] GetAllClassesDerivedFromType(SyntaxTree syntaxTree, SemanticModel semanticModel, INamedTypeSymbol typeSymbol)
            => syntaxTree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Select(c => semanticModel.GetDeclaredSymbol(c))
                    .OfType<ITypeSymbol>()
                    .Where(c => HasBaseType(c, typeSymbol))
                    .ToArray();

        private static bool HasBaseType(ITypeSymbol typeSymbol, INamedTypeSymbol baseTypeSymbol)
        {
            for (var b = typeSymbol.BaseType; b != null; b = b.BaseType)
                if (SymbolEqualityComparer.Default.Equals(b, baseTypeSymbol))
                    return true;

            return false;
        }
    }
}
