using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Extensions
{
    internal static class SymbolExtensions
    {
        public static string FullyQualifiedNamespace(this INamespaceSymbol namespaceSymbol)
        {
            var namespaces = new List<string>();
            while (!string.IsNullOrWhiteSpace(namespaceSymbol?.Name))
            {
                namespaces.Add(namespaceSymbol.Name);
                namespaceSymbol = namespaceSymbol.ContainingNamespace;
            }
            return string.Join(".", namespaces.ToArray().Reverse());
        }


        // This might not be needed
        public static string FullyQualifiedTypeName(this ITypeSymbol typeSymbol)
            => $"{typeSymbol.ContainingNamespace.FullyQualifiedNamespace()}.{typeSymbol.Name}";



        public static T GetAttributeConstructorValueByParameterName<T>(this AttributeData attributeData, string argName)
        {

            // Get the parameter
            IParameterSymbol parameterSymbol = attributeData.AttributeConstructor
                .Parameters
                .Where((constructorParam) => constructorParam.Name == argName).FirstOrDefault();

            // get the index of the parameter
            int parameterIdx = attributeData.AttributeConstructor.Parameters.IndexOf(parameterSymbol);

            // get the construct argument corresponding to this parameter
            TypedConstant constructorArg = attributeData.ConstructorArguments[parameterIdx];

            // return the value passed to the attribute
            return (T)constructorArg.Value;
        }
    }
}
