using System.Linq;
using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators
{
    internal static class RegistrationCreator
    {
        public static Registration[] GetRegistrationsFromInterfaces(RegistrationSymbols registrationSymbols, INamedTypeSymbol[] interfaces)
            => interfaces.Select(i =>
                    registrationSymbols.TryCreateTransientRegistration(i, out var reg1) ? (true, reg1) :
                    registrationSymbols.TryCreateSingletonRegistration(i, out var reg2) ? (true, reg2) :
                    registrationSymbols.TryCreateDelegateRegistration(i, out var reg3) ? (true, reg3) :
                    (false, default))
                .Where(i => i.Item1)
                .Select(i => i.Item2)
                .ToArray();

        private static bool TryCreateTransientRegistration(this RegistrationSymbols registrationSymbols, INamedTypeSymbol typeSymbol, out Registration registration)
        {
            if (registrationSymbols.TransientSymbols.Any(s => SymbolEqualityComparer.Default.Equals(s, typeSymbol.OriginalDefinition)))
            {
                registration = CreateTransientRegistration((INamedTypeSymbol)typeSymbol.TypeArguments.First(), (INamedTypeSymbol)typeSymbol.TypeArguments.Last());
                return true;
            }

            registration = default;
            return false;
        }

        private static bool TryCreateSingletonRegistration(this RegistrationSymbols registrationSymbols, INamedTypeSymbol typeSymbol, out Registration registration)
        {
            if (registrationSymbols.SingletonSymbols.Any(s => SymbolEqualityComparer.Default.Equals(s, typeSymbol.OriginalDefinition)))
            {
                registration = CreateSingletonRegistration((INamedTypeSymbol)typeSymbol.TypeArguments.First(), (INamedTypeSymbol)typeSymbol.TypeArguments.Last());
                return true;
            }

            registration = default;
            return false;
        }

        private static bool TryCreateDelegateRegistration(this RegistrationSymbols registrationSymbols, INamedTypeSymbol typeSymbol, out Registration registration)
        {
            if (registrationSymbols.DelegateSymbols.Any(s => SymbolEqualityComparer.Default.Equals(s, typeSymbol.OriginalDefinition)))
            {
                registration = CreateDelegateRegistration((INamedTypeSymbol)typeSymbol.TypeArguments.First(), typeSymbol.TypeArguments.Skip(1).OfType<INamedTypeSymbol>().ToArray());
                return true;
            }

            registration = default;
            return false;
        }



        // TODO : This chooses the largest constructor. Probably should do something else
        private static Registration CreateTransientRegistration(INamedTypeSymbol serviceType, INamedTypeSymbol implementationType)
            => new Registration(
                new TransientRegistration(
                    serviceType,
                    implementationType,
                    implementationType
                        .InstanceConstructors
                        .OrderBy(ctor => ctor.Parameters.Length)
                        .First()
                        .Parameters
                        .Select(p => p.Type)
                        .ToArray()));

        // TODO : This chooses the largest constructor. Probably should do something else
        private static Registration CreateSingletonRegistration(INamedTypeSymbol serviceType, INamedTypeSymbol implementationType)
            => new Registration(
                new SingletonRegistration(
                    serviceType,
                    implementationType,
                    implementationType
                        .InstanceConstructors
                        .OrderBy(ctor => ctor.Parameters.Length)
                        .First()
                        .Parameters
                        .Select(p => p.Type)
                        .ToArray()));

        private static Registration CreateDelegateRegistration(INamedTypeSymbol serviceType, INamedTypeSymbol[] dependencyTypes)
            => new Registration(new DelegateRegistration(serviceType, dependencyTypes));

    }
}
