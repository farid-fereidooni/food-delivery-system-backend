namespace RestaurantManagement.Core.Domain.Contracts;

public interface IConcurrencyHandler
{
    void Retry(Action action, CancellationToken cancellationToken = default);
    TResult Retry<TResult>(Func<TResult> func, CancellationToken cancellationToken = default);
    ValueTask RetryAsync(Func<Task> func, CancellationToken cancellationToken = default);
    ValueTask<TResult> RetryAsync<TResult>(Func<Task<TResult>> func, CancellationToken cancellationToken = default);
}
