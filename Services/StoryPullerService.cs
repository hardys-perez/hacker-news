using HackerNews.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HackerNews.Services
{
    public class StoryPullerService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<StoryPullerService> _logger;
        private readonly IHackerNewsService _service;
        private readonly IConsumerService _consumerService;
        private readonly int _retrievalIntervalInSeconds;
        private const string ServiceName = "Story Puller Service";

        public StoryPullerService(ILogger<StoryPullerService> logger, 
            IConfiguration configuration, 
            IHackerNewsService service,
            IConsumerService consumerService)
        {
            _logger = logger;
            _service = service;
            _consumerService = consumerService;
            _retrievalIntervalInSeconds = configuration.GetValue<int>("RetrievalIntervalInSeconds");
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is running.");

            var delayTime = TimeSpan.FromSeconds(_retrievalIntervalInSeconds);
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWorkAsync();
                await Task.Delay(delayTime, stoppingToken);
            }
        }

        private async Task DoWorkAsync()
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                $"{ServiceName} is working. Count: {count}", count);

            var topStories = await _service.GetTopStoriesAsync();
            await _consumerService.SetStoriesAsync(topStories);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is stopping.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation($"{ServiceName} is stopped.");
        }
    }
}
