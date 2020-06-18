using System;

namespace HardIoC.Errors
{
    public class ServiceNotFoundException : Exception
    {
        internal ServiceNotFoundException(Type serviceType)
            : base($"Service of type '{serviceType.Name}' could not be found")
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }
    }
}
