using WebsiteBff.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
var configuration = builder.Configuration.GetSection("ReverseProxy");

builder.Services.AddClusterSwagger(configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration);

builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger();
app.UseClusterSwaggerUi();

app.MapReverseProxy();
app.MapGet("/", () => Results.Content("Up"));

app.Run();
