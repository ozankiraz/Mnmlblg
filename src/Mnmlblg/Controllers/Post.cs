using MarkdownSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mnmlblg.Extensions;
using Mnmlblg.Models;

namespace Mnmlblg.Controllers
{
    public class Post : Controller
    {
        private readonly IMarkDown _markdownProcessor;
        private readonly IPostRepository _postRepository;

        public Post(IOptions<MarkdownOptions> markdownOptions, IPostRepository postRepository, IMarkDown markdownProcessor)
        {
            _markdownProcessor = markdownProcessor.WithOptions(markdownOptions.Value);
            _postRepository = postRepository;
        }

        [Route("/{PostTitle}")]
        public IActionResult ShowPost(string postTitle)
        {
            var sluggedPostTitle = postTitle.GenerateSlug();
            var post = _postRepository.GetPostByTitle(sluggedPostTitle);

            var postViewModel = new PostViewModel
            {
                Title = post.Title,
                SluggedTitle = post.Title.GenerateSlug(),
                Body = _markdownProcessor.Transform(post.Body),
                PostDateTime = string.Format("{0:r}", post.PostDateTime),
                Category = post.Category.ToLower()
            };

            return View(postViewModel);
        }
    }
}