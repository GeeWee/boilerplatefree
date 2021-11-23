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
                this.Log.Add(e.ToString());
                if (Environment.GetEnvironmentVariable("BOILERPLATEFREE_TEST") != "1")
                {
                    throw;
                }
            }
            finally
            {
                context.AddSource("Logs",
                    SourceText.From(
                        $@"/*{Environment.NewLine + string.Join(Environment.NewLine, this.Log) + Environment.NewLine}*/",
                        Encoding.UTF8));
            }
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

                var classMethods = GetPublicClassMethods(declaringClass);

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

        private string GetPublicClassMethods(ClassDeclarationSyntax declaringClass)
        {
            string classMethodsString = "";

            var publicNodes = declaringClass.ChildNodes().OfType<MethodDeclarationSyntax>()
                .GetWithPublicKeyword()
                .GetWithoutStaticKeyword();

            foreach (var methodDeclarationSyntax in publicNodes)
            {
                this.Log.Add(methodDeclarationSyntax.Identifier.ToFullString());

                this.Log.Add(methodDeclarationSyntax.ToFullString());

                // this is hacky as fuck
                // Split on first ocurrence of ) which is probably the method end.
                classMethodsString += methodDeclarationSyntax.ToFullString().Split(')')[0] + "); \n";
            }

            return classMethodsString;
        }

        private string GetPublicProperties(ClassDeclarationSyntax declaringClass)
        {
            string propertiesString = "";

            var publicProperties = declaringClass.ChildNodes()
                .OfType<PropertyDeclarationSyntax>()
                .GetWithPublicKeyword()
                .GetWithoutStaticKeyword();


            foreach (var propertyDeclarationSyntax in publicProperties)
            {
                this.Log.Add("Property " + propertyDeclarationSyntax.ToFullString());

                var hasGetter =
                    propertyDeclarationSyntax.AccessorList?.Accessors.FirstOrDefault(x =>
                        x.IsKind(SyntaxKind.GetAccessorDeclaration)) != null;

                var hasExpressionBody = propertyDeclarationSyntax.ExpressionBody != null;


                var hasSetter =
                    propertyDeclarationSyntax.AccessorList?.Accessors.FirstOrDefault(x =>
                        x.IsKind(SyntaxKind.SetAccessorDeclaration)) != null;

                var getterSetterString = "";
                if (hasGetter || hasExpressionBody)
                {
                    getterSetterString += "get; ";
                }

                if (hasSetter)
                {
                    getterSetterString += "set; ";
                }

                var fullString =
                    $"public {propertyDeclarationSyntax.Type.ToFullString()} {propertyDeclarationSyntax.Identifier.ToFullString()} {{{getterSetterString}}}";


                this.Log.Add("Property " + fullString);

                // this is hacky as fuck
                // Split on first ocurrence of ) which is probably the method end.
                propertiesString += fullString + "\n";
            }


            return propertiesString;
        }
    }
}