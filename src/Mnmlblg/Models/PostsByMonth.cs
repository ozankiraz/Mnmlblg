using System.Collections.Generic;

namespace Mnmlblg.Models
{
    public class PostsByMonth
    {
        public string MonthName { get; set; }
        public IEnumerable<PostByDay> PostByDay { get; set; } = new List<PostByDay>();
    }
}