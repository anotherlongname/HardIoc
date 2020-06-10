using System.Linq;

namespace HardIoC.CodeGenerators.Models
{
    internal class ContainerClassContent
    {
        private readonly string _namespace;
        private readonly string _className;
        private readonly SingletonVariableDeclaration[] _singletonVariableDeclarations;
        private readonly ServiceConstructor[] _serviceConstructorMethods;
        private readonly FactoryClassDeclaration[] _factoryClassDeclarations;

        public ContainerClassContent(string @namespace, string className, SingletonVariableDeclaration[] singletonVariableDeclarations, ServiceConstructor[] serviceConstructorMethods, FactoryClassDeclaration[] factoryClassDeclarations)
        {
            _namespace = @namespace;
            _className = className;
            _singletonVariableDeclarations = singletonVariableDeclarations;
            _serviceConstructorMethods = serviceConstructorMethods;
            _factoryClassDeclarations = factoryClassDeclarations;
        }

        public string AsString()
            => @"
using System;

namespace " + _namespace + @"
{
    public partial class " + _className + @"
    {
        private readonly SingletonInstances _SingletonInstances = new SingletonInstances();

        private class SingletonInstances
        {
" + string.Join("\n", _singletonVariableDeclarations.Select(s => "\t\t\t" + s)) + @"
        }

" + string.Join("\n", _serviceConstructorMethods.Select(s => "\t\t" + s)) + @"

" + string.Join("\n", _factoryClassDeclarations.Select(f => "\t\t" + f)) + @"
    }
}";



        private string TransientNode(TransientRegistration node, string[] dependencies)
            => $"new {node.Implementation.FullyQualifiedTypeName()}({string.Join(", ", dependencies)})";

        private string SingletonNode(SingletonRegistration node, string[] dependencies)
            => $"(_SingletonInstances.__{node.Service.Name} ??= new {node.Implementation.FullyQualifiedTypeName()}({string.Join(", ", dependencies)}))";

        private string DelegateNode(DelegateRegistration node, string[] dependencies)
            => $"(({node.DelegateType.RecursiveContainingSymbol()}<{node.Service.FullyQualifiedTypeName()}{(dependencies.Any() ? "," : string.Empty)} {string.Join(", ", node.Dependencies.Select(d => d.FullyQualifiedTypeName()))}>)this).Create({string.Join(", ", dependencies)})";

        private string FactoryNode(FactoryRegistration node, string[] dependencies)
            => $"(_SingletonInstances.__{node.Service.Name} ??= new {node.ImplimentationName}({string.Join(", ", new[] { "_SingletonInstances" }.Concat(dependencies))}))";



        private string CreateSingletonVariableDeclaration(SingletonRegistration singletonRegistration)
    => $"public {singletonRegistration.Service.FullyQualifiedTypeName()} __{singletonRegistration.Service.Name};";

        private string CreateFactoryVariableDeclaration(FactoryRegistration factoryRegistration)
            => $"public {factoryRegistration.Service.FullyQualifiedTypeName()} __{factoryRegistration.Service.Name};";

        private string CreateServiceConstructorWithName(string constructorName, ITypeSymbol serviceType, DependencyGraph dependencyGraph)
            => $"public {serviceType.FullyQualifiedTypeName()} {constructorName}() => {ProduceNode(dependencyGraph.Resolve(serviceType), dependencyGraph)};";

        private string CreateServiceConstructor(ITypeSymbol serviceType, DependencyGraph dependencyGraph)
            => CreateServiceConstructorWithName($"Create{serviceType.Name}", serviceType, dependencyGraph);

        private string CreateFactoryClassDeclaration(FactoryRegistration factoryRegistration, DependencyGraph dependencyGraph)
            => @$"private class {factoryRegistration.ImplimentationName} : {factoryRegistration.Service.FullyQualifiedTypeName()}
        {{
            private readonly SingletonInstances _SingletonInstances;

            public {factoryRegistration.ImplimentationName}(SingletonInstances singletonInstances) => _SingletonInstances = singletonInstances;

            {string.Join("\n\t\t\t", factoryRegistration.ServiceMethods.Select(method => CreateServiceConstructorWithName(method.Name, method.ReturnType, dependencyGraph)))}
        }}";
    }

    internal class FactoryClassDeclaration
    {
        public FactoryClassDeclaration(string factoryImplimentationClassName, string factoryTypeName, ServiceConstructor[] serviceConstructorMethods)
        {
            FactoryImplimentationClassName = factoryImplimentationClassName;
            FactoryTypeName = factoryTypeName;
            ServiceConstructorMethods = serviceConstructorMethods;
        }

        public string FactoryImplimentationClassName { get; }
        public string FactoryTypeName { get; }
        public ServiceConstructor[] ServiceConstructorMethods { get; }
    }

    internal class SingletonVariableDeclaration
    {
        public SingletonVariableDeclaration(string singletonTypeName, string singletonVariableName)
        {
            SingletonTypeName = singletonTypeName;
            SingletonVariableName = singletonVariableName;
        }

        public string SingletonTypeName { get; }
        public string SingletonVariableName { get; }
    }
}
