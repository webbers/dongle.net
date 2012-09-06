using System.Web.Mvc;
using Dongle.Web.Authentication;

namespace Dongle.Web.Tests.Tools
{
	public class FooController : Controller
	{
		[SessionAuthorize(Roles = "Details")] //More permisive action
		public ActionResult Details()
		{
			return View();
		}
	}
}