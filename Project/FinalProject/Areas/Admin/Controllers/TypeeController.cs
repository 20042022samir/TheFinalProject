using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypeeController : Controller
    {
        private readonly FinalProjectDatbase _contaxt;
        public TypeeController(FinalProjectDatbase contaxt)
        {
            _contaxt = contaxt;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Typee> Types = await _contaxt.Typees.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(Types);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Typee type)
        {
  
            await _contaxt.Typees.AddAsync(type);
            await _contaxt.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
         Typee? type=   await _contaxt.Typees.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (type == null)
                return NotFound();
            type.IsDeleted = true;
            await _contaxt.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult>  Update(int id)
        {
            Typee? type = await _contaxt.Typees.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (type == null)
                return NotFound();
            return View(type);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Typee Type)
        {
            if (!ModelState.IsValid)
                return View(Type);
            Typee? typeUpdate = await _contaxt.Typees.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (typeUpdate == null)
                return NotFound();
            typeUpdate.Name=Type.Name;
            await _contaxt.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
