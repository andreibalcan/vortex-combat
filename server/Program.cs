using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using server.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("VC_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The VC_CONNECTION_STRING environment variable is not set.");
}

Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(Environment.GetEnvironmentVariable("VC_CONNECTION_STRING"), new MySqlServerVersion(new Version(8, 4, 4))));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
