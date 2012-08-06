using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcAuthorize.Tests.Mocks;
using WebUtils.Mvc.Authentication;
using WebUtilsTest.Tools;

namespace WebUtilsTest.Mvc.Authentication
{
    [TestClass]
    public class AuthenticationTest
    {
        [TestMethod]
        public void TestAuthorized()
        {
            var controller = new FooController();
            controller.SetupFakeControllerContext(new AuthenticatedUser
                                                      {
                                                          Name = "silvio@santos.com",
                                                          Permissions = new List<string>
                                                                            {
                                                                                "Details"
                                                                            }
                                                      });

            Assert.IsTrue(new ActionInvokerExpecter<ViewResult>().InvokeAction(controller.ControllerContext, "Details"));
        }

        [TestMethod]
        public void TestUnauthorized()
        {
            var controller = new FooController();
            controller.SetupFakeControllerContext(new AuthenticatedUser
                                                      {
                                                          Name = "silvio@santos.com",
                                                          Permissions = new List<string>
                                                                            {
                                                                                "Sem permissão"
                                                                            }
                                                      });

            try
            {
                Assert.IsTrue(new ActionInvokerExpecter<ViewResult>().InvokeAction(controller.ControllerContext, "Details"));
                Assert.Fail("Ele não deveria ter permissão para passar por aqui! Se tem permissão está errado!");
            }
            catch (HttpException exception)
            {
                Assert.AreEqual(403,exception.GetHttpCode());
                Assert.AreEqual("Unauthorized Request", exception.Message);
            }
        }

        [TestMethod]
        public void TestUnauthenticated()
        {
            var controller = new FooController();
            controller.SetupFakeControllerContext();
            Assert.IsTrue(new ActionInvokerExpecter<HttpUnauthorizedResult>().InvokeAction(
                controller.ControllerContext, "Details"));
        }

        [TestMethod]
        public void TestSetRules()
        {
            var sessionAuthorizeAttribute = new SessionAuthorizeAttribute {Roles = "abc"};
            Assert.AreEqual("abc", sessionAuthorizeAttribute.Roles);

            sessionAuthorizeAttribute.Roles = "";
            Assert.AreEqual("", sessionAuthorizeAttribute.Roles);
        }

        /*[TestMethod]
        public void TestLdapAuthenticator()
        {
            var authenticator = new LdapAuthenticator("Midgard.local", "LDAP://192.168.200.251/dc=midgard,dc=local");
            var user = authenticator.Authenticate("usuario", "senha");

            Assert.AreEqual("Afonso França de Oliveira", user.Name);
        }*/
    }
}
