using System;
using HardIoC.IoC;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class RegistrationSymbols
    {
        private RegistrationSymbols(INamedTypeSymbol containerSymbol, INamedTypeSymbol aspNetCoreContainerSymbol, INamedTypeSymbol constructorForAttributeSymbol, INamedTypeSymbol[] transientSymbols, INamedTypeSymbol[] singletonSymbols, INamedTypeSymbol[] delegateSymbols, INamedTypeSymbol factorySymbol)
        {
            ContainerSymbol = containerSymbol;
            AspNetCoreContainerSymbol = aspNetCoreContainerSymbol;
            ConstructorForAttributeSymbol = constructorForAttributeSymbol;
            TransientSymbols = transientSymbols;
            SingletonSymbols = singletonSymbols;
            DelegateSymbols = delegateSymbols;
            FactorySymbol = factorySymbol;
        }

        public INamedTypeSymbol ContainerSymbol { get; }
        public INamedTypeSymbol AspNetCoreContainerSymbol { get; }
        public INamedTypeSymbol ConstructorForAttributeSymbol { get; }
        public INamedTypeSymbol[] TransientSymbols { get; }
        public INamedTypeSymbol[] SingletonSymbols { get; }
        public INamedTypeSymbol[] DelegateSymbols { get; }
        public INamedTypeSymbol FactorySymbol { get; }

        public static RegistrationSymbols FromCompilation(Compilation compilation)
            => new RegistrationSymbols(
                GetType(compilation, typeof(Container)),
                GetType(compilation, typeof(AspNetCoreContainer)),
                GetType(compilation, typeof(ConstructorForAttribute)),
                new[] { GetType(compilation, typeof(Register.Transient<>)), GetType(compilation, typeof(Register.Transient<,>)) },
                new[] { GetType(compilation, typeof(Register.Singleton<>)), GetType(compilation, typeof(Register.Singleton<,>)) },
                new[] { GetType(compilation, typeof(Register.Delegate<>)), GetType(compilation, typeof(Register.Delegate<,>)), GetType(compilation, typeof(Register.Delegate<,,>)), GetType(compilation, typeof(Register.Delegate<,,,>)), GetType(compilation, typeof(Register.Delegate<,,,>)), GetType(compilation, typeof(Register.Delegate<,,,,>)), GetType(compilation, typeof(Register.Delegate<,,,,,>)) },
                GetType(compilation, typeof(Register.Factory<>)));

        private static INamedTypeSymbol GetType(Compilation compilation, Type type)
            => compilation.GetTypeByMetadataName(type.FullName) ?? throw new System.Exception($"Failed to find Named Type Symbol for {type.FullName}");
    }
}
