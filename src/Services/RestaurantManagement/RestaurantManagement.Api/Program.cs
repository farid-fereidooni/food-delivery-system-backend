using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Pipelines;
using RestaurantManagement.Api.Services;
using RestaurantManagement.Core.Application.Command.Admin;
using RestaurantManagement.Core.Domain.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddCustomSwagger();

builder.ConfigureSettings();
builder.AddInfrastructureServices();
builder.AddCoreServices();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.AddCustomOpenIdServer();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateFoodTypeAdminCommand).Assembly));

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

app.Run();
