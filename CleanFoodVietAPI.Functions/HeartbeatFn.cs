using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Functions
{
    public class HeartbeatFn
    {
        private readonly ILogger<HeartbeatFn> _log;
        public HeartbeatFn(ILogger<HeartbeatFn> log) => _log = log;

        [Function("HeartbeatFn")]
        public Task RunAsync([TimerTrigger("0 */2 * * * *")] TimerInfo timer)
        {
            _log.LogInformation("[HeartbeatFn] at {Time}", DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}
