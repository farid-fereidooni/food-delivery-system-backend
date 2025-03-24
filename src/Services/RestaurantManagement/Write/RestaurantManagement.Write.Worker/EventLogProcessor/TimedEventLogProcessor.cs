namespace RestaurantManagement.Write.Worker.EventLogProcessor;

public class TimedEventLogProcessor : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<TimedEventLogProcessor> _logger;

    public TimedEventLogProcessor(
        IServiceProvider services,
        ILogger<TimedEventLogProcessor> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is starting.");

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(2));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await PublishEvents(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }

    private async Task PublishEvents(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisherService>();
        await eventPublisher.PublishEvents(cancellationToken);
    }
}
