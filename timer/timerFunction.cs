using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    /// <summary>
    /// Timer-triggered Azure Function that demonstrates scheduled execution.
    /// </summary>
    public class timerFunction
    {
        private readonly ILogger _logger;

        public timerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<timerFunction>();
        }

        /// <summary>
        /// Timer-triggered function that executes on a schedule defined by TIMER_SCHEDULE app setting.
        /// </summary>
        /// <param name="myTimer">Timer information including schedule status</param>
        /// <param name="context">Function execution context</param>
        /// <remarks>
        /// The RunOnStartup=true parameter is useful for development and testing as it triggers
        /// the function immediately when the host starts, but should typically be set to false
        /// in production to avoid unexpected executions during deployments or restarts.
        /// </remarks>
        [Function("timerFunction")]
        public void Run(
            [TimerTrigger("%TIMER_SCHEDULE%", RunOnStartup = true)] TimerInfo myTimer,
            FunctionContext context
        )
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.IsPastDue)
            {
                _logger.LogWarning("The timer is running late!");
            }
        }
    }
}
