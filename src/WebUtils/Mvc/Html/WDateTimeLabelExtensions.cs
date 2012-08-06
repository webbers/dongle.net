using System;
using System.Web.Mvc;
using WebUtils.System;

namespace WebUtils.Mvc.Html
{
    public static class WDateTimeSpanExtensions
    {
        public static MvcHtmlString WDateTimeSpan(this HtmlHelper htmlHelper, DateTime datetime)
        {
            var tagBuilder = new TagBuilder("span")
                                 {
                                     InnerHtml = datetime.ToFriendlyString()
                                 };
            tagBuilder.MergeAttribute("title", datetime.ToString());
            tagBuilder.AddCssClass("wdatetimespan");
            return new MvcHtmlString(tagBuilder.ToString());
        }
    }
}
