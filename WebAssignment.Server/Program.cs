using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebAssignment.Server.Context;
using WebAssignment.Server.Enums;
using WebAssignment.Server.Repositories;
using WebAssignment.Server.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
LinqToDBForEFTools.Initialize();

NpgsqlDataSourceBuilder dataSourceBuilder = new(builder.Configuration.GetConnectionString("TransactionContext"));

_ = dataSourceBuilder.MapEnum<TransactionStatus>();

NpgsqlDataSource dataSource = dataSourceBuilder.Build();

_ = builder.Services.AddDbContextPool<TransactionContext>(options => _ = options.UseNpgsql(dataSource, options => options.EnableRetryOnFailure()));
_ = builder.Services.AddScoped<AssignmentRepositories>();
_ = builder.Services.AddScoped<TransactionService>();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

await app.RunAsync();
