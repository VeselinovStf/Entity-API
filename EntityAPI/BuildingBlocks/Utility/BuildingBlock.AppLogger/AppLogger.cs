using BuildingBlock.Utility.Abstraction;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.AppLogger
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> logger;

        public AppLogger(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<T>();
        }

        public void LogDebug(string message)
        {
            logger.LogDebug(message);
        }

        public void LogError(string message)
        {
            logger.LogError(message);
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message);
        }
    }
}