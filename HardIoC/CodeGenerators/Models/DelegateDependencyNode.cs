namespace HardIoC.CodeGenerators.Models
{
    internal class DelegateDependencyNode
    {
        public DelegateDependencyNode(DependencyNode[] dependencies, string delegateRegistrationTypeName, string typeName)
        {
            Dependencies = dependencies;
            DelegateRegistrationTypeName = delegateRegistrationTypeName;
            TypeName = typeName;
        }

        public DependencyNode[] Dependencies { get; }
        public string DelegateRegistrationTypeName { get; }
        public string TypeName { get; }
    }
}
