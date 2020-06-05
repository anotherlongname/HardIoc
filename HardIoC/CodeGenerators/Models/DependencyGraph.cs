using System;
using System.Collections.Generic;
using System.Linq;
using HardIoC.CodeGenerators.Extensions;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Models
{
    internal class DependencyGraph
    {
        private readonly IDictionary<ITypeSymbol, Registration> _registrationDictionary;

        private DependencyGraph(Registration[] registrations)
        {
            _registrationDictionary = registrations.ToDictionary(r => r.Service(), r => r);
        }

        public Registration Resolve(ITypeSymbol typeSymbol)
            => _registrationDictionary.TryGetValue(typeSymbol, out var value) ? value : throw new Exception($"Failed to find dependency: {typeSymbol.FullyQualifiedTypeName()}");

        public Option<Registration> TryResolve(ITypeSymbol typeSymbol)
            => _registrationDictionary.TryGetValue(typeSymbol, out var registration) ? Option.Some(registration) : Option.None<Registration>();

        // TODO : Probably not the best
        public Registration[] Registrations => _registrationDictionary.Values.ToArray();

        public static DependencyGraph FromRegistrations(Registration[] registrations)
        {
            // TODO : Check for circular dependencies
            return new DependencyGraph(registrations);
        }
    }
}
