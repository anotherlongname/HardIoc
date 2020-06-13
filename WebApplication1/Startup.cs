using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using HardIoCTests.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication1.Containers;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.RegisterHardIoCServices(new CompiledContainer());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class HardIocServiceExtensions
    {
        public static IServiceCollection RegisterHardIoCServices(this IServiceCollection services, CompiledContainer container)
        {
            foreach (var service in container.RegisteredServiceTypes())
            {
                if (services.Any(s => s.ServiceType == service))
                    continue;
                services.AddTransient(service, sp =>
                {
                    //container.ServiceProvider = sp; // TODO : If we can avoid this, that would be great...
                    return container.Resolve(service);
                });
            }
            return services;
        }
    }


    // TODO : Do somethign similer to THIS \/ and pass the service provider in ...NO THIS WON"T WORK!

    public class ServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        public IServiceCollection CreateBuilder(IServiceCollection services)
            => services;

        public IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            IServiceProvider serviceProvider = null;

            var container = new CompiledContainer(() => serviceProvider);
            services.RegisterHardIoCServices(container);
            serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }

    //public class HardServiceProvider : IServiceProvider
    //{
    //    private readonly ServiceProvider _dynamicProvider;

    //    public HardServiceProvider(CompiledContainer container, IServiceCollection services)
    //    {
    //        services.RegisterHardIoCServices(container);
    //        _dynamicProvider = services.BuildServiceProvider();

    //    }

    //    public object GetService(Type serviceType)
    //    {
    //        if (_container.TryResolve(serviceType, out var service))
    //            return service;
    //        return _dynamicProvider.GetService(serviceType);
    //    }
    //}
}
