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

var receiver = new Receive();
receiver.StartReceiving();

app.Run();
