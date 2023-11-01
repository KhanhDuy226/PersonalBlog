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
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        readonly ApplicationDBContext _context;
        public AdminController(ILogger<AdminController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult home()
        {
            var blog = _context.Blogs;
            ViewData["recommendblog"] = _context.Blogs.OrderByDescending(x => x.Views).Take(3).ToList();
    
            if (HttpContext.Session.GetString("role") != "admin") return RedirectToAction("login");
            ViewData["isAdmin"] = true;
            return View(blog);
        }

        //Mapping to login
        public IActionResult login()
        {

            if (HttpContext.Session.GetString("role") == "admin") return RedirectToAction("home");
            return View();
        }

        [HttpPost]
        public IActionResult login(Account info)
        {
            if (ModelState.IsValid)
            {
                Account user = _context.Accounts.Find(info.Email);
                if (user == null)
                {
                    ViewData["ErrorMessage"] = "Email not exist!";
                    return NotFound();

                }
                // Set Admin role
                if (user.Email == "admin@fpt.edu.vn" && user.Password == info.Password)
                {
                    HttpContext.Session.SetString("role", "admin");
                    HttpContext.Session.SetString("userEmail", user.Email);

                    return RedirectToAction("home");
                }

                ViewData["ErrorMessage"] = "Email or password incorrect!";

            }

            return View(info);
        }

        public IActionResult logout()
        {
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("userEmail");
            return RedirectToAction("login", "admin");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
