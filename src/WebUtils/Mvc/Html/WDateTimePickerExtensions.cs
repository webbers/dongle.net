using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebUtils.Mvc.Html
{
    public static class WDateTimePickerExtensions
    {
        public static MvcHtmlString WDateTimePicker(this HtmlHelper htmlHelper, string name, object htmlAttributes = null)
        {
            return WDateTimePicker(htmlHelper, name, null, htmlAttributes);
        }

        public static MvcHtmlString WDateTimePicker(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes = null)
        {
            var modelMetadata = ModelMetadata.FromStringExpression(name, htmlHelper.ViewContext.ViewData);
            if (value != null)
            {
                modelMetadata.Model = value;
            }
            return WDateTimePickerHelper(htmlHelper, modelMetadata, name, htmlAttributes);
        }

        public static MvcHtmlString WDateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return WDateTimePickerHelper(htmlHelper, ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression), htmlAttributes);
        }

        private static MvcHtmlString WDateTimePickerHelper(HtmlHelper htmlHelper, ModelMetadata modelMetadata, string name, object htmlAttributes = null)
        {
            var attr = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attr.Add("class", "wdatetimepicker");
            return htmlHelper.TextBox(name, modelMetadata.Model, attr);
        }
    }
}
