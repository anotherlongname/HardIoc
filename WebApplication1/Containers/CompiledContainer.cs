using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardIoC.IoC;
using Microsoft.Extensions.Configuration;
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
        private readonly Func<IServiceProvider> _getServiceProvider;

        public DoThing Create()
            => Resolve<DoThingHandler>().Handle;

        IConfiguration Register.Delegate<IConfiguration>.Create()
            => (IConfiguration)_getServiceProvider().GetService(typeof(IConfiguration));

        public ReadConfiguration Create(ReadConfigurationHandler handler)
            => handler.Handle;

        public CompiledContainer(Func<IServiceProvider> getServiceProvider)
        {
            _getServiceProvider = getServiceProvider;
        }
    }

    public interface RegisterDelegateWithHandler<T, U> : Register.Delegate<T, U>, Register.Transient<U> { }
}
