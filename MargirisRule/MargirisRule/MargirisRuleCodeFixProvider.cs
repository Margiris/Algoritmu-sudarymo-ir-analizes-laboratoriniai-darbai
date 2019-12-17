using System;
using System.Collections.Generic;
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
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace MargirisRule
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MargirisRuleCodeFixProvider)), Shared]
    public class MargirisRuleCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Make public";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(MargirisRuleAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var fieldDeclarationNodes = root.FindToken(diagnosticSpan.Start).Parent.ChildNodes()
                .OfType<FieldDeclarationSyntax>();

            // Register a code action that will invoke the fix.
            foreach (var fieldDeclaration in fieldDeclarationNodes)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Title,
                        createChangedDocument: c => MakeUppercaseAsync(context.Document, fieldDeclaration, c),
                        equivalenceKey: Title),
                    diagnostic);
            }
        }

        private async Task<Document> MakeUppercaseAsync(Document document, FieldDeclarationSyntax fieldDeclaration,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var newModifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddRange(fieldDeclaration.Modifiers);
            var newFieldDeclaration = fieldDeclaration.WithModifiers(newModifiers);
            var newRoot = root.ReplaceNode(fieldDeclaration, newFieldDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
    }
}