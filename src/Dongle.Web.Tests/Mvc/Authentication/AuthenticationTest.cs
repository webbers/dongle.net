using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
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

        /*[TestMethod]
        public void TestLdapGroups()
        {
            var sut = new LdapAuthenticator(Domain, Container);
            var result = sut.GetUserGroups("rodrigo.rodrigues");
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void TestLdapInvalidUsername()
        {
            var sut = new LdapAuthenticator(Domain, Container);
            var result = sut.GetUserGroups("invalid.username");
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestLdapInvalidUsernameThrowException()
        {
            const string invalidUsername = "invalid.username";
            var sut = new LdapAuthenticator(Domain, Container);
            try
            {
                sut.GetUserGroups(invalidUsername, true);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, invalidUsername);
                Assert.IsInstanceOfType(ex, typeof(ActiveDirectoryObjectNotFoundException));
            }
        }

        [TestMethod]
        public void TestLdapInvalidDomainThrowException()
        {
            const string invalidUsername = "invalid.username";
            var sut = new LdapAuthenticator("INVALID.DOMAIN", "LDAP://invalidpath");
            try
            {
                sut.GetUserGroups(invalidUsername, true);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(PrincipalServerDownException));
            }
        }

        [TestMethod]
        public void TestLdapInvalidDomain()
        {
            const string invalidUsername = "invalid.username";
            var sut = new LdapAuthenticator("INVALID.DOMAIN", "LDAP://invalidpath");
            var result = sut.GetUserGroups(invalidUsername);
            Assert.AreEqual(0, result.Count);
        }*/

        [TestMethod]
        public void TestSetRules()
        {
            var sessionAuthorizeAttribute = new SessionAuthorizeAttribute { Roles = "abc" };
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
        public void GetPrincipalContextTest()
        {
            //fixture setup
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
