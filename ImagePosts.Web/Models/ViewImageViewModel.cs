using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImagePosts.Data;

namespace ImagePosts.Web.Models
{
    public class ViewImageViewModel
    {
        public Image Image { get; set; }
        public List<int>ImagesViewed { get; set; }

    }
}
