using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MetroController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public MetroController(FinalProjectDatbase context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Metro> Metros = await _context.Motros.Where(x => !x.IsDeleted).ToListAsync();
            return View(Metros);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Metro Metro)
        {
            await _context.Motros.AddAsync(Metro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Metro? metro= await _context.Motros.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (metro == null)
                return NotFound();
            metro.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Metro? metro = await _context.Motros.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (metro == null)
                return NotFound();
            return View(metro);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Metro metro)
        {
            Metro? metroUpdate = await _context.Motros.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (metroUpdate == null)
                return NotFound();
            metroUpdate.Name= metro.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
