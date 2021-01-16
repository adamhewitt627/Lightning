using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Lightning.Extensions
{
    internal static class TypeExtensions
    {
        public static TypeDeclarationSyntax? Rewrite(this ITypeSymbol symbol)
        {
            var @ref = symbol.DeclaringSyntaxReferences.First();
            if (@ref is null || @ref.GetSyntax() is not TypeDeclarationSyntax declaration)
                return null;

            var memberList = List(members().OfType<MemberDeclarationSyntax>());
            if (!memberList.Any())
                return null;

            declaration = declaration
                .WithMembers(memberList);

            declaration = declaration switch
            {
                RecordDeclarationSyntax record => record
                    .WithParameterList(null)
                    .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken))
                    .WithSemicolonToken(default),
                _ => declaration,
            };

            return declaration;

            IEnumerable<MemberDeclarationSyntax?> members()
            {
                var methods = symbol.GetMembers().OfType<IMethodSymbol>()
                    .Select(member => member.Rewrite());
                foreach (var m in methods)
                    yield return m;

                if (declaration is RecordDeclarationSyntax record && record.ParameterList is not null)
                {
                    yield return ConstructorDeclaration(declaration.Identifier)
                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                        .WithParameterList(record.ParameterList)
                        .Rewrite();
                }
            }
        }

        public static IEnumerable<UsingDirectiveSyntax> GetUsingDirectives(this ITypeSymbol symbol) => symbol.DeclaringSyntaxReferences
            .Select(@ref => @ref.GetSyntax().SyntaxTree)
            .SelectMany(tree => tree.GetCompilationUnitRoot().Usings);


    }
}
