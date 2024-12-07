using MyWebServerProject.Extensions;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Custom Extensions
builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureJWTAuthentication(builder.Configuration);
builder.Services.RegisterIOCForManagers();
builder.Services.RegisterIOCForActionFilters();
builder.Services.RegisterIOCForMQTTServices();
builder.Services.ConfigureIdentity();

builder.Services.AddAutoMapper(typeof(Program));

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

//Authentication Service içerisindeki event aboneliklerinin gerçekleþmesini saðlar.
app.Services.InitializeAuthenticationService();

//Start MQTT Server
await app.Services.RunMQTTServer();

app.Run();
//app.Run("http://0.0.0.0:5000");
