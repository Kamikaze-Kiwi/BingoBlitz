using BingoBlitz_CommunityHub.Data;
using BingoBlitz_CommunityHub.Data.Interfaces;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
builder.Configuration.AddDotNetEnv();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Cosmos database connection service:
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
IdentityModelEventSource.ShowPII = true;
// Add authentication service:
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://bingoblitz.eu.auth0.com/";
    options.Audience = "CommunityHub";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(app => app.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
