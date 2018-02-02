using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FISH.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "asp-for")]
    public class TagExtensions : InputTagHelper
    {
        public TagExtensions(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            var validatorMetadata = For.ModelExplorer.Metadata.ValidatorMetadata;
            output.Attributes.Remove(output.Attributes["type"]);
            for (var i = 0; i < validatorMetadata.Count; i++)
            {
                var stringRegularExpressionAttribute = validatorMetadata[i] as RegularExpressionAttribute;
                var stringRequiredAttribute = validatorMetadata[i] as RequiredAttribute;
                if (stringRegularExpressionAttribute != null)
                {
                    if (output.Attributes["data-val"] == null)
                    {
                        output.Attributes.Add("data-val", "true");
                    }
                    output.Attributes.Add("data-val-regex", stringRegularExpressionAttribute.ErrorMessage);
                    output.Attributes.Add("data-val-regex-pattern", stringRegularExpressionAttribute.Pattern);
                }
                if (stringRequiredAttribute != null)
                {
                    if (output.Attributes["data-val"] == null)
                    {
                        output.Attributes.Add("data-val", "true");
                    }
                    output.Attributes.Add("data-val-required", stringRequiredAttribute.ErrorMessage);
                }
            }
        }

    }
}
