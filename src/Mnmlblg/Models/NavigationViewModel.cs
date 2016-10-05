using System.Collections.Generic;

namespace Mnmlblg.Models
{
    public class NavigationViewModel
    {
        public IEnumerable<NavigationItem> NavigationItems { get; set; }

        public string BlogTitle { get; set; }

        public string BlogSlogan { get; set; }

        public string AvatarUrl { get; set; }

        public string AllPostsUrl { get; set; }
        public string FeedUrl { get; set; }
    }
}