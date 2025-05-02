using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class timerFunction
    {
        private readonly ILogger _logger;

        public timerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<timerFunction>();
        }

        [Function("timerFunction")]
        public void Run([TimerTrigger("%TIMER_SCHEDULE%")] TimerInfo myTimer, FunctionContext context)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.IsPastDue)
            {
                _logger.LogWarning("The timer is running late!");
            }
        }
    }
}