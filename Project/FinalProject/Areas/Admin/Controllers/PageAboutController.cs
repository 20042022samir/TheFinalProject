using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
        [Area("Admin")]
    public class PageAboutController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public PageAboutController(FinalProjectDatbase context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PageAbout> about = await _context.PageAbouts
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(about);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PageAbout about)
        {
            await _context.PageAbouts.AddAsync(about);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
          PageAbout? about=  await _context.PageAbouts.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (about == null)
                return NotFound();
            about.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            PageAbout? about = await _context.PageAbouts.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (about == null)
                return NotFound();
            return View(about);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,PageAbout about)
        {
            PageAbout? aboutUpdate = await _context.PageAbouts.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (aboutUpdate == null)
                return NotFound();
            aboutUpdate.FaceboookLink= about.FaceboookLink; aboutUpdate.InstagramLink=about.InstagramLink;
            aboutUpdate.LinkedinLink = about.LinkedinLink;aboutUpdate.Contry = about.Contry; aboutUpdate.Email=about.Email;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
