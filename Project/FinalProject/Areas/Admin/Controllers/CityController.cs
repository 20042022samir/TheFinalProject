using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CityController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public CityController(FinalProjectDatbase context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<City> Cities = await _context.Cities.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(Cities);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(City City)
        {
            await _context.Cities.AddAsync(City);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
          City? city=  await _context.Cities.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (city == null)
                return NotFound();
            city.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            City? city = await _context.Cities.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (city == null)
                return NotFound();
            return View(city);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,City city)
        {
            City? cityUpdate = await _context.Cities.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (city == null)
                return NotFound();
            cityUpdate.Name = city.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
