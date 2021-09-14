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
        public List<string> Log { get; } = new List<string>();

        public void Initialize(GeneratorInitializationContext context)
        {
            this.classSyntaxReceiver = new AttributeClassSyntaxReceiver(this.Log, "AutoGenerateInterface");

#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif

            context.RegisterForSyntaxNotifications(() =>
            {
                return this.classSyntaxReceiver;
            });

            // Log.Add("test?");
            // Debug.WriteLine("Execute code generator");
            // Console.WriteLine("WOOW");
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

    public interface I{declaringClassName} {{

    {classMethods}

    }}
}}


", Encoding.UTF8));
            }

            context.AddSource("Logs2", SourceText.From($@"/*{ Environment.NewLine + string.Join(Environment.NewLine, this.Log) + Environment.NewLine}*/", Encoding.UTF8));

        }
    }

}
