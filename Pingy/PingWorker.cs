using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pingy
{
    public class PingWorker : BackgroundService
    {
        private readonly ILogger<PingWorker> _logger;
        private readonly Config _config;

        public PingWorker(
            ILogger<PingWorker> logger,
            Config config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                var ping = new Ping();
                var pingReply = await ping.SendPingAsync(_config.Address, (int) _config.PingTimeout.TotalMilliseconds);
                _logger.LogInformation($"Sending ping request to: '{_config.Address}'.");

                if (pingReply.Status == IPStatus.Success)
                {
                    _logger.LogInformation("Success!");
                }
                else
                {
                    _logger.LogWarning($"Failed, {pingReply.Status}.");
                }

                await Task.Delay(_config.PingTimeout, stoppingToken);
            }
        }
    }
}