using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot();

string[] allowedHosts = new string[] { "http://localhost:5173", "http://localhost", "http://127.0.0.1:5173" };

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy",
		builder => builder.WithOrigins(allowedHosts)
						.AllowCredentials()
						.AllowAnyMethod()
						.AllowAnyHeader()
	);
});

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var app = builder.Build();

app.UseWebSockets();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseOcelot().Wait();

app.Run();
