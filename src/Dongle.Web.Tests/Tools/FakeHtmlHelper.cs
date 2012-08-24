using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Dongle.Web.Tests.Tools
{
    public class FakeHttpContext : HttpContextBase
    {
        private readonly Dictionary<object, object> _items = new Dictionary<object, object>();
        public override IDictionary Items { get { return _items; } }
    }

    public class FakeViewDataContainer : IViewDataContainer
    {
        private ViewDataDictionary _viewData = new ViewDataDictionary();
        public ViewDataDictionary ViewData { get { return _viewData; } set { _viewData = value; } }
    }
    public class FakeHtmlHelper<T> : HtmlHelper<T>
    {
        new static readonly ViewContext ViewContext = new ViewContext { HttpContext = new FakeHttpContext(), ViewData = new ViewDataDictionary(new FakeViewDataContainer()) };
        public FakeHtmlHelper()
            : base(ViewContext, new FakeViewDataContainer())
        {
        }
    }
}
