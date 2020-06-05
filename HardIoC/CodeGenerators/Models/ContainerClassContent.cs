using System.Linq;

namespace HardIoC.CodeGenerators.Models
{
    internal class ContainerClassContent
    {
        private readonly string _namespace;
        private readonly string _className;
        private readonly string[] _singletonVariableDeclarations;
        private readonly string[] _serviceConstructorMethods;

        public ContainerClassContent(string @namespace, string className, string[] singletonVariableDeclarations, string[] serviceConstructorMethods)
        {
            _namespace = @namespace;
            _className = className;
            _singletonVariableDeclarations = singletonVariableDeclarations;
            _serviceConstructorMethods = serviceConstructorMethods;
        }

        public string AsString()
            => @"
using System;

namespace " + _namespace + @"
{
    public partial class " + _className + @"
    {
" + string.Join("\n", _singletonVariableDeclarations.Select(s => "\t\t" + s)) + @"

" + string.Join("\n", _serviceConstructorMethods.Select(s => "\t\t" + s)) + @"

        public override T Resolve<T>() => default;
    }
}";
    }
}
