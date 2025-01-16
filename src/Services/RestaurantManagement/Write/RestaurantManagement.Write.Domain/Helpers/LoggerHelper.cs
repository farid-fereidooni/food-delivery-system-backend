using Microsoft.Extensions.Logging;

namespace RestaurantManagement.Write.Domain.Helpers;

public static class LoggerHelper
{
    public static void LogRetry(this ILogger logger, int retryCount)
    {
        logger.LogInformation("Concurrency on AddMenuItemStockCommand, Retrying. ({})", retryCount);
    }
}
