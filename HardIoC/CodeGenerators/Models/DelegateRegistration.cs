using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class DelegateRegistration
    {
        public DelegateRegistration(ITypeSymbol delegateType, ITypeSymbol service, ITypeSymbol[] dependencies)
        {
            DelegateType = delegateType;
            Service = service;
            Dependencies = dependencies;
        }

        public ITypeSymbol Service { get; }
        public ITypeSymbol[] Dependencies { get; }

        public ITypeSymbol DelegateType { get; }
    }
}
