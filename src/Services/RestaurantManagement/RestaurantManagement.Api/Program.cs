using RestaurantManagement.Api.Pipelines;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSettings();
builder.AddInfrastructureServices();
builder.AddCustomOpenIdServer();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();

app.UseAuthentication();
app.UseAuthorization();
