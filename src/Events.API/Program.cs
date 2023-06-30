using Events.API.Consumers;
using Events.API.Data;
using Events.API.Services;
using JwtAuthenticationManager;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();
builder.Services.AddDbContext<EventsContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<IMessageService, MessageService>();


// Add http client
builder.Services.AddHttpClient();

// Add Mass Transit Configuration
builder.Services.AddMassTransit(bus =>
{
	bus.AddConsumer<SmsConsumer>();
	bus.AddConsumer<MessageConsumer>();

	bus.UsingRabbitMq((ctx, cfg) =>
	{
		cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
		cfg.ReceiveEndpoint("Sms-Queue", ep =>
		{
			ep.ConfigureConsumer<SmsConsumer>(ctx);
		});

		cfg.ReceiveEndpoint("Message-Queue", ep =>
		{
			ep.ConfigureConsumer<MessageConsumer>(ctx);
		});
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

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<EventsContext>();
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
