using FizzBuzz.Data;              // <-- For FizzBuzzDbContext
using Microsoft.EntityFrameworkCore; // <-- For UseSqlServer

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
        // This tells System.Text.Json to ignore (skip) any cycles
        // rather than throwing an exception.
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4) (Optional) Auto-run migrations when app starts
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FizzBuzzDbContext>();
    db.Database.EnsureCreated(); // or db.Database.Migrate();
}

// 5) The rest of the pipeline
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
