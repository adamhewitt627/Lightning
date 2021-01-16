using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text.RegularExpressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Lightning.Extensions
{
    internal static class ParameterExtensions
    {
        public static bool HasInjectAttribute(this ParameterSyntax parameter) => parameter
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .Any(x => Regex.IsMatch(x.Name.ToFullString(), @"\.?Inject$"));

        public static ArgumentSyntax ToArgument(this ParameterSyntax param)
        {
            return param.HasInjectAttribute()
                ? param.Rewrite()
                : Argument(IdentifierName(param.Identifier));
        }

        private static ArgumentSyntax Rewrite(this ParameterSyntax param)
        {
            var invocation = InvocationExpression(
                MemberAccessExpression(
                    SimpleMemberAccessExpression,
                    IdentifierName($"global::Lightning.LightningDI"),
                    GenericName(Identifier("Resolve"))
                        .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList(param.Type!)))));

            return Argument(invocation);
        }
    }
}
