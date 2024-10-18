using Identity.Api.Pipelines;
using Identity.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSettings();
builder.AddCustomEntityFrameworkCore();
builder.ConfigureCors();
builder.AddCustomOpenIdServer();
builder.AddAspIdentity();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<OpenIdDictAuthorizationService>();
builder.Services.AddHostedService<SeedService>();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting()
    .UseCors()
    .UseAuthorization()
    .UseAuthorization()
    .UseEndpoints(options =>
    {
        options.MapControllers();
        options.MapRazorPages();
    });

app.MapGet("/", () => Results.Content("Up")).RequireCors();

app.UseStaticFiles();

app.Run();
