using System.Linq;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class TransientRegistration
    {
        // TODO : This and the singleton could have multiple dependency groups. should change this to try to find best fit when needed. will help to support factories (later?)
        public TransientRegistration(ITypeSymbol service, ITypeSymbol implementation, ITypeSymbol[][] dependencyGroups)
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
