using System.Collections.Generic;

namespace Mnmlblg.Models
{
    public class PostsByYear
    {
        public int Year { get; set; }
        public IEnumerable<PostsByMonth> PostsByMonth { get; set; } = new List<PostsByMonth>();
    }
}