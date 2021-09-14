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
    public class AutoInterfaceGenerator: ISourceGenerator
    {
        private AttributeClassSyntaxReceiver classSyntaxReceiver = null!;
        public List<string> Log { get; } = new();

        public void Initialize(GeneratorInitializationContext context)
        {
            this.classSyntaxReceiver = new AttributeClassSyntaxReceiver(this.Log, "AutoGenerateInterface");

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

                var usings = RoslynStringBuilders.BuildUsingStrings(compilationUnit.GetUsings());

                this.Log.Add($"Namespace: " + classNamespace);



                string classMethods = "";

                var nodes = declaringClass.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var methodDeclarationSyntax in nodes)
                {
                    this.Log.Add(methodDeclarationSyntax.Identifier.ToFullString());

                    this.Log.Add(methodDeclarationSyntax.ToFullString());

                    // this is hacky as fuck
                    // Split on first ocurrence of ) which is probably the method end.
                    classMethods += methodDeclarationSyntax.ToFullString().Split(')')[0] + "); \n";
                }

                var declaringClassName = declaringClass.GetClassName();
                context.AddSource($"I{declaringClassName}.cs", SourceText.From($@"

namespace {classNamespace} {{
    {usings}

    public interface I{declaringClassName} {{

    {classMethods}

    }}
}}
", Encoding.UTF8));
            }
        }
    }

}
