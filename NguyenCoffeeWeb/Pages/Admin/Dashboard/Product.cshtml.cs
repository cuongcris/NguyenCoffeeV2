using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;
using PagedList;

namespace NguyenCoffeeWeb.Pages.Admin.Dashboard
{
    public class ProductModel : PageModel
    {
		private readonly postgresContext _context;

		public ProductModel(postgresContext context)
		{
			_context = context;
		}

		public IPagedList<Product> Product { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }
		public  void OnGet(int? pageNumber)
		{
			if (_context.Products != null)
			{
				var page = pageNumber ?? 1;
				var pageSize = 4;

				var products = from p in _context.Products
							   .Include(p => p.Category)
							   select p;

				if (!string.IsNullOrEmpty(SearchString))
				{
					products = products.Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower()) || p.Category.CategoryName.ToLower().Contains(SearchString.ToLower()));
				}

				Product = products.OrderBy(p => p.ProductName).ToPagedList(page, pageSize);
			}
		}
		
	}
}
