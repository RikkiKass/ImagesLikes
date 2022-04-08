using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImagePosts.Data;
using ImagePosts.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImagePosts.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HomeController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
     
        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImageRepository(connectionString);
            var vm = new IndexViewModel
            {
                Images = repo.GetAllImages()
            };
            return View(vm);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(string title, IFormFile image)
        {
            string fileName = $"{Guid.NewGuid()}-{image.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            image.CopyTo(fs);

            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImageRepository(connectionString);
            Image i = new Image
            {
                ImageName = fileName,
                ImageTitle = title,
                DateUploaded = DateTime.Now,
                NumberOfLikes = 0
            };
            repo.AddImage(i);
            return Redirect("/");

           
        }
        public IActionResult ViewImage(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImageRepository(connectionString);
            var image = repo.GetImageForId(id);
            return View(new ViewImageViewModel
            {
                Image = image,
                ImagesViewed = HttpContext.Session.Get<List<int>>("Ids")
            }); 
            
        }
        public IActionResult GetLikes(int id)
        {

            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImageRepository(connectionString);
            int count=repo.GetLikes(id);
            return Json(count);
        }
        [HttpPost]
        public void LikeIt(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImageRepository(connectionString);
            repo.LikeIt(id);
            List<int> imagesLiked = HttpContext.Session.Get<List<int>>("Ids");
            if (imagesLiked == null)
            {
                imagesLiked = new List<int>();
            }
            imagesLiked.Add(id);
            HttpContext.Session.Set("Ids", imagesLiked);
        }
       
  
    }
}
