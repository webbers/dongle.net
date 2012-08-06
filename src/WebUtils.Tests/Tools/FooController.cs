using System.Web.Mvc;
using WebUtils.Mvc.Authentication;

namespace WebUtilsTest.Tools
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