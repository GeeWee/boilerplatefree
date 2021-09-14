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
            foreach (var declaringClass in this.classSyntaxReceiver.ClassesToGenerateFor)
            {
                var names = new List<string>();
                var types = new List<string>();

                var usings = new List<string>();

                var compilationUnit = declaringClass.GetCompilationUnit();

                var classNamespace = compilationUnit.GetNamespace();

                this.Log.Add($"Namespace: " + classNamespace);
                this.Log.Add($"usings:" + declaringClass.GetCompilationUnit().Usings.ToFullString());

                var nodes = declaringClass.DescendantNodes().OfType<FieldDeclarationSyntax>();
                foreach (var propertyNode in nodes)
                {
                    this.Log.Add(propertyNode.ToFullString());
                    this.Log.Add(propertyNode.GetType().ToString());

                    var name = propertyNode.Declaration.Variables[0].Identifier.ToFullString();


                    // not the full type
                    var classType = propertyNode.Declaration.Type.ToFullString();

                    names.Add(name);
                    types.Add(classType);

                    this.Log.Add(classType);

                    this.Log.Add(propertyNode.Declaration.Type.ToString());
                }

                var parameterList = "";
                for (int i = 0; i < names.Count - 1; i++)
                {
                    parameterList += $"{types[i]} {names[i]},";
                }

                parameterList += $"{types.Last()} {names.Last()}";

                var assignmentList = "";
                for (int i = 0; i < names.Count; i++)
                {
                    assignmentList += $"this.{names[i]} = {names[i]}; \n";
                }

                var declaringClassName = declaringClass.GetClassName();
                context.AddSource($"{declaringClassName}.cs", SourceText.From($@"

namespace {classNamespace} {{

    public partial class {declaringClassName} {{

    public {declaringClassName}({parameterList})
            {{
                {assignmentList}
            }}

    }}

}}


", Encoding.UTF8));
            }


            context.AddSource("Logs",
                SourceText.From(
                    $@"/*{Environment.NewLine + string.Join(Environment.NewLine, this.Log) + Environment.NewLine}*/",
                    Encoding.UTF8));
        }
    }
}
