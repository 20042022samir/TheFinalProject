using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertiseController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _enviroment;
        public AdvertiseController(FinalProjectDatbase context,IWebHostEnvironment enviroment)
        {
            _context=context;
            _enviroment=enviroment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Advertise> advertises = await _context.Advertises
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(advertises);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Advertise advertise)
        {
            advertise.Image = advertise.file.CreateImage(_enviroment.WebRootPath, "Assets/Images");
            advertise.CreatedDate = DateTime.Now;
            await _context.Advertises.AddAsync(advertise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
           Advertise? advertise= await _context.Advertises.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (advertise == null)
                return NotFound();
            advertise.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
           Advertise? advertise= await _context.Advertises.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (advertise == null)
                return NotFound();
            return View(advertise);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Advertise advertise)
        {
            Advertise advertiseUpdate=await _context.Advertises.Where(x=>!x.IsDeleted && x.Id==id).FirstOrDefaultAsync();
            if(advertiseUpdate==null)
                return NotFound();
            advertiseUpdate.Image = advertise.file.CreateImage(_enviroment.WebRootPath, "Assets/Images");
            advertiseUpdate.Descripption = advertise.Descripption;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
