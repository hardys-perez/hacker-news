using HackerNews.HttpClients;
using HackerNews.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Services
{
    public class ConsumerService: IConsumerService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IHackerNewsService _hackerNewsService;
        private readonly IDictionary<int, Item> _items;
        private readonly IMessageSender _messageSender;
        private IList<int> _topStories;

        public ConsumerService(ILogger<ConsumerService> logger, IHackerNewsService hackerNewsService, IMessageSender messageSender)
        {
            _logger = logger;
            _hackerNewsService = hackerNewsService;
            _items = new Dictionary<int, Item>();
            _topStories = new List<int>();
            _messageSender = messageSender;
        }

        public async Task SetStoriesAsync(IEnumerable<int> newStories)
        {
            if (newStories.SequenceEqual(_topStories))
            {
                return;
            }

            var nonStoredItems = await GetNonStoredItemsAsync(newStories);

            var added = AddItemsToDictionary(nonStoredItems);

            var values = newStories.Select(id => {
                _items.TryGetValue(id, out Item outItem);
                return outItem;
            });

            await _messageSender.PublishNewsAsync(values.Where(item => item != null))
                .ConfigureAwait(false);

            RemoveCacheItemsNotInTheTopStories(newStories);
            _topStories = newStories.ToList();
        }

        private void RemoveCacheItemsNotInTheTopStories(IEnumerable<int> newStories)
        {
            var toRemove = _topStories.Where(topItem => !newStories.Contains(topItem));
            foreach (var item in toRemove)
            {
                _items.Remove(item);
            }
        }

        private int AddItemsToDictionary(Item[] nonStoredItems)
        {
            int count = 0;
            foreach (var item in nonStoredItems)
            {
                if (item != null)
                {
                    var added = _items.TryAdd(item.Id, item);
                    count += added ? 1 : 0;

                    if (!added)
                    {
                        _logger.LogWarning($"ConsumerService: Item with id = {item.Id} couldn't be added");
                    }
                    else
                    {
                        _logger.LogInformation($"ConsumerService: the item = {item.Id} has beed added");
                    }
                }
            }

            return count;
        }

        private async Task<Item[]> GetNonStoredItemsAsync(IEnumerable<int> newStories)
        {
            var itemsToBeRetrieved = new List<Task<Item>>();
            foreach (var id in newStories)
            {
                if (!_items.ContainsKey(id))
                {
                    itemsToBeRetrieved.Add(_hackerNewsService.GetItemAsync(id));
                }
            }

            return await Task.WhenAll(itemsToBeRetrieved);
        }
    }
}
