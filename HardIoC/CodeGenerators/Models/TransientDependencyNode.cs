namespace HardIoC.CodeGenerators.Models
{
    internal class TransientDependencyNode
    {
        public TransientDependencyNode(DependencyNode[] dependencies, string typeName)
        {
            Dependencies = dependencies;
            TypeName = typeName;
        }

        public DependencyNode[] Dependencies { get; }
        public string TypeName { get; }
    }
}
