using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardIoC.IoC;

namespace HardIoCTests.Models
{
    [ConstructorFor(typeof(ITransientClass))]
    [ConstructorFor(typeof(ISingletonClass))]
    public partial class TestContainer : Container,
        Register.Transient<ITransientClass, TransientClass>,
        Register.Singleton<ISingletonClass, SingletonClass>,
        Register.Transient<DependencyClass>
    {
    }
}
