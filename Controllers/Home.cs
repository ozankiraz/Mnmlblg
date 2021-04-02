using System.Linq;
using MarkdownSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mnmlblg.Extensions;
using Mnmlblg.Models;
namespace Mnmlblg.Controllers
{
    public class Home : Controller
    {
        private readonly IMarkDown _markdownProcessor;
        private readonly IPostRepository _postRepository;

        public Home(IOptions<MarkdownOptions> markdownOptions, IPostRepository postRepository, IMarkDown markdownProcessor)
        {
            _markdownProcessor = markdownProcessor.WithOptions(markdownOptions.Value);
            _postRepository = postRepository;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var posts = _postRepository.GetPostsForHomePage();

            var postViewModels = posts.Select(x => new PostViewModel
            {
                Title = x.Title,
                PostDateTime = string.Format("{0:r}", x.PostDateTime),
                SluggedTitle = x.Title.GenerateSlug(),
                Body = _markdownProcessor.Transform(x.Body),
                Category = x.Category.ToLower()
            });

            var postsViewModesl = new PostsViewModel
            {
                PostViewModels = postViewModels,
                PreviousPagePath = string.Empty,
                NextPagePath = Url.AbsoluteContent(string.Format("pages/{0}", 2.ToString()).ToLower())
            };

            return View("DisplayPosts", postsViewModesl);
        }
    }
}
