namespace HardIoC.CodeGenerators.Models
{
    internal class FactoryDependencyNode
    {
        public FactoryDependencyNode(DependencyNode[] dependencies, string instanceName, string implimentationName)
        {
            Dependencies = dependencies;
            InstanceName = instanceName;
            ImplimentationName = implimentationName;
        }

        public DependencyNode[] Dependencies { get; }
        public string InstanceName { get; }
        public string ImplimentationName { get; }
    }
}
