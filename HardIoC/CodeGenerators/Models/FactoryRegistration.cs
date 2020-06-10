using System.Linq;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class FactoryRegistration
    {
        public FactoryRegistration(ITypeSymbol service)
        {
            Service = service;
            ImplimentationName = $"{service.Name}Impl";
            ServiceMethods = service.GetMembers().OfType<IMethodSymbol>().ToArray();
        }

        public ITypeSymbol Service { get; }
        public string ImplimentationName { get; }
        public IMethodSymbol[] ServiceMethods { get; }
    }
}
