using System;
using System.Linq.Expressions;
using System.Resources;
using System.Web.Mvc;

namespace WebUtils.Mvc.Html
{
    public static class WResourceLabelExtensions
    {
        private static ResourceManager _resourceManager;

        public static void SetResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public static MvcHtmlString WResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            var innerText = metadata.DisplayName;
            if (innerText == null)
            {
                innerText = metadata.PropertyName;

                if (_resourceManager != null)
                {
                    var resstr = _resourceManager.GetString(innerText);

                    if (!String.IsNullOrEmpty(resstr))
                    {
                        innerText = resstr;
                    }
                }
            }

            var tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(innerText);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}