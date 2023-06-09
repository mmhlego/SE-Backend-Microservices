using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Products.API.Data;
using Products.API.Services;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();

builder.Services.AddDbContext<ProductsContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRequestService, RequestService>();


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
	var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();
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
