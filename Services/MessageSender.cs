using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Models;
using HackerNews.Server;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace HackerNews.Services
{
    public class MessageSender: IMessageSender
    {
        private readonly IHubContext<TopStoriesHub> _hubContext;

        public MessageSender(IHubContext<TopStoriesHub> contextHub)
        {
            _hubContext = contextHub;
        }
        
        public async Task PublishNewsAsync(IEnumerable<Item> items)
        {
            var data = JsonConvert.SerializeObject(items);
            await _hubContext.Clients.All.SendAsync("update", data);
        }
    }
}
