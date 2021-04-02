using Mnmlblg.Extensions;

namespace Mnmlblg.Models
{
    public class NavigationItem
    {
        public string Name { get; set; }

        public string UrlName => Name.GenerateSlug();
    }
}