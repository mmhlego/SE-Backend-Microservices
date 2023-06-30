using JwtAuthenticationManager;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Users.API.Data;
using Users.API.Services;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();

builder.Services.AddDbContext<UsersContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IVerificationService, VerificationService>();

builder.Services.AddMassTransit(bus =>
{
	bus.SetKebabCaseEndpointNameFormatter();


	bus.UsingRabbitMq((ctx, cfg) =>
	{
		cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
	});
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

Console.WriteLine(builder.Configuration.GetConnectionString("SqlServer"));

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
	context.Database.EnsureCreated();
	context.EnsureCreatingMissingTables();
}

app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
