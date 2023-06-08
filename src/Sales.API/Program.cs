using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Sales.API.Data;
using Sales.API.Services;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();
builder.Services.AddDbContext<SalesContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<SalesContext>();
    context.Database.EnsureCreated();
    context.EnsureCreatingMissingTables();
}

app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
