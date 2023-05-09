SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<DevPace.DbConnectionSQLite.DatabaseConnector>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();