using Microsoft.AspNetCore.Mvc.DataAnnotations;
using RestaurantManagement.Api.Filters;
using RestaurantManagement.Api.Pipelines;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSettings();
builder.AddInfrastructureServices();
builder.AddCustomOpenIdServer();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateFoodTypeAdminCommand).Assembly));

builder.Services.AddSingleton<IValidationAttributeAdapterProvider, LocalizedValidationAttributeAdapterProvider>();

builder.Services.AddControllers(options =>
    options.Filters.Add<ModelStateFilter>());

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();

app.UseAuthentication();
app.UseAuthorization();
