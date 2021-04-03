using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Mnmlblg.Configuration;
using Mnmlblg.Extensions;
using Mnmlblg.Models;

namespace Mnmlblg
{
    public class MarkdownPostsRepository : IPostRepository
    {
        public MarkdownPostsRepository(IOptions<BlogConfiguration> blogConfiguration, IWebHostEnvironment environment)
        {
            _blogConfiguration = blogConfiguration;
            _environment = environment;
            _posts = GetPosts();
        }

        readonly List<Post> _posts;

        private List<Post> GetPosts()
        {
            var posts = new List<Post>();

            var directories = Directory.GetDirectories(Path.Combine(_environment.WebRootPath, "posts"));

            foreach(var directory in directories)
            {
                var files = Directory.GetFiles(directory, "*.md");
                foreach(var markdownFile in files)
                {
                    var fileName = (new DirectoryInfo(markdownFile)).Name;
                    var fileNameParts = fileName.Split('_');
                    var title = fileNameParts[0];
                    var postDateTime =  DateTime.ParseExact(fileNameParts[1].Replace(".md", string.Empty), "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture);
                    var markDownContent = File.ReadAllText(markdownFile);
                    var category =  (new DirectoryInfo(directory)).Name;
                    posts.Add(new Post(){
                        Body = markDownContent,
                        Category = category,
                        Title = title,
                        PostDateTime = postDateTime
                    });
                }
            }

            return posts;
        }

        private readonly IOptions<BlogConfiguration> _blogConfiguration;
        private readonly IWebHostEnvironment _environment;

        public IEnumerable<Post> AllPosts => _posts;

        public IEnumerable<Post> GetPostsByCategory(string categoryName)
        {
            var numberOfPostsPerPage = _blogConfiguration.Value.NumberOfPostsPerPage;

            return _posts.Where(x => x.Category.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).Take(numberOfPostsPerPage);
        }

        public Post GetPostByTitle(string postTitle)
        {
            return _posts.FirstOrDefault(x => x.Title.GenerateSlug().Equals(postTitle, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Post> GetPostsForPaginatedPage(int pageNumber)
        {
            var numberOfPostsPerPage = _blogConfiguration.Value.NumberOfPostsPerPage;

            var numberOfPostsToSkip = (pageNumber - 1) * numberOfPostsPerPage;

            return _posts.OrderByDescending(x => x.PostDateTime)
                .Skip(numberOfPostsToSkip)
                .Take(numberOfPostsPerPage)
                .ToList();
        }

        public IEnumerable<Post> GetPostsForPaginatedCategoryPage(int pageNumber, string category)
        {
            var numberOfPostsPerPage = _blogConfiguration.Value.NumberOfPostsPerPage;

            var numberOfPostsToSkip = (pageNumber - 1) * numberOfPostsPerPage;

            return _posts.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.PostDateTime)
                .Skip(numberOfPostsToSkip)
                .Take(numberOfPostsPerPage)
                .ToList();
        }

        public IEnumerable<Post> GetPostsForHomePage()
        {
            var numberOfPostsPerPage = _blogConfiguration.Value.NumberOfPostsPerPage;

            return _posts.OrderByDescending(x => x.PostDateTime)
                .Take(numberOfPostsPerPage)
                .ToList();
        }

        public int TotalPosts => _posts.Count;
        public int GetPostsCountForCategory(string category)
        {
            return _posts.Count(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }
    }
}