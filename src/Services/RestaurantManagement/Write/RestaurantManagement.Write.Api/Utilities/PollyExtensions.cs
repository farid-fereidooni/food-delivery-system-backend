using System.Data.Common;
using Polly;

namespace RestaurantManagement.Write.Api.Utilities;

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
}
