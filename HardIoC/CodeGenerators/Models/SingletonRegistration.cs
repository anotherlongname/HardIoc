using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class SingletonRegistration
    {
        public SingletonRegistration(ITypeSymbol service, ITypeSymbol implementation, ITypeSymbol[][] dependencyGroups)
        {
            Service = service;
            Implementation = implementation;
            DependencyGroups = dependencyGroups;
        }

        public ITypeSymbol Service { get; }
        public ITypeSymbol Implementation { get; }
        public ITypeSymbol[][] DependencyGroups { get; }
    }
}
