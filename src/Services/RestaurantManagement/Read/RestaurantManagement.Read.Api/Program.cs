using EventBus.Registration;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Read.Api.Pipelines;
using RestaurantManagement.Read.Api.Services;
using RestaurantManagement.Read.Domain.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddCustomSwagger();

builder.AddInfrastructureServices();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.AddCustomOpenIdServer();
builder.AddEventBus();
builder.AddApplicationServices();
builder.AddCustomControllers();

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

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

app.UseHttpsRedirection();

await app.StartRabbitMq();
app.Run();
