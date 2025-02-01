using EventBus.Registration;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Write.Api.Pipelines;
using RestaurantManagement.Write.Api.Services;
using RestaurantManagement.Write.Domain.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddCustomSwagger();

builder.AddInfrastructureServices();
builder.AddCoreServices();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.AddCustomOpenIdServer();
builder.AddEventBus();
builder.AddDomainServices();
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

await app.StartRabbitMq();
app.Run();
