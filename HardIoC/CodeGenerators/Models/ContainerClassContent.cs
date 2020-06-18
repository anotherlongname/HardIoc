using System.Linq;
using HardIoC.CodeGenerators.Extensions;

namespace HardIoC.CodeGenerators.Models
{
    internal class ContainerClassContent
    {
        private readonly string _namespace;
        private readonly string _className;
        private readonly SingletonVariableDeclaration[] _singletonVariableDeclarations;
        private readonly ServiceConstructor[] _serviceConstructorMethods;
        private readonly ResolvableService[] _allServiceMethods;
        private readonly FactoryClassDeclaration[] _factoryClassDeclarations;

        public ContainerClassContent(string @namespace, string className, SingletonVariableDeclaration[] singletonVariableDeclarations, ServiceConstructor[] serviceConstructorMethods, ResolvableService[] allServiceMethods, FactoryClassDeclaration[] factoryClassDeclarations)
        {
            _namespace = @namespace;
            _className = className;
            _singletonVariableDeclarations = singletonVariableDeclarations;
            _serviceConstructorMethods = serviceConstructorMethods;
            _allServiceMethods = allServiceMethods;
            _factoryClassDeclarations = factoryClassDeclarations;
        }

        public string AsString()
            => $@"
using System;

namespace {_namespace}
{{
    public partial class {_className}
    {{
        private readonly SingletonInstances _SingletonInstances = new SingletonInstances();

        private class SingletonInstances
        {{
            {StringSingletonVariableDeclarations()}
        }}

        public override bool TryResolve(Type serviceType, out object service)
        {{
            {(_allServiceMethods.Any() ? StringResolveSwitchStatement() : string.Empty)}
            service = default;
            return false;
        }}

        public System.Collections.Generic.IEnumerable<Type> RegisteredServiceTypes()
        {{
            {StringRegisteredServiceTypes()}
        }}

        {StringServiceConstructorMethods(_serviceConstructorMethods)}

        {StringFactoryClassDeclarations()}
    }}
}}";

        private string StringRegisteredServiceTypes()
            => _allServiceMethods.Any() ?
                string.Join("\n\t\t\t", _allServiceMethods.Select(m => $"yield return typeof({m.ServiceTypeName});")) :
                "yield break;";

        private string StringResolveSwitchStatement()
            => $@"
            switch(serviceType.FullName)
            {{
                {StringResolveMethodSwitches()}
            }}";
                

        private string StringResolveMethodSwitches()
            => string.Join("\n\t\t\t\t", _allServiceMethods.Select(m => $"case \"{m.ServiceTypeName}\": service = (object){UnwrapNode(m.Dependencies)}; return true;"));

        private string StringSingletonVariableDeclarations()
            => string.Join("\n\t\t\t", _singletonVariableDeclarations.Select(s => $"public {s.SingletonTypeName} __{s.SingletonVariableName};"));

        private string StringServiceConstructorMethods(ServiceConstructor[] serviceConstructors)
            => string.Join("\n\t\t", serviceConstructors.Select(m => $"public {m.ServiceTypeName} {m.ConstructorName}() => {UnwrapNode(m.Dependencies)};"));

        private string UnwrapNode(DependencyNode node)
            => node.Match(
                t => StringTransientNode(t, t.Dependencies.Select(UnwrapNode).ToArray()),
                s => StringSingletonNode(s, s.Dependencies.Select(UnwrapNode).ToArray()),
                d => StringDelegateNode(d, d.Dependencies.Select(UnwrapNode).ToArray()),
                f => StringFactoryNode(f, f.Dependencies.Select(UnwrapNode).ToArray()));

        private string StringTransientNode(TransientDependencyNode node, string[] dependencies)
            => $"new {node.TypeName}({string.Join(", ", dependencies)})";

        private string StringSingletonNode(SingletonDependencyNode node, string[] dependencies)
            => $"(_SingletonInstances.__{node.InstanceName} ??= new {node.TypeName}({string.Join(", ", dependencies)}))";

        private string StringDelegateNode(DelegateDependencyNode node, string[] dependencies)
            => $"(({node.DelegateRegistrationTypeName}<{node.TypeName}{(dependencies.Any() ? "," : string.Empty)} {string.Join(", ", node.Dependencies.TypeNames())}>)this).Create({string.Join(", ", dependencies)})";

        private string StringFactoryNode(FactoryDependencyNode node, string[] dependencies)
            => $"(_SingletonInstances.__{node.InstanceName} ??= new {node.ImplimentationName}({string.Join(", ", new[] { "_SingletonInstances" }.Concat(dependencies))}))";


        private string StringFactoryClassDeclarations()
            => string.Join("\n\t\t", _factoryClassDeclarations.Select(f => $@"
        private class {f.FactoryImplimentationClassName} : {f.FactoryTypeName}
        {{
            private readonly SingletonInstances _SingletonInstances;

            public {f.FactoryImplimentationClassName}(SingletonInstances singletonInstances) => _SingletonInstances = singletonInstances;

            {StringServiceConstructorMethods(f.ServiceConstructorMethods)}
        }}
"));
    }
}
