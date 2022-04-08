using System;

namespace ImagePosts.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageName { get; set; }
        public DateTime DateUploaded { get; set; }
        public int NumberOfLikes { get; set; }
    }
}
