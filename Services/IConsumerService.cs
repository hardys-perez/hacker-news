using HackerNews.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNews.Services
{
    public interface IConsumerService
    {
        Task SetStoriesAsync(IEnumerable<int> stories);
    }
}
