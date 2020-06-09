using System.Linq;

namespace HardIoC.CodeGenerators.Models
{
    internal class ContainerClassContent
    {
        private readonly string _namespace;
        private readonly string _className;
        private readonly string[] _singletonVariableDeclarations;
        private readonly string[] _serviceConstructorMethods;
        private readonly string[] _factoryClassDeclarations;

        public ContainerClassContent(string @namespace, string className, string[] singletonVariableDeclarations, string[] serviceConstructorMethods, string[] factoryClassDeclarations)
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
    }
}
