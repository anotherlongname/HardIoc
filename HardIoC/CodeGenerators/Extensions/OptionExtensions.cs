using System;
using System.Collections.Generic;
using HardIoC.CodeGenerators.Models;

namespace HardIoC.CodeGenerators.Extensions
{
    internal static class OptionExtensions
    {
        public static Option<T[]> WhereAllSome<T>(this IEnumerable<Option<T>> options)
        {
            var list = new List<T>();
            foreach(var opt in options)
            {
                var result = opt.Match(some => (true, some), () => (false, default));
                if (!result.Item1)
                    return Option.None<T[]>();

                list.Add(result.Item2);
            }

            return Option.Some(list.ToArray());
        }

        public static bool HasValue<T>(this Option<T> option)
            => option.Match(_ => true, () => false);

        public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> bindFunc)
            => option.Match(bindFunc, () => Option.None<U>());
    }
}
