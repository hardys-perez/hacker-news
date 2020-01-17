using System.Collections.Generic;

namespace HackerNews.Models
{
    public class Item
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public int Id { get; set; }
        public IList<int> Kids { get; set; }
        public int Score { get; set; }
        public string Text { get; set; }
        public int Time { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}