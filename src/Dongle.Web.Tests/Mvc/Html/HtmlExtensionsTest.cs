using System;
using Dongle.Web.Html;
using Dongle.Web.Tests.Tools;
using NUnit.Framework;

namespace Dongle.Web.Tests.Mvc.Html
{
    [TestFixture]
    public class HtmlExtensionsTest
    {
        [Test]
        public void DateTimePicker()
        {
            var hh = new FakeHtmlHelper<Foo>();
            hh.ViewData.Model = new Foo
            {
                CreatedAt = new DateTime(2011, 8, 15, 12, 30, 15)
            };

            var html = hh.WDateTimePicker("abc", "123", new { bla = "blabla" });
            Assert.AreEqual("<input bla=\"blabla\" class=\"wdatetimepicker\" id=\"abc\" name=\"abc\" type=\"text\" value=\"123\" />", html.ToHtmlString());

            html = hh.WDateTimePicker("abc");
            Assert.AreEqual("<input class=\"wdatetimepicker\" id=\"abc\" name=\"abc\" type=\"text\" value=\"\" />", html.ToHtmlString());

            html = hh.WDateTimePickerFor(m=>m.CreatedAt);
            Assert.IsTrue(html.ToHtmlString() == "<input class=\"wdatetimepicker\" id=\"CreatedAt\" name=\"CreatedAt\" type=\"text\" value=\"15/08/2011 12:30:15\" />" ||
                          html.ToHtmlString() == "<input class=\"wdatetimepicker\" id=\"CreatedAt\" name=\"CreatedAt\" type=\"text\" value=\"8/15/2011 12:30:15 PM\" />");
        }

        [Test]
        public void ResourceLabel()
        {
            var hh = new FakeHtmlHelper<Foo>();

            //Pegou do nome da propriedade
            var html = hh.WResourceLabelFor(m => m.Name);
            Assert.AreEqual("<label for=\"Name\">Name</label>", html.ToHtmlString());

            //Pegou do DisplayAttribute
            html = hh.WResourceLabelFor(m => m.Age);
            Assert.AreEqual("<label for=\"Age\">Idade</label>", html.ToHtmlString());

            WResourceLabelExtensions.SetResourceManager(TestResource.ResourceManager);
            //Pegou do Resource
            html = hh.WResourceLabelFor(m => m.Name);
            Assert.AreEqual("<label for=\"Name\">Peguei do resource</label>", html.ToHtmlString());
        }

        [Test]
        public void WSpinButton()
        {
            var hh = new FakeHtmlHelper<Foo>();
            hh.ViewData.Model = new Foo
            {
                Price = 10
            };

            var html = hh.WSpinButton("Nome");
            Assert.AreEqual("<input class=\"wspinbutton\" id=\"Nome\" name=\"Nome\" type=\"text\" value=\"\" />", html.ToHtmlString());

            html = hh.WSpinButton("Nome", "Valor");
            Assert.AreEqual("<input class=\"wspinbutton\" id=\"Nome\" name=\"Nome\" type=\"text\" value=\"Valor\" />", html.ToHtmlString());

            html = hh.WSpinButtonFor(m => m.Price);
            Assert.AreEqual("<input class=\"wspinbutton\" id=\"Price\" name=\"Price\" type=\"text\" value=\"10\" />", html.ToHtmlString());
        }

        [Test]
        public void SwitchButtonExtensions()
        {
            var hh = new FakeHtmlHelper<Foo>();
            hh.ViewData.Model = new Foo
                                    {
                                        CreatedAt = new DateTime(2011, 8, 15, 12, 30, 15)
                                    };

            var html = hh.WSwitchButtonFor(m => m.Enabled);
            Assert.AreEqual("<input class=\"wswitchbutton\" id=\"Enabled\" name=\"Enabled\" no=\"Não!\" type=\"checkbox\" value=\"true\" yes=\"Sim!\" /><input name=\"Enabled\" type=\"hidden\" value=\"false\" />", html.ToHtmlString());

            html = hh.WSwitchButton("teste");
            Assert.AreEqual("<input class=\"wswitchbutton\" id=\"teste\" name=\"teste\" type=\"checkbox\" value=\"true\" /><input name=\"teste\" type=\"hidden\" value=\"false\" />", html.ToHtmlString());

        }

        [Test]
        public void ButtonExtensions()
        {
            var hh = new FakeHtmlHelper<Foo>();

            var html = hh.WButton("botao", "Label", true);
            Assert.AreEqual("<div class=\"wbutton-normal submit wbutton\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label", false);
            Assert.AreEqual("<div class=\"wbutton-normal wbutton\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label", "icone");
            Assert.AreEqual("<div class=\"wbutton-normal wbutton\" icon=\"icone\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label", "icone", true);
            Assert.AreEqual("<div class=\"wbutton-normal submit wbutton\" icon=\"icone\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label", WButtonType.Action);
            Assert.AreEqual("<div class=\"wbutton-action wbutton\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label", "icone", WButtonType.Alert);
            Assert.AreEqual("<div class=\"wbutton-alert wbutton\" icon=\"icone\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label");
            Assert.AreEqual("<div class=\"wbutton-normal wbutton\" id=\"botao\" name=\"botao\">Label</div>", html.ToHtmlString());

            html = hh.WButton("botao", "Label", new { @class = "abc", value = "123" });
            Assert.AreEqual("<div class=\"wbutton-normal wbutton abc\" id=\"botao\" name=\"botao\" value=\"123\">Label</div>", html.ToHtmlString());
        }

        [Test]
        public void LinkButtonExtensions()
        {
            var hh = new FakeHtmlHelper<Foo>();

            var html = hh.WLinkButton("botao", "Label", true);
            Assert.AreEqual("<a class=\"wbutton-normal submit wbutton\" href=\"botao\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label", false);
            Assert.AreEqual("<a class=\"wbutton-normal wbutton\" href=\"botao\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label", "icone");
            Assert.AreEqual("<a class=\"wbutton-normal wbutton\" href=\"botao\" icon=\"icone\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label", "icone", true);
            Assert.AreEqual("<a class=\"wbutton-normal submit wbutton\" href=\"botao\" icon=\"icone\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label", WButtonType.Action);
            Assert.AreEqual("<a class=\"wbutton-action wbutton\" href=\"botao\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label", "icone", WButtonType.Alert);
            Assert.AreEqual("<a class=\"wbutton-alert wbutton\" href=\"botao\" icon=\"icone\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label");
            Assert.AreEqual("<a class=\"wbutton-normal wbutton\" href=\"botao\">Label</a>", html.ToHtmlString());

            html = hh.WLinkButton("botao", "Label", new { @class = "abc", value = "123" });
            Assert.AreEqual("<a class=\"wbutton-normal wbutton abc\" href=\"botao\" value=\"123\">Label</a>", html.ToHtmlString());
        }
    }
}
