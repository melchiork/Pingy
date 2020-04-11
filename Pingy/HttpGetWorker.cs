using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pingy
{
    public class HttpGetWorker :BackgroundService
    {
        private readonly ILogger<HttpGetWorker> _logger;
        private readonly Config _config;

        public HttpGetWorker(
            ILogger<HttpGetWorker> logger, 
            Config config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                HttpResponseMessage reply;
                using (var client = new HttpClient())
                {
                    client.Timeout = _config.HttpGetTimeout;
                    reply = await client.GetAsync(new Uri("http://" + _config.Address), stoppingToken);
                    _logger.LogInformation($"Sending HTTP GET request to: '{_config.Address}'.");
                }

                if (reply.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Success!");
                }
                else
                {
                    _logger.LogWarning($"Failed, {reply.StatusCode}.");
                }

                await Task.Delay(_config.Interval, stoppingToken);
            }
        }
    }
}
