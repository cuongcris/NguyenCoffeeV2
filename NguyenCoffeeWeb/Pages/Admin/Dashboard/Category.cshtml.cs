using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;
using PagedList;

namespace NguyenCoffeeWeb.Pages.Admin.Dashboard
{
    public class CategoryModel : PageModel
    {
		private readonly postgresContext _context;
		public CategoryModel(postgresContext context)
		{
			_context = context;
		}

		public IPagedList<Category> Category { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }
		public void OnGet(int? pageNumber)
		{
			if (_context.Products != null)
			{
				var page = pageNumber ?? 1;
				var pageSize = 4;

				var categories = from p in _context.Categories
							   select p;

				if (!string.IsNullOrEmpty(SearchString))
				{
					categories = categories.Where(p => p.CategoryName.ToLower().Contains(SearchString.ToLower()));
				}

				Category = categories.OrderBy(p => p.CategoryName).ToPagedList(page, pageSize);
			}
		}

	}
}
