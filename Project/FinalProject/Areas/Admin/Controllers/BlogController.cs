using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
        [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _enviroment;
        public BlogController(FinalProjectDatbase context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _context.Blogs
                .Where(x => !x.IsDeleted)
                 .ToListAsync();
            return View(blogs);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            blog.ViewCount= 0;
            blog.Image = blog.file.CreateImage(_enviroment.WebRootPath, "Assets/Images");
            blog.CreatedDate = DateTime.Now;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
          Blog? blog=  await _context.Blogs
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (blog == null)
                return RedirectToAction("index", "error");
            blog.IsDeleted= true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _context.Blogs
               .Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (blog == null)
                return RedirectToAction("index", "error");
            return View(blog);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Blog blog)
        {
            Blog? blogUpdate = await _context.Blogs
              .Where(x => !x.IsDeleted && x.Id == id)
              .FirstOrDefaultAsync();
            if (blog == null)
                return RedirectToAction("index", "error");
            blogUpdate.Tittle = blog.Tittle;
            if (blog.file != null)
                blogUpdate.Image = blog.file.CreateImage(_enviroment.WebRootPath, "Assets/Images");
            blogUpdate.Description = blog.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
