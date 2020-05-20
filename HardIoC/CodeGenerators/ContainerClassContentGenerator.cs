using System.Linq;
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
            => $"private {singletonRegistration.Service.FullyQualifiedTypeName()} __{singletonRegistration.Service.Name}";


        // TODO : Uses recursion. Should use a stack instead
        private string CreateServiceConstructor(ITypeSymbol serviceType, DependencyGraph dependencyGraph)
            => $"public {serviceType.FullyQualifiedTypeName()} Create{serviceType.Name}() => {ProduceNode(dependencyGraph.Resolve(serviceType), dependencyGraph)};";

        // TODO : Finish and refactor
        private string ProduceNode(Registration node, DependencyGraph dependencyGraph)
        {
            var dependencies = node.Dependencies().Select(d => ProduceNode(dependencyGraph.Resolve(d), dependencyGraph)).ToArray();
            return node.Match(
                t => TransientNode(t, dependencies),
                s => SingletonNode(s, dependencies),
                d => DelegateNode(d, dependencies));
        }

        private string TransientNode(TransientRegistration node, string[] dependencies)
            => $"new {node.Implementation.FullyQualifiedTypeName()}({string.Join(", ", dependencies)})";

        private string SingletonNode(SingletonRegistration node, string[] dependencies)
            => $"__{node.Service.Name} ?? (__{node.Service.Name} = new {node.Implementation.FullyQualifiedTypeName()}({string.Join(", ", dependencies)}))";

        // TODO : improve to follow full path for delegate stuff
        private string DelegateNode(DelegateRegistration node, string[] dependencies)
            => $"((Functional.Structures.Register.Delegate<{node.Service.FullyQualifiedTypeName()}, {string.Join(", ", node.Dependencies.Select(d => d.FullyQualifiedTypeName()))}>)this).Create({string.Join(", ", dependencies)})";

    }
}
