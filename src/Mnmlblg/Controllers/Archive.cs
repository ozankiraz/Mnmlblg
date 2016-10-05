using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mnmlblg.Extensions;
using Mnmlblg.Models;

namespace Mnmlblg.Controllers
{
    public class Archive : Controller
    {
        private readonly IPostRepository _postRepository;

        public Archive(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        //TO DO
        [Route("/archive")]
        public IActionResult Index()
        {
            var postsByYear =
                _postRepository.AllPosts.GroupBy(post => post.PostDateTime.Year).Select(postsYearGroup => new
                {
                    Year = postsYearGroup.Key,
                    MonthGroups =
                        postsYearGroup.GroupBy(post => post.PostDateTime.Month).Select(postsByMonth => new { Month = postsByMonth.Key, Posts = postsByMonth.OrderByDescending(x => x.PostDateTime.Month) })
                }).OrderByDescending(x => x.Year);

            var archiveViewModel = new ArchiveViewModel
            {
                Title = "Archive",
                PostsByYear =
                    postsByYear.Select(
                        x =>
                            new PostsByYear
                            {
                                Year = x.Year,
                                PostsByMonth =
                                    x.MonthGroups.Select(
                                        y =>
                                            new PostsByMonth()
                                            {
                                                MonthName = new DateTime(1987, y.Month, 1).ToString("MMMM", CultureInfo.InvariantCulture),
                                                PostByDay =
                                                    y.Posts.Select(
                                                        z =>
                                                            new PostByDay()
                                                            {
                                                                Day = z.PostDateTime.Day.ToString("00"),
                                                                PostTitle = z.Title,
                                                                PostLink = z.Title.GenerateSlug()
                                                            })
                                            })
                            })
            };

            return View(archiveViewModel);
        }
    }
}