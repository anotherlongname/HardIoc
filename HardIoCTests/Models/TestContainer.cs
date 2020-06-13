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
    [ConstructorFor(typeof(MultiConstructorClass))]
    [ConstructorFor(typeof(IExampleFactory))]
    public partial class TestContainer : Container,
        Register.Transient<ITransientClass, TransientClass>,
        Register.Singleton<ISingletonClass, SingletonClass>,
        Register.Transient<DependencyClass>,
        Register.Delegate<StringDelegate>,
        Register.Transient<GenericClass<int>>,
        Register.Transient<MultiConstructorClass>,
        Register.Factory<IExampleFactory> 
    {
        private readonly string _stringDelegateContents;

        public TestContainer(string stringDelegateContents)
        {
            _stringDelegateContents = stringDelegateContents;
        }

        public TestContainer()
        {
            _stringDelegateContents = "Hello World! ";
        }

        public StringDelegate Create()
            => () => _stringDelegateContents;
    }

}
