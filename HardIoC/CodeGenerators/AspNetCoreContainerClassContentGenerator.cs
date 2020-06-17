using HardIoC.CodeGenerators.Models;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators
{
    internal class AspNetCoreContainerClassContentGenerator
    {
        private readonly SourceGeneratorContext _context;
        private readonly RegistrationSymbols _registrationSymbols;

        public AspNetCoreContainerClassContentGenerator(SourceGeneratorContext context, RegistrationSymbols registrationSymbols)
        {
            _context = context;
            _registrationSymbols = registrationSymbols;
        }

        public string GenerateClassString(ContainerClassDescription containerClassDescription)
        {
            var content = new AspNetCoreContainerClassContent(containerClassDescription.FullyQualifiedNamespace, containerClassDescription.ShortName);
            return content.AsString();
        }

    }
}
