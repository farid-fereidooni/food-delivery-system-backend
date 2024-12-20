using Microsoft.Extensions.Logging;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Exceptions;
using RestaurantManagement.Core.Helpers;

namespace RestaurantManagement.Core.Application.Services;

public class ConcurrencyHandler : IConcurrencyHandler
{
    private readonly ILogger<ConcurrencyHandler> _logger;

    public ConcurrencyHandler(ILogger<ConcurrencyHandler> logger)
    {
        _logger = logger;
    }

    public void Retry(Action action, CancellationToken cancellationToken = default)
    {
        Retry(() =>
        {
            action();
            return 0;
        },
        cancellationToken);
    }

    public TResult Retry<TResult>(Func<TResult> func, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                return func();
            }
            catch (UpdateConcurrencyException)
            {
                _logger.LogRetry(++retryCount);
            }
        }

        throw new OperationCanceledException();
    }

    public async ValueTask RetryAsync(Func<Task> func, CancellationToken cancellationToken = default)
    {
        await RetryAsync(async () =>
        {
            await func();
            return 0;
        },
        cancellationToken);
    }

    public async ValueTask<TResult> RetryAsync<TResult>(
        Func<Task<TResult>> func, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                return await func();
            }
            catch (UpdateConcurrencyException)
            {
                _logger.LogRetry(++retryCount);
            }
        }

        throw new OperationCanceledException();
    }
}
