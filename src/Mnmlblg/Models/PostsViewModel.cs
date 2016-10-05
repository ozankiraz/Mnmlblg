using System.Collections.Generic;

namespace Mnmlblg.Models
{
    public class PostsViewModel
    {
        public IEnumerable<PostViewModel> PostViewModels { get; set; }

        public string PreviousPagePath { get; set; }

        public string NextPagePath { get; set; }
    }
}