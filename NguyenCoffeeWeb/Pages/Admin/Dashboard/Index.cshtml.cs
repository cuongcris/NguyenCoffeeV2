using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Admin.Dashboard
{
	public class IndexModel : PageModel
	{
		private readonly postgresContext _context;

		public IndexModel(postgresContext context)
		{
			_context = context;
		}

		public Dictionary<string, int> OrdersByMonth { get; set; }
		public Dictionary<string, float> RevenueByMonth { get; set; }

		public void OnGet()
		{
			// Lấy tất cả đơn hàng
			var orders = _context.OrdersOnlines
				.ToList();

			// Nhóm đơn hàng theo tháng
			OrdersByMonth = orders
				.GroupBy(o => new { Year = o.CreatedAt.Year, Month = o.CreatedAt.Month })
				.Select(g => new
				{
					MonthYear = $"{g.Key.Month:D2}/{g.Key.Year}",
					Count = g.Count()
				})
				.OrderBy(x => x.MonthYear)
				.ToDictionary(x => x.MonthYear, x => x.Count);

			// Tính doanh thu theo từng tháng
			RevenueByMonth = orders
				.SelectMany(o => _context.OrderDetails
					.Where(od => od.OrderId == o.Id)
					.Select(od => new
					{
						MonthYear = $"{o.CreatedAt.Month:D2}/{o.CreatedAt.Year}",
						Revenue = (od.UnitPrice ?? 0) * (od.Quanlity ?? 0)
					}))
				.GroupBy(x => x.MonthYear)
				.Select(g => new
				{
					MonthYear = g.Key,
					Revenue = g.Sum(x => x.Revenue)
				})
				.OrderBy(x => x.MonthYear)
				.ToDictionary(x => x.MonthYear, x => x.Revenue);
		}
	}
}
