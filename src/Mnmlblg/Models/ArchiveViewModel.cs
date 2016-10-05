using System.Collections.Generic;

namespace Mnmlblg.Models
{
    public class ArchiveViewModel
    {
        public string Title { get; set; }

        public IEnumerable<PostsByYear> PostsByYear { get; set; } = new List<PostsByYear>();
    }
}