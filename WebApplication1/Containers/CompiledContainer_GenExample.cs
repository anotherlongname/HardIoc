using System;

namespace WebApplication1.Containers
{
    // TODO : Generate this class based on an interface ("AspNetCoreFactory")
    public partial class CompiledContainer
    {
        private readonly Func<IServiceProvider> _getServiceProvider;

        private CompiledContainer(Func<IServiceProvider> getServiceProvider)
        {
            _getServiceProvider = getServiceProvider;
        }

        private IServiceProvider ServiceProvider => _getServiceProvider();

        public class ServiceProviderFactory : Microsoft.Extensions.DependencyInjection.IServiceProviderFactory<Microsoft.Extensions.DependencyInjection.IServiceCollection>
        {
            public static ServiceProviderFactory Create()
                => new ServiceProviderFactory(sp => new CompiledContainer(sp));

            private readonly Func<Func<IServiceProvider>, CompiledContainer> _containerConstructor;

            private ServiceProviderFactory(Func<Func<IServiceProvider>, CompiledContainer> containerConstructor)
            {
                _containerConstructor = containerConstructor;
            }

            public Microsoft.Extensions.DependencyInjection.IServiceCollection CreateBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
                => services;

            public IServiceProvider CreateServiceProvider(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
            {
                IServiceProvider serviceProvider = null;

                var container = _containerConstructor(() => serviceProvider);

                foreach (var service in container.RegisteredServiceTypes())
                {
                    if (System.Linq.Enumerable.Any(services, s => s.ServiceType == service))
                        continue;
                    Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient(services, service, sp => container.Resolve(service));
                }

                serviceProvider = Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
                return serviceProvider;
            }
        }
    }
}
