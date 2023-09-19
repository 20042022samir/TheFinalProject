
using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public CountryController(FinalProjectDatbase context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Country> Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            return View(Countries);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Country country)
        {
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
           Country? country= await _context.Countries.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (country == null)
                return NotFound();
            country.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Country? country = await _context.Countries.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (country == null)
                return NotFound();
            return View(country);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Country country)
        {
            Country? countryUpdate = await _context.Countries.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (countryUpdate == null)
                return NotFound();
            countryUpdate.Name = country.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
