namespace HardIoC.CodeGenerators.Models
{
    internal class AspNetCoreContainerClassContent
    {
        private readonly string _namespace;
        private readonly string _className;

        public AspNetCoreContainerClassContent(string @namespace, string className)
        {
            _namespace = @namespace;
            _className = className;
        }

        public string AsString() => $@"
using System;

namespace {_namespace}
{{
    public partial class {_className}
    {{
        private readonly Func<IServiceProvider> _getServiceProvider;

        private {_className}(Func<IServiceProvider> getServiceProvider)
        {{
            _getServiceProvider = getServiceProvider;
        }}

        private IServiceProvider ServiceProvider => _getServiceProvider();

        public class ServiceProviderFactory : Microsoft.Extensions.DependencyInjection.IServiceProviderFactory<Microsoft.Extensions.DependencyInjection.IServiceCollection>
        {{
            public static ServiceProviderFactory Create()
                => new ServiceProviderFactory(sp => new {_className}(sp));

            private readonly Func<Func<IServiceProvider>, {_className}> _containerConstructor;

            private ServiceProviderFactory(Func<Func<IServiceProvider>, {_className}> containerConstructor)
            {{
                _containerConstructor = containerConstructor;
            }}

            public Microsoft.Extensions.DependencyInjection.IServiceCollection CreateBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
                => services;

            public IServiceProvider CreateServiceProvider(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
            {{
                IServiceProvider serviceProvider = null;

                var container = _containerConstructor(() => serviceProvider);

                foreach (var service in container.RegisteredServiceTypes())
                {{
                    if (System.Linq.Enumerable.Any(services, s => s.ServiceType == service))
                        continue;
                    Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient(services, service, sp => container.Resolve(service));
                }}

                serviceProvider = Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
                return serviceProvider;
            }}
        }}
    }}
}}";
    }
}
