using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalePriceController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public SalePriceController(FinalProjectDatbase context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SalePrice> prices = await _context.SalePrices
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(prices);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SalePrice price)
        {
            await _context.SalePrices.AddAsync(price);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            SalePrice? price = await _context.SalePrices.Where(x => !x.IsDeleted && x.Id == id)
                  .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            price.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            SalePrice? price = await _context.SalePrices.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            return View(price);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, SalePrice price)
        {
            SalePrice? PriceUpdate = await _context.SalePrices.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            PriceUpdate.FromPrice = price.FromPrice;
            PriceUpdate.ToPrice = price.ToPrice;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
