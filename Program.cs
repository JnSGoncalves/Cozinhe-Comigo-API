using System;
using Cozinhe_Comigo_API.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

string connectionString =
    $"Host={Environment.GetEnvironmentVariable("DB_DEV_HOST")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_DEV_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_DEV_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_DEV_PASS")};" +
    $"SSL Mode={Environment.GetEnvironmentVariable("DB_SSL")};" +
    $"Trust Server Certificate={Environment.GetEnvironmentVariable("DB_TRUST")};" +
    $"Channel Binding={Environment.GetEnvironmentVariable("DB_CHANNEL")};";

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)); // Inicia a conexão no Banco de dados

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
