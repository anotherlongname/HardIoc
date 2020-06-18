namespace HardIoC.CodeGenerators.Models
{
    internal class ServiceConstructor
    {
        public ServiceConstructor(string serviceTypeName, string constructorName, DependencyNode dependencies)
        {
            ServiceTypeName = serviceTypeName;
            ConstructorName = constructorName;
            Dependencies = dependencies;
        }

        public string ServiceTypeName { get; }
        public string ConstructorName { get; }
        public DependencyNode Dependencies { get; }
    }

    internal class ResolvableService
    {
        public ResolvableService(string serviceTypeName, DependencyNode dependencies)
        {
            ServiceTypeName = serviceTypeName;
            Dependencies = dependencies;
        }

        public string ServiceTypeName { get; }
        public DependencyNode Dependencies { get; }
    }
}
