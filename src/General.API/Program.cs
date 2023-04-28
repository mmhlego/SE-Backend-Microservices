using General.API.Services;
using JwtAuthenticationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();

// builder.Services.AddDbContext<Context>(options => {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
// });

builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPosterService, PosterService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
