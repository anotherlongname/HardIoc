using System;
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
                d => d.Service,
                f => f.Service);

        public static ITypeSymbol[][] DependencyGroups(this Registration registration)
            => registration.Match(
                t => t.DependencyGroups,
                s => s.DependencyGroups,
                d => new[] { d.Dependencies },
                f => Array.Empty<ITypeSymbol[]>());

        public static TransientRegistration[] TransientRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (true, t),
                s => (false, (TransientRegistration)null),
                d => (false, (TransientRegistration)null),
                f => (false, (TransientRegistration)null)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();

        public static SingletonRegistration[] SingletonRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (false, (SingletonRegistration)null),
                s => (true, s),
                d => (false, (SingletonRegistration)null),
                f => (false, (SingletonRegistration)null)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();

        public static DelegateRegistration[] DelegateRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (false, (DelegateRegistration)null),
                s => (false, (DelegateRegistration)null),
                d => (true, d),
                f => (false, (DelegateRegistration)null)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();

        public static FactoryRegistration[] FactoryRegistrations(this Registration[] registrations)
            => registrations.Select(r => r.Match(
                t => (false, (FactoryRegistration)null),
                s => (false, (FactoryRegistration)null),
                d => (false, (FactoryRegistration)null),
                f => (true, f)))
            .Where(r => r.Item1)
            .Select(r => r.Item2)
            .ToArray();
    }
}
