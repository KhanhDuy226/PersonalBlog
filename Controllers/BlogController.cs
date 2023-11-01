using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using myBlog.DB;
using myBlog.Models;

namespace myBlog.Controllers
{
    public class BlogController : Controller
    {

        private readonly ILogger<BlogController> _logger;
        readonly ApplicationDBContext _context;
        public BlogController(ILogger<BlogController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult createblog()
        {
            if (HttpContext.Session.GetString("role") == "admin") ViewData["isAdmin"] = true;
            return View();
        }
        [HttpPost("/Blog/createblog")]
        public async Task<IActionResult> createblog(Blog Object, IFormFile ImageFileRaw)
        {
            List<String> Errors = new List<string>();
            if (ImageFileRaw == null) Errors.Add("Please choose a image to presentation!");

            else if (ModelState.IsValid)
            {
                String ext = Path.GetExtension(ImageFileRaw.FileName);
                List<String> image_extensions = new List<string>() { ".jpg", ".png", ".jpeg", ".gif" };
                if (!image_extensions.Contains(ext)) Errors.Add("File format invalid!");
                else
                {
                    Object.ImageURL = SaveImage(ImageFileRaw);
                    await _context.Blogs.AddAsync(Object);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Detail", new { blog_id = Object.ID });
                }
            }

            ViewData["Errors"] = Errors;
            return View(Object);
        }

        public String SaveImage(IFormFile ImageFile)
        {

            string Name = $"img_{Guid.NewGuid()}" + Path.GetExtension(ImageFile.FileName);

            //Get url To Save
            string RelativeSaveDirectory = $"/media/BlogPhotos/";
            string AbsoluteSaveFullPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot" + RelativeSaveDirectory + Name;

            if (Directory.Exists(Path.GetDirectoryName(AbsoluteSaveFullPath)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AbsoluteSaveFullPath));
            }
            using (FileStream stream = new FileStream(AbsoluteSaveFullPath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

            return $"{RelativeSaveDirectory}{Name}?last_update={-DateTime.Now.ToBinary()}";
        }

        public async Task<IActionResult> Detail(int blog_id)
        {
            if (HttpContext.Session.GetString("role") == "admin") ViewData["isAdmin"] = true;

            Blog obj = await _context.Blogs.FindAsync(blog_id);
            if (obj == null) return NotFound();
            obj.Views++;
            _context.Blogs.Update(obj);
            _context.SaveChanges();
            return View(obj);
        }

        //view Edit
        public async Task<IActionResult> Edit(int blog_id)
        {
            if (HttpContext.Session.GetString("role") == "admin") ViewData["isAdmin"] = true;
            else
            {
                return RedirectToAction("login", "admin");
            }
            Blog model = await _context.Blogs.FindAsync(blog_id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Blog Object, IFormFile ImageFileRaw)
        {

            List<String> Errors = new List<string>();

            if (ModelState.IsValid)
            {
                if (ImageFileRaw != null)
                {

                    String ext = Path.GetExtension(ImageFileRaw.FileName);
                    List<String> image_extensions = new List<string>() { ".jpg", ".png", ".jpeg", ".gif" };
                    if (!image_extensions.Contains(ext)) Errors.Add("File format invalid!");

                    else
                    {
                        Object.ImageURL = SaveImage(ImageFileRaw);
                        Object.ReleaseDate = DateTime.Now;
                    }
                }

                _context.Blogs.Update(Object);
                await _context.SaveChangesAsync();

                return RedirectToAction("Detail", new { blog_id = Object.ID });
            }
            Errors.AddRange(ModelState.Values.SelectMany(item => item.Errors).Select(error => error.ErrorMessage).ToList());
            ViewData["Errors"] = Errors;

            return RedirectToAction("home", "admin");

        }


        // delete function
        public IActionResult Delete(int blog_id)
        {
            if (HttpContext.Session.GetString("role") == "admin") ViewData["isAdmin"] = true;

            Blog obj = this._context.Blogs.Find(blog_id);
            if (obj == null)
            {
                return NotFound();
            }
            this._context.Blogs.Remove(obj);
            this._context.SaveChanges();
            return RedirectToAction("home", "admin");
        }

        
    }
}
