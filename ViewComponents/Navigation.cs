using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mnmlblg.Configuration;
using Mnmlblg.Extensions;
using Mnmlblg.Models;
using System.Collections.Generic;

namespace Mnmlblg.ViewComponents
{
    public class Navigation : ViewComponent
    {
        private readonly IOptions<BlogConfiguration> _blogConfiguration;

        public Navigation(IPostRepository postRepository, IOptions<BlogConfiguration> blogConfiguration)
        {
            _blogConfiguration = blogConfiguration;
        }

        public IViewComponentResult Invoke()
        {
            var navigationViewModel = new NavigationViewModel
            {
                BlogTitle = _blogConfiguration.Value.BlogTitle,
                BlogSlogan = _blogConfiguration.Value.BlogSlogan,
                NavigationItems = new List<NavigationItem> { new NavigationItem { Name = "About Me" } },
                AvatarUrl = Url.AbsoluteContent(_blogConfiguration.Value.AvatarUrl),
                AllPostsUrl = Url.AbsoluteContent("/archive"),
                FeedUrl = Url.AbsoluteContent("/feed")
            };

            return View(navigationViewModel);
        }
    }
}