using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Lightning.Extensions
{
    internal static class NamespaceExtensions
    {
        public static NamespaceDeclarationSyntax Rewrite(this INamespaceSymbol symbol)
        {
            return NamespaceDeclaration(IdentifierName(string.Join(".", name(symbol).Reverse())));

            static IEnumerable<string> name(INamespaceSymbol @namespace)
            {
                while (@namespace is { } && !@namespace.IsGlobalNamespace)
                {
                    yield return @namespace.Name;
                    @namespace = @namespace.ContainingNamespace;
                }
            }
        }

    }
}
