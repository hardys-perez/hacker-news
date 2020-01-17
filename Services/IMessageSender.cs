using HackerNews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNews.Services
{
    public interface IMessageSender
    {
        Task PublishNewsAsync(IEnumerable<Item> items);
    }
}
