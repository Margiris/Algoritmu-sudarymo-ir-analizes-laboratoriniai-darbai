using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MargirisRule
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MargirisRuleAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MargirisRule";

        private static readonly string Title = "Accessibility rule violation";
        private static readonly string MessageFormat = "Primitive non-constant field {0} should be public";
        // private static readonly string MessageFormat = "{0}";
        private static readonly string Description = "No need to hide simple things";
        private const string Category = "Visibility";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.FieldDeclaration);
        }

        private static void AnalyzeProperty(SyntaxNodeAnalysisContext context)
        {
            var field = (IFieldSymbol) context.ContainingSymbol;

            if (!field.IsConst && field.DeclaredAccessibility != Accessibility.Public && IsFieldPrimitive(field))
            {
                var diagnostic = Diagnostic.Create(Rule, field.Locations[0], field.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        internal static bool IsFieldPrimitive(IFieldSymbol field)
        {
            var primitiveFieldTypes = new List<string>
            {
                "bool",
                "byte",
                "sbyte",
                "char",
                "decimal",
                "double",
                "float",
                "int",
                "uint",
                "long",
                "ulong",
                "object",
                "short",
                "ushort",
                "string"
            };

            return primitiveFieldTypes.Contains(field.Type.ToDisplayString());
        }
    }
}