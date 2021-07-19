using ClassLibrary1.Extensions;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    [Generator]
    public class CSVToClassGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var methodSymbol = context.Compilation.GetEntryPoint(context.CancellationToken);

            foreach (var file in context.AdditionalFiles.Where(x => x.Path.EndsWith(".csv")))
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Path)
                    .Dehumanize()
                    .RemoveDiacritics();

                var text = file.GetText();

                var members = text.Lines.FirstOrDefault()
                    .ToString()
                    .Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());

                var identationForMembers = "\t\t";
                var source = $@"
using System;

namespace {methodSymbol.ContainingType.ContainingNamespace.Name}
{{
    public class {fileName}
    {{
{string.Join("\n", members.Select(x => $@"{identationForMembers}public object {x.Dehumanize().RemoveDiacritics()} {{ get; set; }}"))}
    }}
}}
";

                // TODO: Add collection serialization support

                context.AddSource($"{fileName}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
        }
    }
}
