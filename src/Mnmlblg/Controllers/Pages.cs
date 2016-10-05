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
    public class Pages : Controller
    {
        private readonly IMarkDown _markdownProcessor;
        private readonly IPostRepository _postRepository;
        private readonly BlogConfiguration _blogConfiguration;

        public Pages(IOptions<MarkdownOptions> markdownOptions, IOptions<BlogConfiguration> blogConfiguration, IPostRepository postRepository, IMarkDown markdownProcessor)
        {
            _markdownProcessor = markdownProcessor.WithOptions(markdownOptions.Value);
            _blogConfiguration = blogConfiguration.Value;
            _postRepository = postRepository;
        }

        [Route("/pages")]
        public IActionResult Index(string categoryName)
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: /<controller>/
        [Route("/pages/{currentPage:int}")]
        public IActionResult ShowPage(int? currentPage = 1)
        {
            var numberOfPostsPerPage = _blogConfiguration.NumberOfPostsPerPage;

            var posts = _postRepository.GetPostsForPaginatedPage(currentPage.GetValueOrDefault());

            var postViewModels = posts.Select(x => new PostViewModel
            {
                Title = x.Title,
                PostDateTime = string.Format("{0:r}", x.PostDateTime),
                SluggedTitle = x.Title.GenerateSlug(),
                Body = _markdownProcessor.Transform(x.Body),
                Category = x.Category.ToLower()
            });

            var lastPageNumber = Math.Ceiling((double) _postRepository.TotalPosts / numberOfPostsPerPage);

            var showNewerPostsLink = currentPage > 1;
            var showOlderPostsLink = currentPage < lastPageNumber; 

            var controller = GetType().Name;

            var postsViewModesl = new PostsViewModel
            {
                PostViewModels = postViewModels,
                PreviousPagePath = showNewerPostsLink ? Url.AbsoluteContent(string.Format("{0}/{1}",controller, ((currentPage - 1).ToString()))).ToLower() : string.Empty,
                NextPagePath = showOlderPostsLink ? Url.AbsoluteContent(string.Format("{0}/{1}", controller, ((currentPage + 1).ToString()))).ToLower() : string.Empty
            };

            return View("DisplayPosts", postsViewModesl);
        }
    }
}