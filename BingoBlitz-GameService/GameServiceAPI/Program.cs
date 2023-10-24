using BingoBlitz_GameService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(app => app.AllowAnyOrigin());

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();


IConfigurationSection rabbitMQ = app.Configuration.GetRequiredSection("RabbitMQ");

Connector connector = new(
    userName: rabbitMQ.GetValue<string>("UserName"),
    password: rabbitMQ.GetValue<string>("Password"),
    virtualHost: rabbitMQ.GetValue<string>("VirtualHost"),
    hostName: rabbitMQ.GetValue<string>("HostName"),
    port: rabbitMQ.GetValue<int>("Port")
);

GameDataReceiver gameDataReceiver = new(connector);

app.Run();
