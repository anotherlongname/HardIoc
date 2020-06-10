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

    public interface IExampleFactory 
    {
        DependencyClass CreateDependencyClass();
        ISingletonClass CreateSingletonClass();
    }

    public class MultiConstructorClass
    {
        public const string GoodValue = "Should be used";

        public MultiConstructorClass() => Value = "Parameterless";
        public MultiConstructorClass(string value) => Value = "string";
        public MultiConstructorClass(DependencyClass dependencyClass, string value) => Value = "Dependency with string";
        public MultiConstructorClass(DependencyClass dependencyClass) => Value = GoodValue;

        public string Value { get; }
    }

}
