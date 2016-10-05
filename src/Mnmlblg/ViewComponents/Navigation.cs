using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mnmlblg.Configuration;
using Mnmlblg.Extensions;
using Mnmlblg.Models;
using System.Linq;

namespace Mnmlblg.ViewComponents
{
    public class Navigation : ViewComponent
    {
        private readonly IPostRepository _postRepository;
        private readonly IOptions<BlogConfiguration> _blogConfiguration;

        public Navigation(IPostRepository postRepository, IOptions<BlogConfiguration> blogConfiguration)
        {
            _postRepository = postRepository;
            _blogConfiguration = blogConfiguration;
        }

        public IViewComponentResult Invoke()
        {
            var gravatarEmailAddress = _blogConfiguration.Value.GravatarEmailAddress;
            var avatarUrl = _blogConfiguration.Value.AvatarUrl;

            var navigationViewModel = new NavigationViewModel
            {
                BlogTitle = _blogConfiguration.Value.BlogTitle,
                BlogSlogan = _blogConfiguration.Value.BlogSlogan,
                NavigationItems = _postRepository.AllPosts.Where(x => x.IsNavigationPost).OrderBy(x => x.Title).Select(x => new NavigationItem { Name = x.Title }),
                AvatarUrl = _blogConfiguration.Value.AvatarUrl,
                AllPostsUrl = Url.AbsoluteContent("/archive"),
                FeedUrl = Url.AbsoluteContent("/feed")
            };

            if (!string.IsNullOrEmpty(gravatarEmailAddress))
            {
                navigationViewModel.AvatarUrl =
                    StringExtensions.GenerateGravatarUrl(_blogConfiguration.Value.GravatarEmailAddress);
            }
            else if (!string.IsNullOrEmpty(avatarUrl))
            {
                var avatarImageWithAbosutePath = Url.AbsoluteContent(avatarUrl);
                navigationViewModel.AvatarUrl = avatarImageWithAbosutePath;
            }

            return View(navigationViewModel);
        }
    }
}