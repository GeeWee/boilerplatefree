using Microsoft.CodeAnalysis.CSharp;

#pragma warning disable 1591
namespace BoilerplateFree
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class RoslynExtensions
    {
        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
        {
            var current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol type)
        {
            return type.GetBaseTypesAndThis().SelectMany(n => n.GetMembers());
        }

        public static CompilationUnitSyntax GetCompilationUnit(this SyntaxNode syntaxNode)
        {
            return syntaxNode.Ancestors().OfType<CompilationUnitSyntax>().FirstOrDefault();
        }

        public static string GetClassName(this ClassDeclarationSyntax proxy)
        {
            return proxy.Identifier.Text;
        }

        public static IEnumerable<FieldDeclarationSyntax> GetFields(this ClassDeclarationSyntax declaringClass)
        {
            var nodes = declaringClass.DescendantNodes().OfType<FieldDeclarationSyntax>();
            return nodes;
        }

        public static string GetFieldName(this FieldDeclarationSyntax declaringField)
        {
            var name = declaringField.Declaration.Variables[0].Identifier.ToFullString();
            return name;
        }

        public static string GetFieldType(this FieldDeclarationSyntax declaringField)
        {
            var name = declaringField.Declaration.Type.ToFullString();
            return name;
        }

        public static string GetClassModifier(this ClassDeclarationSyntax proxy)
        {
            return proxy.Modifiers.ToFullString().Trim();
        }

        public static bool HaveAttribute(this ClassDeclarationSyntax classSyntax, string attributeName)
        {
            return classSyntax.AttributeLists.Count > 0 &&
                   classSyntax.AttributeLists.SelectMany(al => al.Attributes
                           .Where(a => ((a.Name as IdentifierNameSyntax))?.Identifier.Text == attributeName))
                       .Any();
        }


        public static string GetNamespace(this CompilationUnitSyntax root)
        {
            return root.ChildNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault()
                .Name
                .ToString();
        }

        public static List<string> GetUsingsInsideNamespace(this CompilationUnitSyntax root)
        {
            return root.DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .SelectMany(n => n.DescendantNodes())
                .OfType<UsingDirectiveSyntax>()
                .Select(n => n.Name.ToString())
                .ToList();
        }

        public static List<string> GetUsingsOutsideNamespace(this CompilationUnitSyntax root)
        {
            return root.ChildNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(n => n.Name.ToString())
                .ToList();
        }

        public static IEnumerable<T> GetWithPublicKeyword<T>(this IEnumerable<T> unfilteredTokens)
            where T : MemberDeclarationSyntax
        {
            var publicNodes = unfilteredTokens.Where(property =>
                property.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.PublicKeyword));
            return publicNodes;
        }

        public static IEnumerable<T> GetWithoutStaticKeyword<T>(this IEnumerable<T> unfilteredTokens)
            where T : MemberDeclarationSyntax
        {
            var nonStaticNodes = unfilteredTokens.Where(property => property.Modifiers.All(modifier =>
                modifier.Kind() != SyntaxKind.StaticKeyword));
            return nonStaticNodes;
        }
        
        // Remove all fields that have an initializer already set (aka myVarName = "foo")
        public static IEnumerable<FieldDeclarationSyntax> GetUnInitialized(this IEnumerable<FieldDeclarationSyntax> unfilteredTokens)
        {
            return unfilteredTokens.Where(token => token.Declaration.Variables.All((var) => var.Initializer == null));
        }
    }
}