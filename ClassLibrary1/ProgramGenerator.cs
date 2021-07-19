using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace ClassLibrary1
{
    [Generator]
    public class ProgramGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var methodSymbol = context.Compilation.GetEntryPoint(context.CancellationToken);

            var source = $@"
using System;
using System.Text;
using System.Linq;

namespace {methodSymbol.ContainingType.ContainingNamespace.Name}
{{
    partial class {methodSymbol.ContainingType.Name}
    {{
        static void HelloGeneratedWorld(params string[] args)
        {{
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(""Hello Generated World!"");
            Console.WriteLine(""Printing args:"");

            foreach (var (arg, index) in args.Select((arg, index) => (arg, index)))
            {{
                Console.WriteLine($""[{{index}}] {{arg}}"");
            }}
        }}
    }}
}}
";

            context.AddSource(methodSymbol.ContainingType.Name, SourceText.From(source, Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
