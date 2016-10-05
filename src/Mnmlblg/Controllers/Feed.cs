using Microsoft.AspNetCore.Mvc;

namespace Mnmlblg.Controllers
{
    public class Feed : Controller
    {
        private readonly IRss2FeedGenerator _rss2FeedGenerator;

        public Feed(IRss2FeedGenerator rss2FeedGenerator)
        {
            _rss2FeedGenerator = rss2FeedGenerator;
        }

        [Route("/feed")]
        [HttpGet]
        public IActionResult Get()
        {
            var feedPath = GetType().Name.ToLower();

            return new ContentResult
            {
                Content = _rss2FeedGenerator.Generate(feedPath, Url).ToString(),
                ContentType = "application/xml",
                StatusCode = 200
            };
        }


    }
}