﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HardIoC.CodeGenerators.Extensions;
using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators
{
    internal class ContainerClassContentGenerator
    {
        private readonly SourceGeneratorContext _context;
        private readonly RegistrationSymbols _registrationSymbols;

        public ContainerClassContentGenerator(SourceGeneratorContext context, RegistrationSymbols registrationSymbols)
        {
            _context = context;
            _registrationSymbols = registrationSymbols;
        }

        public string GenerateClassString(ContainerClassDescription containerClassDescription)
        {
            var registrations = RegistrationCreator.GetRegistrationsFromInterfaces(_registrationSymbols, containerClassDescription.AllInterfaces);
            var typesRequiringConstructors = ConstructorForTypeFinder.GetTypesRequiringConstructorsFromContainerAttributes(_registrationSymbols, containerClassDescription.AllAttributes);
            var dependencyGraph = DependencyGraph.FromRegistrations(registrations);

            var serviceConstructorMethods = typesRequiringConstructors.Select(t => CreateServiceConstructor(t, dependencyGraph)).ToArray();
            var singletonVariableDeclarations = dependencyGraph.Registrations.SingletonRegistrations().Select(CreateSingletonVariableDeclaration).ToArray();

            var content = new ContainerClassContent(containerClassDescription.FullyQualifiedNamespace, containerClassDescription.ShortName, singletonVariableDeclarations, serviceConstructorMethods);
            return content.AsString();
        }

        private string CreateSingletonVariableDeclaration(SingletonRegistration singletonRegistration)
            => $"private {singletonRegistration.Service.FullyQualifiedTypeName()} __{singletonRegistration.Service.Name};";


        private string CreateServiceConstructor(ITypeSymbol serviceType, DependencyGraph dependencyGraph)
            => $"public {serviceType.FullyQualifiedTypeName()} Create{serviceType.Name}() => {ProduceNode(dependencyGraph.Resolve(serviceType), dependencyGraph)};";

        private string ProduceNode(Registration node, DependencyGraph dependencyGraph)
            => TryProduceNode(node, dependencyGraph)
                .Match(
                    node => node, 
                    () => throw new System.Exception("Failed to find dependency for TODO"));

        // TODO : Uses recursion. Should use a stack (queue?) instead. Also needs refactoring
        private Option<string> TryProduceNode(Registration node, DependencyGraph dependencyGraph)
        {
            foreach (var group in node.DependencyGroups().OrderBy(g => g.Length))
            {
                var dependencies = group
                    .Select(dependencyGraph.TryResolve)
                    .WhereAllSome()
                    .Bind(deps => deps.Select(d => TryProduceNode(d, dependencyGraph)).WhereAllSome())
                    .Match(d => (true, d), () => (false, default));

                if(dependencies.Item1)
                {
                    return Option.Some(node.Match(
                        t => TransientNode(t, dependencies.Item2),
                        s => SingletonNode(s, dependencies.Item2),
                        d => DelegateNode(d, dependencies.Item2)));
                }
            }

            return Option.None<string>();
        }

        private string TransientNode(TransientRegistration node, string[] dependencies)
            => $"new {node.Implementation.FullyQualifiedTypeName()}({string.Join(", ", dependencies)})";

        private string SingletonNode(SingletonRegistration node, string[] dependencies)
            => $"__{node.Service.Name} ?? (__{node.Service.Name} = new {node.Implementation.FullyQualifiedTypeName()}({string.Join(", ", dependencies)}))";

        private string DelegateNode(DelegateRegistration node, string[] dependencies)
            => $"(({node.DelegateType.RecursiveContainingSymbol()}<{node.Service.FullyQualifiedTypeName()}{(dependencies.Any() ? "," : string.Empty)} {string.Join(", ", node.Dependencies.Select(d => d.FullyQualifiedTypeName()))}>)this).Create({string.Join(", ", dependencies)})";

    }
}
