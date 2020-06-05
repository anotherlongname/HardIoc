using System.Linq;
using HardIoC.CodeGenerators.Extensions;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class ContainerClassDescription
    {
        public ContainerClassDescription(ITypeSymbol[] containerTypeInstances)
        {
            TypeSymbols = containerTypeInstances;
            FullyQualifiedName = containerTypeInstances.First()?.FullyQualifiedTypeName();
            FullyQualifiedNamespace = containerTypeInstances.First()?.ContainingNamespace.FullyQualifiedNamespace();
            ShortName = containerTypeInstances.First()?.Name;
            AllInterfaces = containerTypeInstances.SelectMany(c => c.AllInterfaces).ToArray();
            AllAttributes = containerTypeInstances.SelectMany(c => c.GetAttributes()).ToArray();
        }

        public ITypeSymbol[] TypeSymbols { get; }
        public INamedTypeSymbol[] AllInterfaces { get; }
        public AttributeData[] AllAttributes { get; }
        public string FullyQualifiedName { get; }
        public string FullyQualifiedNamespace { get; }
        public string ShortName { get; }
    }
}
