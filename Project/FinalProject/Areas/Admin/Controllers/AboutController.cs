using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _enviroment;
        public AboutController(FinalProjectDatbase context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AboutPage> pages = await _context
                .aboutPages.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(pages);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AboutPage page)
        {
            
            page.Image = page.file.CreateImage(_enviroment.WebRootPath, "Assets/Images");
            await _context.aboutPages.AddAsync(page);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AboutPage? page = await _context.aboutPages
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (page == null)
                return NotFound();
            page.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            AboutPage? page = await _context.aboutPages
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (page == null)
                return RedirectToAction("index", "error");
            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,AboutPage page)
        {
           AboutPage? pageUpdate= await _context.aboutPages.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (pageUpdate == null)
                return RedirectToAction("index", "error");
            pageUpdate.Description = page.Description;
            if (page.file != null)
            {
                pageUpdate.Image = page.file.CreateImage(_enviroment.WebRootPath, "Assets/Images");
            } else
                pageUpdate.Image = pageUpdate.Image;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
