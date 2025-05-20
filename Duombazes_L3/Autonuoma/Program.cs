namespace Org.Ktu.Isk.P175B602.Autonuoma;

using System.Collections.Concurrent;
using NLog;


/// <summary>
/// <para>Program entry class.</para>
/// <para>Static members are thread safe, instance members are not.</para>
/// </summary>
public class Program {
	/// <summary>
	/// Logger for this class.
	/// </summary>
	Logger log = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Configure logging subsystem.
	/// </summary>
	private static void ConfigureLogging()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var console =
			new NLog.Targets.ConsoleTarget("console")
			{
				Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
			};
		config.AddTarget(console);
		config.AddRuleForAllLevels(console);

		LogManager.Configuration = config;
	}

	/// <summary>
	/// Program entry point.
	/// </summary>
	/// <param name="args">Command line arguments.</param>
	public static void Main(string[] args)
	{
		ConfigureLogging();

		var self = new Program();
		self.Run(args);
	}

	/// <summary>
	/// Program body.
	/// </summary>
	/// <param name="args">Command line arguments.</param>
	private void Run(string[] args)
	{
		try
		{
			var builder = WebApplication.CreateBuilder(args);

			//set the address and port the Kestrel server should bind to
			builder.WebHost.ConfigureKestrel(opts =>
			{
				opts.Listen(System.Net.IPAddress.Loopback, 5000);
			});

			//add services
			builder.Services
				.AddRazorPages()
				.AddRazorOptions(opts => {
					//this will allow having _Exception.cshtml as the root view
					opts.ViewLocationFormats.Add("/Views/{0}.cshtml");
				});

			//build the web app
			var app = builder.Build();

			//initialize configuration helper
			Config.CreateSingletonInstance(app.Configuration);

			//add middleware to set request ID and no-cache headers
			app.Use(async (context, next) =>
			{
				// Set request ID to correlate request logs
				context.Items["HttpRequestID"] = Guid.NewGuid().ToString();

				// Initialize Sql.LocalInstance for request-scoped SQL usage
				Sql.LocalInstance.Value = new Sql(context.TraceIdentifier);

				// Set no-cache headers
				#pragma warning disable CA1861
				context.Response.Headers.CacheControl = new[] {
					"no-store, no-cache, must-revalidate, max-age=0",
					"post-check=0, pre-check=0"
				};
				#pragma warning restore CA1861
				context.Response.Headers.Pragma = "no-cache";

				// Continue to next middleware
				await next();
			});


			//add request processing middleware
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();

			app.MapDefaultControllerRoute();
			app.MapRazorPages();

			//run the web app
			app.Run();
		}
		catch( Exception e )
		{
			log.Error(e, "Unhandled exception caught when initializing program. The main thread is now dead.");
		}
	}
}
