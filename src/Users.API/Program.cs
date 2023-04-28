using JwtAuthenticationManager;
using Users.API.Data;
using Users.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddJwtAuthentication();

// builder.Services.AddDbContext<Context>(options => {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
// });

builder.Services.AddScoped<IUsersService, UsersService>();

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