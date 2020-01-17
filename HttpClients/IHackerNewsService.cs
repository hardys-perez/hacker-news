using HackerNews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNews.HttpClients
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<int>> GetTopStoriesAsync();
        Task<Item> GetItemAsync(int itemNumber);
    }
}
