using BingoBlitz_CommunityHub.Data;
using BingoBlitz_CommunityHub.Data.Interfaces;
using DotNetEnv.Configuration;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
builder.Configuration.AddDotNetEnv();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CosmosClient>(provider =>
{
    string accountEndpoint = builder.Configuration.GetValue<string>("CosmosAccountEndpoint");
    string accountKey = builder.Configuration.GetValue<string>("CosmosAccountKey");
    string cosmosConnectionString = $"AccountEndpoint={accountEndpoint};AccountKey={accountKey}"; 

    CosmosClientOptions cosmosClientOptions = new()
    {

    };

    return new CosmosClient(cosmosConnectionString, cosmosClientOptions);
});

builder.Services.AddScoped<IObjectiveData, CosmosObjectiveData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(app => app.AllowAnyOrigin());

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
