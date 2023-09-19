using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RayonController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public RayonController(FinalProjectDatbase context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Distinct> distincts = await _context.Distincts
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(distincts);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Distinct distinct)
        {
            await _context.Distincts.AddAsync(distinct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Distinct? distinct = await _context.Distincts.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (distinct == null)
                return NotFound();
            distinct.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Distinct? distinct = await _context.Distincts.Where(x => !x.IsDeleted && x.Id == id)
           .FirstOrDefaultAsync();
            if (distinct == null)
                return NotFound();
            return View(distinct);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Distinct distinct)
        {
            Distinct? distinctUpdate = await _context.Distincts.Where(x => !x.IsDeleted && x.Id == id)
           .FirstOrDefaultAsync();
            if (distinctUpdate == null)
                return NotFound();
            distinctUpdate.Name=distinct.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
