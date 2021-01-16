using Lightning.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Lightning
{
    internal class InjectSyntaxReceiver : ISyntaxReceiver
    {
        private readonly List<TypeDeclarationSyntax> _types = new List<TypeDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            //TODO - RecordDeclarationSyntax
            TypeDeclarationSyntax? declaration = syntaxNode switch
            {
                ClassDeclarationSyntax @class => @class,
                RecordDeclarationSyntax record => record,
                _ => null,
            };
            if (declaration is null)
                return;

            bool include = declaration.Members
                .OfType<BaseMethodDeclarationSyntax>()
                .SelectMany(m => m.ParameterList.Parameters)
                .Concat((declaration as RecordDeclarationSyntax)?.ParameterList?.Parameters ?? Enumerable.Empty<ParameterSyntax>())
                .Any(p => p.HasInjectAttribute());

            if (include)
                _types.Add(declaration);
        }

        public IEnumerable<INamedTypeSymbol> GetSymbols(GeneratorExecutionContext context) => _types
            .Select(c =>
            {
                var model = context.Compilation.GetSemanticModel(c.SyntaxTree, ignoreAccessibility: true);
                ISymbol? method = model?.GetDeclaredSymbol(c);

                return method;
            })
            .OfType<INamedTypeSymbol>()
            .Distinct();

    }
}
