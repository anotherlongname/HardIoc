using System;

namespace HardIoC.CodeGenerators.Models
{
    internal class Option<T>
    {
        private readonly T _some;
        private readonly bool _hasSome;
        public Option()
        {
            _hasSome = false;
        }

        public Option(T some)
        {
            _hasSome = true;
            _some = some;
        }

        public U Match<U>(Func<T, U> someFunc, Func<U> noneFunc)
            => _hasSome ? someFunc(_some) : noneFunc();
    }

    internal static class Option
    {
        public static Option<T> Some<T>(T some) => new Option<T>(some);
        public static Option<T> None<T>() => new Option<T>();
    }
}
