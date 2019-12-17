using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MargirisRule
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MargirisRuleCodeFixProvider)), Shared]
    public class MargirisRuleCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Make public";

        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(MargirisRuleAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var fieldDeclaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<FieldDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    Title,
                    c => MakeUppercaseAsync(context.Document, fieldDeclaration, c),
                    nameof(MargirisRuleCodeFixProvider)),
                diagnostic);
        }

        private async Task<Document> MakeUppercaseAsync(Document document, FieldDeclarationSyntax fieldDeclaration,
            CancellationToken cancellationToken)
        {
            // Access modifiers we target
            var accessModifiers = new[]
            {
                SyntaxKind.PrivateKeyword,
                SyntaxKind.ProtectedKeyword,
                SyntaxKind.InternalKeyword
            };

            var token = fieldDeclaration.GetFirstToken();
            // Save trivia to not "eat up" any newlines and indentation in front of declaration
            var leadingTrivia = token.LeadingTrivia;
            var trimmedModifiers = fieldDeclaration.Modifiers;

            // Remove access modifiers from declaration if any
            while (accessModifiers.Contains(token.Kind()))
            {
                trimmedModifiers = trimmedModifiers.RemoveAt(0);
                token = token.GetNextToken();
            }

            // Create a public keyword modifier with leading trivia and add it to modifiers
            var newModifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(leadingTrivia, SyntaxKind.PublicKeyword,
                SyntaxFactory.TriviaList(SyntaxFactory.ElasticMarker))).AddRange(trimmedModifiers);
            var newFieldDeclaration = fieldDeclaration.WithModifiers(newModifiers);

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(fieldDeclaration, newFieldDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}