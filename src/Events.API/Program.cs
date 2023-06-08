using Events.API.Data;
using Events.API.Services;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();
builder.Services.AddDbContext<EventsContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<EventsContext>();
    context.Database.EnsureCreated();
    context.EnsureCreatingMissingTables();
}

app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
