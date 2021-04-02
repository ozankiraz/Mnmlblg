using System;
using System.Linq;
using MarkdownSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mnmlblg.Configuration;
using Mnmlblg.Extensions;
using Mnmlblg;
using Mnmlblg.Models;
namespace Mnmlblg.Controllers
{
    public class Category : Controller
    {
        private readonly IMarkDown _markdownProcessor;
        private readonly BlogConfiguration _blogConfiguration;
        private readonly IPostRepository _postRepository;

        public Category(IOptions<MarkdownOptions> markdownOptions,
            IOptions<BlogConfiguration> blogConfiguration,
            IPostRepository postRepository,
            IMarkDown markdownProcessor)
        {
            _markdownProcessor = markdownProcessor.WithOptions(markdownOptions.Value);
            _blogConfiguration = blogConfiguration.Value;
            _postRepository = postRepository;
        }

        [Route("/category/")]
        public IActionResult Index(string categoryName)
        {
            return RedirectToAction("Index", "Home");
        }

        [Route("/category/{categoryName:alpha}/{categoryPage:int?}")]
        public IActionResult ShowPostsInCategory(string categoryName, int categoryPage = 1)
        {
            var numberOfPostsPerPage = _blogConfiguration.NumberOfPostsPerPage;

            var posts = _postRepository.GetPostsForPaginatedCategoryPage(categoryPage, categoryName);

            var postViewModels = posts.Select(x => new PostViewModel
            {
                Title = x.Title,
                PostDateTime = string.Format("{0:r}", x.PostDateTime),
                SluggedTitle = x.Title.GenerateSlug(),
                Body = _markdownProcessor.Transform(x.Body),
                Category = x.Category.ToLower()
            });

            var lastPageNumber = Math.Ceiling((double)_postRepository.GetPostsCountForCategory(categoryName) / numberOfPostsPerPage);

            var showOlderPostsLink = categoryPage < lastPageNumber;

            var showNewerPostsLink = categoryPage > 1;

            var controller = GetType().Name;

            var postsViewModesl = new PostsViewModel
            {
                PostViewModels = postViewModels,
                PreviousPagePath = showNewerPostsLink ? base.Url.AbsoluteContent(string.Format("{0}/{1}/{2}", controller, categoryName, ((categoryPage - 1).ToString()))) : string.Empty,
                NextPagePath = showOlderPostsLink ? base.Url.AbsoluteContent(string.Format("{0}/{1}/{2}", controller, categoryName, ((categoryPage + 1).ToString()))) : string.Empty
            };

            return View("DisplayPosts", postsViewModesl);
        }
    }
}