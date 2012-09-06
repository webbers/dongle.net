using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Web.Mvc;
using Dongle.Web.Authentication;
using Dongle.Web.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Web.Tests.Mvc.Authentication
{
    [TestClass]
    public class AuthenticationTest
    {
        private const string Domain = "YOURDOMAIN.LOCAL";
        private const string Container = "dc=YOURDOMAIN,dc=local";

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
                Assert.IsTrue(new ActionInvokerExpecter<ViewResult>().InvokeAction(controller.ControllerContext,
                                                                                   "Details"));
                Assert.Fail("Ele não deveria ter permissão para passar por aqui! Se tem permissão está errado!");
            }
            catch (HttpException exception)
            {
                Assert.AreEqual(403, exception.GetHttpCode());
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
        public void TestLdapAuthenticatorGetUser()
        {   //fixture setup
            var sut = new LdapAuthenticator(Domain, Container);

            //exercize
            var result = sut.GetUser("rodrigo.rodrigues");

            //verify outcome
            Assert.IsNotNull(result);
            Assert.AreEqual("Rodrigo Aiala Feijolo Rodrigues", result.Name);

            //teardown
            result.Dispose();
        }*/

        [TestMethod]
        public void GetPrincipalContextTet()
        {
            //fixture setup
            var sut = new LdapAuthenticator("", "");

            //exercize
            //var result = sut.GetPrincipalContext();

            //verify outcome
            //Assert.IsNotNull(result);
        }

    /*[TestMethod]
        public void TestLdapAuthenticatorValidateCredentials()
        {
            //fixture setup
            var sut = new LdapAuthenticator(Domain, Container);

            //exercize
            var result = sut.ValidateCredentials("unknown.user", "");

            //verify outcome
            Assert.IsFalse(result);
        }*/
    }
}
