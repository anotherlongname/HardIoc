namespace HardIoC.IoC
{
    public abstract class Container
    {
        public virtual T Resolve<T>() => throw new System.Exception("Container class has not been properly inherited or generated");
    }
}
