using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.CodeAnalysis.Extensibility;

namespace CustomMargirisRule
{
    public class AllSimpleTypePropertiesShouldBePublic : BaseIntrospectionRule
    {
        public AllSimpleTypePropertiesShouldBePublic() :
            base(@"AllSimpleTypePropertiesShouldBePublic", "CustomMargirisRule.Rules",
                typeof(AllSimpleTypePropertiesShouldBePublic).Assembly)
        {
        }

        public override ProblemCollection Check(Member m)
        {
            PropertyNode property = (PropertyNode) m;
            if (!property.IsPublic)
            {
                var resolution = GetResolution(property.Name.Name);
                Problems.Add(new Problem(resolution, property)
                {
                    Certainty = 10
                });
            }
            if (property.Type.IsPrimitive && property.Type.IsNonPublic)
            {
                var resolution = GetResolution(property.Name.Name);
                Problems.Add(new Problem(resolution, property)
                {
                    Certainty = 100
                });
            }

            return Problems;
        }
    }
}