using System.Linq;
using HardIoC.CodeGenerators.Models;

namespace HardIoC.CodeGenerators.Extensions
{
    internal static class DependencyNodeExtensions
    {
        public static string[] TypeNames(this DependencyNode[] dependencyNodes)
            => dependencyNodes.Select(
                node => node.Match(
                    t => t.TypeName,
                    s => s.TypeName,
                    d => d.TypeName,
                    f => f.TypeName))
            .ToArray();
    }
}
