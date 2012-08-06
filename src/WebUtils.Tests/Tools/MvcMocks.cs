using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using WebUtils.Mvc.Authentication;

namespace WebUtilsTest.Tools
{
	public static class MvcMocks
	{
		public static HttpContextBase FakeHttpContext(AuthenticatedUser user)
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var server = new Mock<HttpServerUtilityBase>();
			new Mock<IIdentity>();
		    var session = new Mock<HttpSessionStateBase>();

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			context.Setup(ctx => ctx.Response).Returns(response.Object);
			context.Setup(ctx => ctx.Session).Returns(session.Object);
			context.Setup(ctx => ctx.Server).Returns(server.Object);
		    session.Setup(s => s["User"]).Returns(user);
			context.Setup(ctx => ctx.Response.Cache).Returns(CreateCachePolicy());
			return context.Object;
		}

		public static HttpCachePolicyBase CreateCachePolicy()
		{
			var mock = new Mock<HttpCachePolicyBase>();
			return mock.Object;
		}

		public static void SetupFakeControllerContext(this Controller controller, AuthenticatedUser authUser = null)
		{
		    var httpContext = FakeHttpContext(authUser);

			var context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
			controller.ControllerContext = context;
		}
	}
}