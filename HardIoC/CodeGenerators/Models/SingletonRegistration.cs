using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class SingletonRegistration
    {
        public SingletonRegistration(ITypeSymbol service, ITypeSymbol implementation, ITypeSymbol[] dependencies)
        {
            Service = service;
            Implementation = implementation;
            Dependencies = dependencies;
        }

        public ITypeSymbol Service { get; }
        public ITypeSymbol Implementation { get; }
        public ITypeSymbol[] Dependencies { get; }
    }
}
