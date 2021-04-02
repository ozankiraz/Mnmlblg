using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mnmlblg.Configuration;
using Mnmlblg.Extensions;
using Mnmlblg.Models;
using System.Linq;

namespace Mnmlblg
{
    public class Rss2FeedGenerator : IRss2FeedGenerator
    {
        private readonly IOptions<BlogConfiguration> _blogConfiguration;
        private readonly IPostRepository _postRepository;

        public Rss2FeedGenerator(IOptions<BlogConfiguration> blogConfiguration, IPostRepository postRepository)
        {
            _blogConfiguration = blogConfiguration;
            _postRepository = postRepository;
        }
        public XDocument Generate(string feedPath, IUrlHelper urlHelper)
        {
            var feed = new Feed<RssItem>
            {
                Title = _blogConfiguration.Value.BlogTitle,
                Link = new Uri(urlHelper.AbsoluteContent(feedPath)),
                Description = _blogConfiguration.Value.BlogSlogan,
                Items = _postRepository.AllPosts.OrderByDescending(x => x.PostDateTime)
                    .Take(_blogConfiguration.Value.RssFeedNumberOfItemsToShow)
                    .Select(x => new RssItem
                    {
                        Title = x.Title,
                        Body = x.Body,
                        Link = new Uri(urlHelper.AbsoluteContent(x.Title.GenerateSlug())),
                        Author =
                            new Author
                            {
                                Name = _blogConfiguration.Value.RssAuthorName,
                                Email = _blogConfiguration.Value.RssAuthorEmailAddress
                            },
                        Categories = new List<string> {x.Category},
                        Permalink = new Uri(urlHelper.AbsoluteContent(x.Title.GenerateSlug())).AbsoluteUri,
                        PublishDate = x.PostDateTime
                    }).ToList()
            };
            
            var feedXmlDocument = new XDocument(new XElement("rss"));
            feedXmlDocument.Root.Add(new XAttribute("version", "2.0"));

            var channel = new XElement("channel");
            channel.Add(new XElement("title", feed.Title));
            channel.Add(new XElement("link", feed.Link.AbsoluteUri));
            channel.Add(new XElement("description", feed.Description));

            feedXmlDocument.Root.Add(channel);

            foreach (var item in feed.Items)
            {
                var itemElement = new XElement("item");
                itemElement.Add(new XElement("title", item.Title));
                itemElement.Add(new XElement("link", item.Link.AbsoluteUri));
                itemElement.Add(new XElement("description", item.Body.GetPreview(_blogConfiguration.Value.RssPreviewMaxCharacters)));
                if (item.Author != null) itemElement.Add(new XElement("author", $"{item.Author.Email} ({item.Author.Name})"));
                foreach (var category in item.Categories) itemElement.Add(new XElement("category", category));
                if (item.Comments != null) itemElement.Add(new XElement("comments", item.Comments.AbsoluteUri));
                if (!string.IsNullOrWhiteSpace(item.Permalink)) itemElement.Add(new XElement("guid", item.Permalink));
                var dateFmt = string.Concat(item.PublishDate.ToString("ddd',' d MMM yyyy HH':'mm':'ss"), " ", item.PublishDate.ToString("zzzz").Replace(":", ""));
                if (item.PublishDate != DateTime.MinValue) itemElement.Add(new XElement("pubDate", dateFmt));
                channel.Add(itemElement);
            }

            return feedXmlDocument;
        }
    }

    public interface IRss2FeedGenerator
    {
        XDocument Generate(string feedPath, IUrlHelper urlHelper);
    }
}