using ExampleAspNetCoreWebApplication.Handlers;
using HardIoC.IoC;
using Microsoft.Extensions.Configuration;

namespace ExampleAspNetCoreWebApplication.Containers
{
    public partial class CompiledContainer : AspNetCoreContainer,
        Register.Delegate<DoThing>,
        Register.Transient<DoThingHandler>,
        Register.Delegate<ReadConfiguration, ReadConfigurationHandler>,
        Register.Transient<ReadConfigurationHandler>,
        Register.Delegate<IConfiguration>
    {
        DoThing Register.Delegate<DoThing>.Create()
            => Resolve<DoThingHandler>().Handle;

        IConfiguration Register.Delegate<IConfiguration>.Create()
            => (IConfiguration)ServiceProvider.GetService(typeof(IConfiguration));

        ReadConfiguration Register.Delegate<ReadConfiguration, ReadConfigurationHandler>.Create(ReadConfigurationHandler handler)
            => handler.Handle;
    }
}
