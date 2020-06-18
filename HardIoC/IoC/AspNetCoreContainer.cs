using System;

namespace HardIoC.IoC
{
    public abstract class AspNetCoreContainer : Container 
    {
        private readonly Func<IServiceProvider> _getServiceProvider;

        protected AspNetCoreContainer(Func<IServiceProvider> getServiceProvider)
        {
            _getServiceProvider = getServiceProvider;
        }

        protected IServiceProvider ServiceProvider => _getServiceProvider();
    }
}
