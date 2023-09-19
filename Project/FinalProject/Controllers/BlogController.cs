using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FinalProject.Controllers
{
	public class BlogController : Controller
	{
		private readonly FinalProjectDatbase _context;
		private readonly UserManager<AppUser> _userManager;
		public BlogController(FinalProjectDatbase context,UserManager<AppUser> userManager)
		{
			_context= context;
			_userManager= userManager;
		}
		[HttpGet]
		public async Task<IActionResult> Index(int page=1, int pageSize = 3)
		{
            var blogs = await _context.Blogs
				.Where(x => !x.IsDeleted)
                .OrderByDescending(blog => blog.ViewCount)
                .Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
            var totalItems = await _context.Blogs.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
			var sortedBlogs = blogs.ToList();
            var viewModel = new PagginationViewModel<Blog>
            {
                Items = blogs,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return View(viewModel);
		}
		[HttpGet]
		public async Task<IActionResult> Detail(int id)
		{
            Blog? blog = await _context.Blogs
				.Where(x => !x.IsDeleted && x.Id == id)
				.FirstOrDefaultAsync();
			blog.ViewCount++;
			await _context.SaveChangesAsync();
            ViewBag.ExtraThree = await _context.Blogs
				.Where(x => !x.IsDeleted && x.Id != id)
				.OrderByDescending(x=>x.CreatedDate)
				.Take(4)
				.ToListAsync();
			return View(blog);
		}


		

	}
}
