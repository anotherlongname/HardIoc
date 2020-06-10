namespace HardIoC.CodeGenerators.Models
{
    internal class FactoryDependencyNode
    {
        public FactoryDependencyNode(DependencyNode[] dependencies, string instanceName, string typeName, string implimentationName)
        {
            Dependencies = dependencies;
            InstanceName = instanceName;
            TypeName = typeName;
            ImplimentationName = implimentationName;
        }

        public DependencyNode[] Dependencies { get; }
        public string InstanceName { get; }
        public string TypeName { get; }
        public string ImplimentationName { get; }
    }
}
