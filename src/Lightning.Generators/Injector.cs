using Lightning.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Lightning
{
    [Generator]
    internal class CombineGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG && false
            if (!System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Launch();
#endif
            context.RegisterForSyntaxNotifications(() => new InjectSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not InjectSyntaxReceiver receiver)
                return;

            foreach (var @class in receiver.GetSymbols(context))
            {
                MemberDeclarationSyntax? type = @class.Rewrite();
                if (type is null)
                    continue;

                var @namespace = @class.ContainingNamespace?.Rewrite()
                    ?.WithMembers(List(new[] { type }));

                var sourceText = CompilationUnit()
                    .WithUsings(List(@class.GetUsingDirectives()))
                    .WithMembers(List(new[] { @namespace ?? type }))
                    .WithLeadingTrivia(TriviaList(Trivia(PragmaWarningDirectiveTrivia(Token(DisableKeyword), true))))
                    .NormalizeWhitespace()
                    .ToFullString();

                context.AddSource($"Lightning_{@class.MetadataName}.cs", sourceText);
            }
        }
    }
}
