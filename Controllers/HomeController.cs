using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myBlog.DB;
using myBlog.Models;

namespace myBlog.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        readonly ApplicationDBContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
  
        public IActionResult contact()
        {
            return View();
        }
        public IActionResult index()
        {
            var blog = _context.Blogs;
            ViewData["recommendblog"] = _context.Blogs.OrderByDescending(x => x.Views).Take(3).ToList();
            return View(blog);
        }
        public IActionResult about()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
