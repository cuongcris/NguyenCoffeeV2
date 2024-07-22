using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;
using PagedList;
using static NuGet.Packaging.PackagingConstants;

namespace NguyenCoffeeWeb.Pages.Admin.Dashboard
{
    public class OrderModel : PageModel
    {
		private readonly postgresContext _context;
		public OrderModel(postgresContext context)
		{
			_context = context;
		}

		public IPagedList<OrdersOnline> Orders { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }
		public void OnGet(int? pageNumber)
		{
			if (_context.Products != null)
			{
				var page = pageNumber ?? 1;
				var pageSize = 4;

				var orders = from p in _context.OrdersOnlines.Include(s=>s.User)
								 select p;
				Orders = orders.OrderBy(p => p.ShippedDate).ToPagedList(page, pageSize);
			}
		}
	}
}
