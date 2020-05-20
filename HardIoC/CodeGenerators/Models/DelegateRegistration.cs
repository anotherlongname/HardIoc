using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class DelegateRegistration
    {
        public DelegateRegistration(ITypeSymbol service, ITypeSymbol[] dependencies)
        {
            Service = service;
            Dependencies = dependencies;
        }

        public ITypeSymbol Service { get; }
        public ITypeSymbol[] Dependencies { get; }
    }
}
