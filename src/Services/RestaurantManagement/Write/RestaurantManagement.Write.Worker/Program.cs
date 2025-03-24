using RestaurantManagement.Write.Worker;
using RestaurantManagement.Write.Worker.EventLogProcessor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ReplicationEventLogProcessor>();
builder.Services.AddHostedService<TimedEventLogProcessor>();

builder.Services.AddScoped<IEventPublisherService, EventPublisherService>();

builder.AddInfrastructureServices();
builder.AddEventBus();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

var host = builder.Build();
host.Run();
