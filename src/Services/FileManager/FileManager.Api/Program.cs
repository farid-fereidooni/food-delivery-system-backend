using EventBus.Registration;
using FileManager.Api.Pipelines;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddCustomSwagger();

builder.AddInfrastructureServices();

builder.AddCustomOpenIdServer();
builder.AddEventBus();

builder.AddApplicationServices();
builder.AddCustomControllers();

var app = builder.Build();

await app.Seed();

app.UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(options =>
    {
        options.MapControllers();
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.StartRabbitMq();
app.Run();
