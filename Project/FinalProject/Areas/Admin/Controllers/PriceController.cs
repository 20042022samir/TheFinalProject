using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PriceController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public PriceController(FinalProjectDatbase context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Price> prices = await _context.Prices
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(prices);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Price price)
        {
            await _context.Prices.AddAsync(price);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
          Price? price=  await _context.Prices.Where(x => !x.IsDeleted && x.Id == id)
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
            Price? price = await _context.Prices.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            return View(price);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Price price)
        {
            Price? PriceUpdate = await _context.Prices.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            PriceUpdate.FromPrice = price.FromPrice;
            PriceUpdate.ToPrice=price.ToPrice;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
