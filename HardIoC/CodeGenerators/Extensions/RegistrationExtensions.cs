using System.Linq;
using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Extensions
{
    internal static class RegistrationExtensions
    {
        public static ITypeSymbol Service(this Registration registration)
            => registration.Match(
                t => t.Service,
                s => s.Service,
                d => d.Service);

        public static ITypeSymbol[] Dependencies(this Registration registration)
            => registration.Match(
                t => t.Dependencies,
                s => s.Dependencies,
                d => d.Dependencies);

        public static TransientRegistration[] TransientRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (true, t),
                s => (false, (TransientRegistration)null),
                d => (false, (TransientRegistration)null)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();

        public static SingletonRegistration[] SingletonRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (false, (SingletonRegistration)null),
                s => (true, s),
                d => (false, (SingletonRegistration)null)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();

        public static DelegateRegistration[] DelegateRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (false, (DelegateRegistration)null),
                s => (false, (DelegateRegistration)null),
                d => (true, d)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();
    }
}
