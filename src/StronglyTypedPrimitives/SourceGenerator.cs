using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StronglyTypedPrimitives
{
    public sealed record StructToGenerate(Template Template, string Name, string Namespace)
    {
        public string Namespace { get; } = Namespace;
        public string Name { get; } = Name;
        public Template Template { get; } = Template;

        public static StructToGenerate Empty { get; } =
            new(Template.Guid, string.Empty, string.Empty);
    }

    [Generator]
    public class SourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // context.RegisterPostInitializationOutput(i =>
            //     i.AddSource("StronglyTypedAttribute.g.cs", GetAttributeSource())
            // );

            var structProviders = context
                .SyntaxProvider.CreateSyntaxProvider(
                    predicate: static (node, _) => node is StructDeclarationSyntax,
                    transform: static (syntaxContext, _) =>
                    {
                        var structDeclaration = (StructDeclarationSyntax)syntaxContext.Node;

                        if (
                            syntaxContext.SemanticModel.GetDeclaredSymbol(structDeclaration)
                            is not INamedTypeSymbol structType
                        )
                        {
                            return StructToGenerate.Empty;
                        }

                        var attribute = structType
                            .GetAttributes()
                            .FirstOrDefault(a =>
                                a.AttributeClass?.Name == "StronglyTypedAttribute"
                            );

                        if (attribute?.ConstructorArguments is not { IsEmpty: false } args)
                        {
                            return StructToGenerate.Empty;
                        }

                        var templateArg = args[0];
                        if (templateArg is not { Kind: TypedConstantKind.Enum, Value: not null })
                        {
                            return StructToGenerate.Empty;
                        }

                        var template = (Template)templateArg.Value;
                        return new StructToGenerate(
                            template,
                            structType.Name,
                            GetNameSpace(structDeclaration)
                        );
                    }
                )
                .Collect();

            context.RegisterSourceOutput(structProviders, Execute);
        }

        private static string GetAttributeSource()
        {
            var resourceStream = typeof(SourceGenerator).Assembly.GetManifestResourceStream(
                "StronglyTypedPrimitives.Templates.strongly-typed-attribute"
            );
            if (resourceStream is null)
            {
                throw new InvalidOperationException("Could not find embedded resource");
            }

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        private static void Execute(
            SourceProductionContext context,
            ImmutableArray<StructToGenerate> structTypeNames
        )
        {
            foreach (var structToGenerate in structTypeNames.Distinct())
            {
                if (structToGenerate == StructToGenerate.Empty)
                {
                    continue;
                }

                var source = GetTemplate(structToGenerate);
                context.AddSource($"{structToGenerate.Name}.g.cs", source);
            }
        }

        private static string GetTemplate(StructToGenerate structToGenerate) =>
            GetTemplateResource(structToGenerate.Template)
                .Replace("PLACEHOLDER", structToGenerate.Name)
                .Replace("NAMESPACE", structToGenerate.Namespace);

        private static string GetTemplateResource(Template template)
        {
            var resourceStream = typeof(SourceGenerator).Assembly.GetManifestResourceStream(
                $"StronglyTypedPrimitives.Templates.strongly-typed-primitive-{template.ToString().ToLower()}"
            );
            if (resourceStream is null)
            {
                throw new InvalidOperationException("Could not find embedded resource");
            }

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        private static string GetNameSpace(StructDeclarationSyntax structSymbol)
        {
            // determine the namespace the struct is declared in, if any
            var potentialNamespaceParent = structSymbol.Parent;
            while (
                potentialNamespaceParent != null
                && potentialNamespaceParent is not NamespaceDeclarationSyntax
                && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax
            )
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            if (potentialNamespaceParent is not BaseNamespaceDeclarationSyntax namespaceParent)
            {
                return string.Empty;
            }

            var nameSpace = namespaceParent.Name.ToString();
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax namespaceParentParent)
                {
                    break;
                }

                namespaceParent = namespaceParentParent;
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
            }

            return nameSpace;
        }
    }
}
