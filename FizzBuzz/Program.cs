using FizzBuzz.Data;              // <-- For FizzBuzzDbContext
using Microsoft.EntityFrameworkCore; // <-- For UseSqlServer

var builder = WebApplication.CreateBuilder(args);

// 1) Pull connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2) Register FizzBuzzDbContext with SQL Server
builder.Services.AddDbContext<FizzBuzzDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3) Register controllers & swagger
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
