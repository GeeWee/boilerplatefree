// ReSharper disable SA1600

using Microsoft.CodeAnalysis.CSharp;

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
    public class AutoInterfaceGenerator : ISourceGenerator
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

            context.AddSource("Logs",
                SourceText.From(
                    $@"/*{Environment.NewLine + string.Join(Environment.NewLine, this.Log) + Environment.NewLine}*/",
                    Encoding.UTF8));
        }


        public void ExecuteInTryCatch(GeneratorExecutionContext context)
        {
            foreach (var declaringClass in this.classSyntaxReceiver.ClassesToGenerateFor)
            {
                var compilationUnit = declaringClass.GetCompilationUnit();

                var classNamespace = compilationUnit.GetNamespace();

                var usingsInsideNamespace =
                    RoslynStringBuilders.BuildUsingStrings(compilationUnit.GetUsingsInsideNamespace());
                var usingsOutsideNamespace =
                    RoslynStringBuilders.BuildUsingStrings(compilationUnit.GetUsingsOutsideNamespace());

                this.Log.Add($"Namespace: " + classNamespace);

                var publicProperties = GetPublicProperties(declaringClass);

                var classMethods = GetClassMethods(declaringClass);

                var declaringClassName = declaringClass.GetClassName();
                context.AddSource($"I{declaringClassName}.cs", SourceText.From($@"
{usingsOutsideNamespace}
namespace {classNamespace} {{
{usingsInsideNamespace}

    public interface I{declaringClassName} {{

    {classMethods}

    {publicProperties}

    }}
}}
", Encoding.UTF8));
            }
        }

        private string GetClassMethods(ClassDeclarationSyntax declaringClass)
        {
            string classMethods = "";

            var nodes = declaringClass.ChildNodes().OfType<MethodDeclarationSyntax>();

            var publicNodes = nodes.Where(property =>
                property.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.PublicKeyword));

            foreach (var methodDeclarationSyntax in publicNodes)
            {
                this.Log.Add(methodDeclarationSyntax.Identifier.ToFullString());

                this.Log.Add(methodDeclarationSyntax.ToFullString());

                // this is hacky as fuck
                // Split on first ocurrence of ) which is probably the method end.
                classMethods += methodDeclarationSyntax.ToFullString().Split(')')[0] + "); \n";
            }

            return classMethods;
        }

        private string GetPublicProperties(ClassDeclarationSyntax declaringClass)
        {
            string properties = "";

            var nodes = declaringClass.ChildNodes().OfType<PropertyDeclarationSyntax>();

            var publicNodes = nodes.Where(property =>
                property.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.PublicKeyword));


            foreach (var propertyDeclarationSyntax in publicNodes)
            {
                this.Log.Add("Property " + propertyDeclarationSyntax.Identifier.ToFullString());

                this.Log.Add("Property " + propertyDeclarationSyntax.ToFullString());

                var getter =
                    propertyDeclarationSyntax.AccessorList?.Accessors.FirstOrDefault(x =>
                        x.IsKind(SyntaxKind.GetAccessorDeclaration));

                var hasExpressionBody = propertyDeclarationSyntax.ExpressionBody != null;

                Log.Add("ExpressionBody: " + propertyDeclarationSyntax.ExpressionBody?.ToFullString() ??
                        "no expression body");


                var setter =
                    propertyDeclarationSyntax.AccessorList?.Accessors.FirstOrDefault(x =>
                        x.IsKind(SyntaxKind.SetAccessorDeclaration));

                var getterSetterString = "";
                if (getter != null || hasExpressionBody)
                {
                    getterSetterString += "get; ";
                }
                if (setter != null)
                {
                    getterSetterString += "set; ";
                }

                var fullString =
                    $"public {propertyDeclarationSyntax.Type.ToFullString()} {propertyDeclarationSyntax.Identifier.ToFullString()} {{{getterSetterString}}}";


                this.Log.Add("Property " + fullString);

                // this is hacky as fuck
                // Split on first ocurrence of ) which is probably the method end.
                properties += fullString + "\n";
            }


            return properties;
        }
    }
}