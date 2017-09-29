using System.Collections.Generic;

namespace EFCoreSamples.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }
}