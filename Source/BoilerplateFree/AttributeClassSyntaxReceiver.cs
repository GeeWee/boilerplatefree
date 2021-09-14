namespace BoilerplateFree
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal class AttributeClassSyntaxReceiver : ISyntaxReceiver
    {
        private readonly string attributeName;
        public List<string> Log { get; }
        public List<ClassDeclarationSyntax> ClassesToGenerateFor { get; } = new List<ClassDeclarationSyntax>();

        public AttributeClassSyntaxReceiver(List<string> log, string attributeName)
        {
            this.attributeName = attributeName;
            this.Log = log;
        }


        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Log.Add(syntaxNode.GetType().ToString());

            if (syntaxNode is ClassDeclarationSyntax classSyntax)
            {
                Log.Add($"Class: " + classSyntax.GetClassName());

                if (classSyntax.HaveAttribute(this.attributeName))
                {
                    this.Log.Add($"Class got dat good attribute: " + classSyntax.GetClassName());
                    this.ClassesToGenerateFor.Add(classSyntax);
                }
            }
        }
    }
}
