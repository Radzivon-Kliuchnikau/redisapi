using Microsoft.AspNetCore.Http.HttpResults;
using RedisAPI.Data;
using RedisAPI.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!));

builder.Services.AddScoped<IPlatformRepo, RedisPlatformRepo>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/platform", (string id, IPlatformRepo repo) =>
{
    var platform = repo.GetPlatformById(id);

    if (platform != null)
    {
        return Results.Ok(platform);
    }

    return Results.NotFound();
})
.WithName("GetPlatformById")
.WithOpenApi();

app.MapPost("/platform", (Platform platform, IPlatformRepo repo) =>
{
    repo.CreatePlatform(platform);

    return Results.Created($"/platform/{platform.Id}", platform);
})
.WithName("SetPlatform")
.WithOpenApi();

app.MapGet("/platforms", (IPlatformRepo repo) =>
{
    var platforms = repo.GetAllPlatforms();

    return Results.Ok(platforms);
})
.WithName("GetAllPlatforms")
.WithOpenApi();


app.Run();

