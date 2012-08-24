using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;

namespace Dongle.Web.Html
{
    public enum WButtonType
    {
        Default,
        Action,
        Alert
    }

    public static class WButtonExtensions
    {
        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, WButtonType type, object htmlAttributes = null)
        {
            return WButton(html, name, title, null, type, false, htmlAttributes);
        }

        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, object htmlAttributes = null)
        {
            return WButton(html, name, title, null, WButtonType.Default, false, htmlAttributes);
        }

        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, bool submit, object htmlAttributes = null)
        {
            return WButton(html, name, title, null, WButtonType.Default, submit, htmlAttributes);
        }

        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, string icon, object htmlAttributes = null)
        {
            return WButton(html, name, title, icon, WButtonType.Default, false, htmlAttributes);
        }

        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, string icon, bool submit, object htmlAttributes = null)
        {
            return WButton(html, name, title, icon, WButtonType.Default, submit, htmlAttributes);
        }

        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, string icon, WButtonType type, object htmlAttributes = null)
        {
            return WButton(html, name, title, icon, type, false, htmlAttributes);
        }

        public static MvcHtmlString WButton(this HtmlHelper html, string name, string title, string icon, WButtonType type, bool submit, object htmlAttributes = null)
        {
            return WButtonHelper(name, null, title, icon, type, submit, htmlAttributes);
        }
        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, WButtonType type, object htmlAttributes = null)
        {
            return WLinkButton(html, href, title, null, type, false, htmlAttributes);
        }

        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, object htmlAttributes = null)
        {
            return WLinkButton(html, href, title, null, WButtonType.Default, false, htmlAttributes);
        }

        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, bool submit, object htmlAttributes = null)
        {
            return WLinkButton(html, href, title, null, WButtonType.Default, submit, htmlAttributes);
        }

        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, string icon, object htmlAttributes = null)
        {
            return WLinkButton(html, href, title, icon, WButtonType.Default, false, htmlAttributes);
        }

        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, string icon, bool submit, object htmlAttributes = null)
        {
            return WLinkButton(html, href, title, icon, WButtonType.Default, submit, htmlAttributes);
        }

        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, string icon, WButtonType type, object htmlAttributes = null)
        {
            return WLinkButton(html, href, title, icon, type, false, htmlAttributes);
        }

        public static MvcHtmlString WLinkButton(this HtmlHelper html, string href, string title, string icon, WButtonType type, bool submit, object htmlAttributes = null)
        {
            return WButtonHelper(null, href, title, icon, type, submit, htmlAttributes);
        }

        private static MvcHtmlString WButtonHelper(string name, string href, string title, string icon, WButtonType type, bool submit, object htmlAttributes = null)
        {
            var dictionary = (IDictionary<string, object>)AnonymousObjectToHtmlAttributes(htmlAttributes);

            TagBuilder tagBuilder;

            if (href != null)
            {
                tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", href);
            }
            else
            {
                tagBuilder = new TagBuilder("div");
            }

            tagBuilder.MergeAttributes(dictionary);

            if (name != null)
            {
                tagBuilder.MergeAttribute("id", name);
                tagBuilder.MergeAttribute("name", name);
            }

            tagBuilder.AddCssClass("wbutton");
            tagBuilder.InnerHtml = title;

            if (submit)
            {
                tagBuilder.AddCssClass("submit");
            }

            if (type == WButtonType.Alert)
            {
                tagBuilder.AddCssClass("wbutton-alert");
            }
            else if (type == WButtonType.Action)
            {
                tagBuilder.AddCssClass("wbutton-action");
            }
            else
            {
                tagBuilder.AddCssClass("wbutton-normal");
            }

            if (!String.IsNullOrEmpty(icon))
            {
                tagBuilder.MergeAttribute("icon", icon);
            }
            return new MvcHtmlString(tagBuilder.ToString());
        }

        private static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            var routeValueDictionary = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(htmlAttributes))
                    routeValueDictionary.Add(propertyDescriptor.Name.Replace('_', '-'), propertyDescriptor.GetValue(htmlAttributes));
            }
            return routeValueDictionary;
        }
    }
}