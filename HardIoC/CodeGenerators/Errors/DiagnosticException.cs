using System;
using Microsoft.CodeAnalysis;

namespace HardIoC.CodeGenerators.Errors
{
    internal class DiagnosticException : Exception
    {
        public DiagnosticException(Diagnostic diagnostic)
        {
            Diagnostic = diagnostic;
        }

        public Diagnostic Diagnostic { get; }
    }
}
