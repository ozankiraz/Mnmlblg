using Microsoft.AspNetCore.Mvc;
using Mnmlblg.Models;

namespace Mnmlblg.ViewComponents
{
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(string olderPostsPageNumber, string newerPostsPageNumber, string path)
        {
            return View("Default", new PaginationViewModel { PreviousPagePath = newerPostsPageNumber, NextPagePath = olderPostsPageNumber ,Path = path});
        }
    }
}