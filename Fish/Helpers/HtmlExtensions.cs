using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FISH.Helpers
{
    public static class HtmlExtensions
    {

        public static IHtmlContent Script(this IHtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return new HtmlString("");
        }
        public static IHtmlContent Script(this IHtmlHelper htmlHelper, string template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return new HtmlString("");
        }

        public static IHtmlContent RenderScripts(this IHtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    if (htmlHelper.ViewContext.HttpContext.Items[key] is Func<object, Microsoft.AspNetCore.Mvc.Razor.HelperResult> template)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                    if (htmlHelper.ViewContext.HttpContext.Items[key] is string stringTemplate)
                    {
                        htmlHelper.ViewContext.Writer.Write(stringTemplate);
                    }
                }
            }
            return new HtmlString("");
        }
    }

}
