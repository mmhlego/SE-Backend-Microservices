
using General.API.Data;
using General.API.Services;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();
builder.Services.AddDbContext<GeneralContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPosterService, PosterService>();


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
	var context = scope.ServiceProvider.GetRequiredService<GeneralContext>();
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



