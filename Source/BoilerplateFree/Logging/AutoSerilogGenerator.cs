// ReSharper disable SA1600

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable 1591
namespace BoilerplateFree
{
    [Generator]
    public class AutoSerilogGenerator: ISourceGenerator
    {
        private AttributeClassSyntaxReceiver classSyntaxReceiver = null!;
        public List<string> Log { get; } = new();

        public void Initialize(GeneratorInitializationContext context)
        {
            this.classSyntaxReceiver = new AttributeClassSyntaxReceiver(this.Log, "AddSerilog");

            context.RegisterForSyntaxNotifications(() => this.classSyntaxReceiver);

        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                this.ExecuteInTryCatch(context);
            }
            catch (Exception e)
            {
                this.Log.Add(e.StackTrace);
            }

            context.AddSource("Logs", SourceText.From($@"/*{ Environment.NewLine + string.Join(Environment.NewLine, this.Log) + Environment.NewLine}*/", Encoding.UTF8));

        }


        public void ExecuteInTryCatch(GeneratorExecutionContext context)
        {
            foreach (var declaringClass in this.classSyntaxReceiver.ClassesToGenerateFor)
            {
                var compilationUnit = declaringClass.GetCompilationUnit();

                var classNamespace = compilationUnit.GetNamespace();

                var declaringClassName = declaringClass.GetClassName();
                context.AddSource($"{declaringClassName}.cs", SourceText.From($@"

namespace {classNamespace} {{
using Serilog;

    public partial class {declaringClassName} {{
        private static Serilog.ILogger _logger = Log.ForContext<{declaringClassName}>();

    }}

}}
", Encoding.UTF8));
            }
        }
    }

}
