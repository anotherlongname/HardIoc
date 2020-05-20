using System.Linq;
using HardIoC.CodeGenerators.Extensions;
using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators
{
    internal class ConstructorForTypeFinder
    {
        public static INamedTypeSymbol[] GetTypesRequiringConstructorsFromContainerAttributes(RegistrationSymbols registrationSymbols, AttributeData[] attributes)
        {
            var constructorForAttributes = attributes.Where(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, registrationSymbols.ConstructorForAttributeSymbol)).ToArray();
            return constructorForAttributes.Select(attr => attr.GetAttributeConstructorValueByParameterName<INamedTypeSymbol>("constructType")).ToArray();
        }
    }
}
