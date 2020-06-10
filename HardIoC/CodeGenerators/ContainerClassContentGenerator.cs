using System;
using System.Collections.Generic;
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

            var factoryClasses = registrations.FactoryRegistrations().Select(f => CreateFactoryClassDeclaration(f, dependencyGraph)).ToArray();
            var singletonVariableDeclarations = dependencyGraph.Registrations.SingletonRegistrations().Select(CreateSingletonVariableDeclaration)
                .Concat(registrations.FactoryRegistrations().Select(CreateFactoryVariableDeclaration)).ToArray();

            var content = new ContainerClassContent(containerClassDescription.FullyQualifiedNamespace, containerClassDescription.ShortName, singletonVariableDeclarations, serviceConstructorMethods, factoryClasses);
            return content.AsString();
        }

        private FactoryClassDeclaration CreateFactoryClassDeclaration(FactoryRegistration factoryRegistration, DependencyGraph dependencyGraph)
            => new FactoryClassDeclaration(
                factoryRegistration.ImplimentationName,
                factoryRegistration.Service.FullyQualifiedTypeName(),
                factoryRegistration.ServiceMethods.Select(method => CreateServiceConstructorWithName(method.Name, method.ReturnType, dependencyGraph)).ToArray());

        private SingletonVariableDeclaration CreateSingletonVariableDeclaration(SingletonRegistration singletonRegistration)
            => new SingletonVariableDeclaration(singletonRegistration.Service.FullyQualifiedTypeName(), singletonRegistration.Service.Name);

        private SingletonVariableDeclaration CreateFactoryVariableDeclaration(FactoryRegistration factoryRegistration)
            => new SingletonVariableDeclaration(factoryRegistration.Service.FullyQualifiedTypeName(), factoryRegistration.Service.Name);

        private ServiceConstructor CreateServiceConstructorWithName(string constructorName, ITypeSymbol serviceType, DependencyGraph dependencyGraph)
            => new ServiceConstructor(serviceType.FullyQualifiedTypeName(), constructorName, ProduceNode(dependencyGraph.Resolve(serviceType), dependencyGraph));

        private ServiceConstructor CreateServiceConstructor(ITypeSymbol serviceType, DependencyGraph dependencyGraph)
            => CreateServiceConstructorWithName($"Create{serviceType.Name}", serviceType, dependencyGraph);

        private DependencyNode ProduceNode(Registration node, DependencyGraph dependencyGraph)
            => TryProduceNode(node, dependencyGraph)
                .Match(
                    node => node, 
                    () => throw new System.Exception($"Failed to resolve dependencies for {node.Service().FullyQualifiedTypeName()}"));

        // TODO : Uses recursion. Should use a stack (queue?) instead
        private Option<DependencyNode> TryProduceNode(Registration node, DependencyGraph dependencyGraph)
            => node.DependencyGroups()
                .Select(group => TryProduceDependencies(group, dependencyGraph))
                .FirstOrDefault(dependencies => dependencies.HasValue())
                .Map(dependencies => node.Match(
                    t => TransientNode(t, dependencies),
                    s => SingletonNode(s, dependencies),
                    d => DelegateNode(d, dependencies),
                    f => FactoryNode(f, dependencies)));

        private Option<DependencyNode[]> TryProduceDependencies(ITypeSymbol[] dependencyGroup, DependencyGraph dependencyGraph)
            => dependencyGroup
                .Select(dependencyGraph.TryResolve)
                .WhereAllSome()
                .Bind(dependencies => dependencies.Select(d => TryProduceNode(d, dependencyGraph)).WhereAllSome());

        private DependencyNode TransientNode(TransientRegistration node, DependencyNode[] dependencies)
            => new DependencyNode(new TransientDependencyNode(dependencies, node.Implementation.FullyQualifiedTypeName()));

        private DependencyNode SingletonNode(SingletonRegistration node, DependencyNode[] dependencies)
            => new DependencyNode(new SingletonDependencyNode(dependencies, node.Service.Name, node.Implementation.FullyQualifiedTypeName()));

        private DependencyNode DelegateNode(DelegateRegistration node, DependencyNode[] dependencies)
            => new DependencyNode(new DelegateDependencyNode(dependencies, node.DelegateType.RecursiveContainingSymbol(), node.Service.FullyQualifiedTypeName()));

        private DependencyNode FactoryNode(FactoryRegistration node, DependencyNode[] dependencies)
            => new DependencyNode(new FactoryDependencyNode(dependencies, node.Service.Name, node.Service.FullyQualifiedTypeName(), node.ImplimentationName));

    }
}
