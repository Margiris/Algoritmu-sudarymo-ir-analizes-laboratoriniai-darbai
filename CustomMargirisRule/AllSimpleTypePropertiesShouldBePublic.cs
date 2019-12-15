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

        public override ProblemCollection Check(Parameter parameter)
        {
            if (parameter.Type.IsPrimitive && parameter.Type.IsNonPublic)
            {
                var resolution = GetResolution(parameter.Name.Name);
                Problems.Add(new Problem(resolution, parameter)
                {
                    Certainty = 100,
                    FixCategory = FixCategories.NonBreaking,
                    MessageLevel = MessageLevel.Warning
                });
            }

            return Problems;
        }
    }
}