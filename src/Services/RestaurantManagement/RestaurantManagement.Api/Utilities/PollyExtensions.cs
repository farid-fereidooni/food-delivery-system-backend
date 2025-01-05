using System.Data.Common;
using System.Net.Sockets;
using MongoDB.Driver;
using Polly;

namespace RestaurantManagement.Api.Utilities;

public static class PollyHelper
{
    public static AsyncPolicy HandleSqlNotReady(ILogger logger)
    {
        return Policy.Handle<DbException>()
            .WaitAndRetryForeverAsync(
                _ => TimeSpan.FromSeconds(5),
                onRetry: (exception, retry, time) =>
                {
                    logger.LogWarning(
                        exception,
                        "Exception \"{Message}\" occured on connecting to database. retry attempt {retry}",
                        exception.Message,
                        retry);
                });
    }

    public static AsyncPolicy HandleNoSqlNotReady(ILogger logger)
    {
        return Policy.Handle<MongoConnectionException>()
            .Or<TimeoutException>()
            .Or<SocketException>()
            .WaitAndRetryForeverAsync(
                _ => TimeSpan.FromSeconds(5),
                onRetry: (exception, retry, time) =>
                {
                    logger.LogWarning(
                        exception,
                        "Exception \"{Message}\" occured on connecting to database. retry attempt {retry}",
                        exception.Message,
                        retry);
                });
    }
}
