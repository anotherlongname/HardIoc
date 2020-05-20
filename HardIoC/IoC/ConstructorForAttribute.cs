using System;

namespace HardIoC.IoC
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConstructorForAttribute : Attribute
    {
        public ConstructorForAttribute(Type constructType) { }
    }
}
