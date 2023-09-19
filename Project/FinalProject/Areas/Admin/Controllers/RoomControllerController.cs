using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomControllerController : Controller
    {
        private readonly FinalProjectDatbase _context;
        public RoomControllerController(FinalProjectDatbase context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<RoomCount> roomCounts = await _context.RoomCounts
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(roomCounts);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoomCount count)
        {
            await _context.RoomCounts.AddAsync(count);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
           RoomCount? count= await _context.RoomCounts.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (count == null)
                return NotFound();
            return View(count);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,RoomCount count)
        {
            RoomCount? countUpdte = await _context.RoomCounts.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(countUpdte==null)
                return NotFound();
            countUpdte.UpdatedDate = DateTime.Now;
            countUpdte.Count = count.Count;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            RoomCount count = await _context.RoomCounts.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (count == null)
                return NotFound();
            count.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
