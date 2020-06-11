using System;

namespace HardIoC.Errors
{
    public class ContainerClassImproperlyGeneratedException : Exception
    {
        internal ContainerClassImproperlyGeneratedException()
            : base("Container class has not been properly inherited or generated") { }
    }
}
