// ReSharper disable SA1600

#pragma warning disable 1591
namespace BoilerplateFree
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    [Generator]
    public class ConstructorGenerator : ISourceGenerator
    {
        public List<string> Log { get; } = new List<string>();
        private AttributeClassSyntaxReceiver classSyntaxReceiver = null!;

        public void Initialize(GeneratorInitializationContext context)
        {
            this.classSyntaxReceiver = new AttributeClassSyntaxReceiver(this.Log, "AutoGenerateConstructor");

            context.RegisterForSyntaxNotifications(() =>
            {
                return this.classSyntaxReceiver;
            });
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

            context.AddSource("Logs",
                SourceText.From(
                    $@"/*{Environment.NewLine + string.Join(Environment.NewLine, this.Log) + Environment.NewLine}*/",
                    Encoding.UTF8));
        }

        public void ExecuteInTryCatch(GeneratorExecutionContext context)
        {
            foreach (var declaringClass in this.classSyntaxReceiver.ClassesToGenerateFor)
            {
                var names = new List<string>();
                var types = new List<string>();

                // TODO there is a GetUsings in the extensions

                var compilationUnit = declaringClass.GetCompilationUnit();

                var classNamespace = compilationUnit.GetNamespace();

                this.Log.Add($"Namespace: " + classNamespace);

                // this.Log.Add(compilationUnit.ToFullString());
                // this.Log.Add($"Usings count: {compilationUnit.GetUsings().Count}");
                // this.Log.Add($"usings:" + string.Join(",", compilationUnit.GetUsings()));

                var usingStrings = RoslynStringBuilders.BuildUsingStrings(compilationUnit.GetUsings());

                var fieldNodes = declaringClass.GetFields();
                foreach (var field in fieldNodes)
                {
                    this.Log.Add($"{field.ToFullString()} : type : {field.GetType()}");

                    names.Add(field.GetFieldName());
                    types.Add(field.GetFieldType()); // note that this is not the full type.

                    this.Log.Add(field.Declaration.Type.ToString());
                }

                // Build up list of parameters
                var parameterList = "";
                for (int i = 0; i < names.Count - 1; i++)
                {
                    parameterList += $"{types[i]} {names[i].ToCamelCase()}, ";
                }

                parameterList += $"{types.Last()} {names.Last().ToCamelCase()}";

                var assignmentList = "";
                for (int i = 0; i < names.Count; i++)
                {
                    assignmentList += $"this.{names[i]} = {names[i].ToCamelCase()}; \n";
                }

                var declaringClassName = declaringClass.GetClassName();
                context.AddSource($"{declaringClassName}.cs", SourceText.From($@"

namespace {classNamespace} {{
{usingStrings}

    public partial class {declaringClassName} {{

    public {declaringClassName}({parameterList})
            {{
                {assignmentList}
            }}

    }}

}}
", Encoding.UTF8));
            }
        }
    }
}
