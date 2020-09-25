using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }
        //list of all tags that can be selected
        public List<Tags> TagList{ get; set; }
        //List of all tags for post
        public List<Tags> PostTagList { get; set; }
        //String of Tags selected
        public int[] SelectedTags { get; set; }
        public PostTag PostTags { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; }
    }
}
