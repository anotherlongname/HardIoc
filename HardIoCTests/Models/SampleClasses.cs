using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardIoCTests.Models
{
    public interface ITransientClass { }
    public class TransientClass : ITransientClass
    {
        public TransientClass(GenericClass<int> test) { }
    }

    public interface ISingletonClass { }
    public class SingletonClass : ISingletonClass
    {
        public SingletonClass(DependencyClass dependencyClass) { }
    }

    public class DependencyClass
    {

    }

    public delegate string StringDelegate();

    public class GenericClass<T>
    {
        
    }
}
