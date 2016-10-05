using System;
using System.Collections.Generic;

namespace Mnmlblg.Models
{
    public class Feed<T>
    {
        public string Description { get; set; }
        public Uri Link { get; set; }
        public string Title { get; set; }
        public string Copyright { get; set; }

        public List<T> Items { get; set; } = new List<T>();
    }

    public class RssItem
    {
        public Author Author { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public Uri Comments { get; set; }
        public Uri Link { get; set; }
        public string Permalink { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }


}