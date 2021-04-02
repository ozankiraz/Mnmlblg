namespace Mnmlblg.Configuration
{
    public class BlogConfiguration
    {
        public int NumberOfPostsPerPage { get; set; }

        public string BlogTitle { get; set; }

        public string BlogSlogan { get; set; }

        public string AvatarUrl { get; set; }

        public int RssFeedNumberOfItemsToShow { get; set; }

        public string RssAuthorName { get; set; }

        public string RssAuthorEmailAddress { get; set; }

        public int RssPreviewMaxCharacters { get; set; }
    }
}