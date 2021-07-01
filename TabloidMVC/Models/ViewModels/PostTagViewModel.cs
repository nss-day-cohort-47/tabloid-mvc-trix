using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }
        public Tag Tag { get; set; }
        public List<Tag> TagOptions { get; set; }
    }
}
