using System;
using HardIoC.Errors;

namespace HardIoC.IoC
{
    public abstract class Container
    {
        public T Resolve<T>() => TryResolve<T>(out var service) ? service : throw new ServiceNotFoundException(typeof(T));

        public object Resolve(Type serviceType) => TryResolve(serviceType, out var service) ? service : throw new ServiceNotFoundException(serviceType);

        public bool TryResolve<T>(out T service)
        {
            if(TryResolve(typeof(T), out var svc))
            {
                service = (T)svc;
                return true;
            }
            service = default;
            return false;
        }
        public virtual bool TryResolve(Type serviceType, out object service) => throw new ContainerClassImproperlyGeneratedException();
    }
}
