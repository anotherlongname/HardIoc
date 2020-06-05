namespace HardIoC.IoC
{
    public abstract class Container
    {
        public abstract T Resolve<T>();
    }
}
