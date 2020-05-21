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
    [ConstructorFor(typeof(StringDelegate))]
    public partial class TestContainer : Container,
        Register.Transient<ITransientClass, TransientClass>,
        Register.Singleton<ISingletonClass, SingletonClass>,
        Register.Transient<DependencyClass>,
        Register.Delegate<StringDelegate>
    {
        private readonly string _stringDelegateContents;

        public TestContainer(string stringDelegateContents)
        {
            _stringDelegateContents = stringDelegateContents;
        }

        public TestContainer()
        {
            _stringDelegateContents = "Hello World!";
        }

        public StringDelegate Create()
            => () => _stringDelegateContents;
    }
}
