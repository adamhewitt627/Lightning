using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Lightning.Extensions
{
    internal static class MethodExtensions
    {
        public static BaseMethodDeclarationSyntax? Rewrite(this IMethodSymbol symbol)
        {
            if (symbol.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is BaseMethodDeclarationSyntax declaration)
                return declaration.Rewrite();
            
            return null;
        }

        public static BaseMethodDeclarationSyntax? Rewrite(this BaseMethodDeclarationSyntax declaration)
        {
            var parameters = declaration.ParameterList.Parameters;

            var toInject = parameters.Where(p => p.HasInjectAttribute()).ToList();
            if (toInject.Count == 0)
                return null;

            declaration = declaration
                .WithParameterList(ParameterList(SeparatedList(parameters.Except(toInject))))
                .WithExpressionBody(null)
                .WithBody(null);


            return declaration switch
            {
                ConstructorDeclarationSyntax ctor => ctor
                    .WithInitializer(ConstructorInitializer(ThisConstructorInitializer)
                        .WithArgumentList(parameters.ToArgumentList()))
                        .WithBody(Block()),

                MethodDeclarationSyntax method => method
                    .WithExpressionBody(ArrowExpressionClause(InvocationExpression(
                        expression: MemberAccessExpression(SimpleMemberAccessExpression,
                            ThisExpression(), IdentifierName(method.Identifier)),
                        argumentList: parameters.ToArgumentList()
                    ))),

                _ => declaration,
            };
        }


        private static ArgumentListSyntax ToArgumentList(this IEnumerable<ParameterSyntax> parameters)
            => ArgumentList(SeparatedList(parameters.Select(p => p.ToArgument())));
    }
}
