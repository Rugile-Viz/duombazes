namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;


/// <summary>
/// <para>This controller is used when no controller is specified in the request URL.</para>
/// <para>The request URL is interpretted with the following template http://server-address/controller/action/[...suffix...]</para>
/// </summary>
public class HomeController : ControllerBase
{
	/// <summary>
	/// Handles 'Index' action. Is also invoked when no action is specified.
	/// </summary>
	/// <returns>View to render.</returns>
	[HttpGet]
	public ActionResult Index()
	{
		return View();
	}
}
