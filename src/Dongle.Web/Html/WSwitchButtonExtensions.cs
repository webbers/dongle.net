using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dongle.Web.ModelAttributes;

namespace Dongle.Web.Html
{
    public static class WSwitchButtonExtensions
    {
        public static MvcHtmlString WSwitchButton(this HtmlHelper htmlHelper, string name, object htmlAttributes = null)
        {
            return WSwitchButton(htmlHelper, name, false, htmlAttributes);
        }

        public static MvcHtmlString WSwitchButton(this HtmlHelper htmlHelper, string name, bool @checked, object htmlAttributes = null)
        {
            return htmlHelper.CheckBox(name, @checked, MakeAttributes(htmlAttributes));
        }

        public static MvcHtmlString WSwitchButtonFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes = null)
        {
            var attr = MakeAttributes(htmlAttributes);

            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var property = metadata.ContainerType.GetProperty(metadata.PropertyName);
            var switchButtonAttribute = Attribute.GetCustomAttribute(property, typeof(WSwitchButtonAttribute), false) as WSwitchButtonAttribute;
            if (switchButtonAttribute != null)
            {
                attr.Add("yes", switchButtonAttribute.LabelYes);
                attr.Add("no", switchButtonAttribute.LabelNo);
            }

            return htmlHelper.CheckBoxFor(expression, attr);
        }

        private static IDictionary<string, object> MakeAttributes(object htmlAttributes)
        {
            var attr = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attr.Add("class", "wswitchbutton");
            return attr;
        }
    }


}
