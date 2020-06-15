using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Specialized;
using HardIoC.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Handlers;

namespace WebApplication1.Containers
{
    public partial class CompiledContainer : Container,
        Register.Delegate<DoThing>,
        Register.Transient<DoThingHandler>,
        //Register.Delegate<ReadConfiguration, ReadConfigurationHandler>,
        //Register.Transient<ReadConfigurationHandler>,
        RegisterDelegateWithHandler<ReadConfiguration, ReadConfigurationHandler>,
        Register.Delegate<IConfiguration>
    {
        public DoThing Create()
            => Resolve<DoThingHandler>().Handle;

        IConfiguration Register.Delegate<IConfiguration>.Create()
            => (IConfiguration)ServiceProvider.GetService(typeof(IConfiguration));

        public ReadConfiguration Create(ReadConfigurationHandler handler)
            => handler.Handle;
    }


    public interface RegisterDelegateWithHandler<T, U> : Register.Delegate<T, U>, Register.Transient<U> { }
}
