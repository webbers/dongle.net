using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Dongle.Web.Html
{
    public static class WSpinButtonExtensions
    {
        public static MvcHtmlString WSpinButton(this HtmlHelper htmlHelper, string name, object htmlAttributes = null)
        {
            return WSpinButton(htmlHelper, name, null, htmlAttributes);
        }

        public static MvcHtmlString WSpinButton(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes = null)
        {
            return htmlHelper.TextBox(name, value, MakeAttributes(htmlAttributes));
        }

        public static MvcHtmlString WSpinButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return htmlHelper.TextBoxFor(expression, MakeAttributes(htmlAttributes));
        }

        private static IDictionary<string, object> MakeAttributes(object htmlAttributes)
        {
            var attr = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attr.Add("class", "wspinbutton");
            return attr;
        }
    }
}