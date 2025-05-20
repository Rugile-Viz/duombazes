namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

using Newtonsoft.Json;


/// <summary>
/// Base class for ASP.NET controllers that is used to setup per-request SQL helper and send out SQL queries
/// used in request in HTTP response coookie.
/// </summary>
public class ControllerBase : Controller
{
	public override void OnActionExecuted(ActionExecutedContext context)
	{
		//render exception page if controller action has resulted in exception, this will ensure that any recorder queries go into browser console
		if( context.Exception != null ) {
			context.Result = View("_Exception");
			context.ExceptionHandled = true;
		}

		//get ID of current HTTP request
		var requestId = (String)context.HttpContext.Items["HttpRequestID"];

		//expose SQL queries used in servicing current HTTP request
		if( Sql.Queries.ContainsKey(requestId) )
		{
			//get SQL queries as BASE64 encoded JSON array
			var queries = Sql.Queries[requestId].ToArray();
			var jsonQueries = JsonConvert.SerializeObject(queries);
			var base64JsonQueries = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonQueries));

			//pass the queries into ViewData of view returned by the current action
			if( context.Result is ViewResult )
				((ViewResult)context.Result).ViewData["sql-queries"] = base64JsonQueries;

			//clear storage
			Sql.Queries.Remove(requestId, out _);
		}

		//
		base.OnActionExecuted(context);
	}

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		//initialize SQL helper for the current HTTP request
		Sql.LocalInstance.Value = new Sql((String)context.HttpContext.Items["HttpRequestID"]);

		//
		base.OnActionExecuting(context);
	}

	public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		//initialize SQL helper for the current HTTP request
		Sql.LocalInstance.Value = new Sql((String)context.HttpContext.Items["HttpRequestID"]);

		//
		return base.OnActionExecutionAsync(context, next);
	}
}