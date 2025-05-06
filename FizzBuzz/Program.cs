using FizzBuzz.Data;             
using Microsoft.EntityFrameworkCore; 
using FizzBuzz.Services;           

var builder = WebApplication.CreateBuilder(args);

// 1) Pull connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2) Register FizzBuzzDbContext with SQL Server
builder.Services.AddDbContext<FizzBuzzDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// 3) Register controllers & swagger
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
      
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ISessionService, SessionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FizzBuzzDbContext>();
    db.Database.EnsureCreated(); // or db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
