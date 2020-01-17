using HackerNews.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNews.HttpClients
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HackerNewsService> _logger;

        private const string TopStories = "topstories.json";
        private readonly Func<int, string> Item = (int number) => $"item/{number}.json";

        public HackerNewsService(ILogger<HackerNewsService> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<int>> GetTopStoriesAsync()
        {
            try
            {
                var responseString = await _httpClient.GetStringAsync(TopStories);
                return JsonConvert.DeserializeObject<IEnumerable<int>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An Error Ocurred while retrieving the top stories");
                return new int[] { };
            }
        }

        public async Task<Item> GetItemAsync(int itemNumber)
        {
            try
            {
                var responseString = await _httpClient.GetStringAsync(Item(itemNumber));
                return JsonConvert.DeserializeObject<Item>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An Error Ocurred while retrieving the Story");
                return null;
            }
        }
    }
}
