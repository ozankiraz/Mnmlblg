using System;

namespace Mnmlblg.Models
{
    public class Post
    {
        public string Title { get; set; }

        public DateTime PostDateTime { get; set; }

        public string Body { get; set; }

        public string Category { get; set; }

        public bool IsNavigationPost { get; set; }
    }
}