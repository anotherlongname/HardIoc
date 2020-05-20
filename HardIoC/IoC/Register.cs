
namespace HardIoC.IoC
{
    public static class Register
    {
        public interface Transient<TService> { }
        public interface Transient<TService, TImplimentation> where TImplimentation : TService { }

        public interface Singleton<TService> { }
        public interface Singleton<TService, TImplimentation> where TImplimentation : TService { }

        public interface Delegate<TService, TDependency> { TService Create(TDependency dependency); }
        public interface Delegate<TService, TDependency1, TDependency2> { TService Create(TDependency1 dependency1, TDependency2 dependency2); }
        public interface Delegate<TService, TDependency1, TDependency2, TDependency3> { TService Create(TDependency1 dependency1, TDependency2 dependency2, TDependency3 dependency3); }
        public interface Delegate<TService, TDependency1, TDependency2, TDependency3, TDependency4> { TService Create(TDependency1 dependency1, TDependency2 dependency2, TDependency3 dependency3, TDependency4 dependency4); }
        public interface Delegate<TService, TDependency1, TDependency2, TDependency3, TDependency4, TDependency5> { TService Create(TDependency1 dependency1, TDependency2 dependency2, TDependency3 dependency3, TDependency4 dependency4, TDependency5 dependency5); }

        // TODO : Look into ways to implement Register.Factory
        public interface Factory<TFactory> { }
    }
}
