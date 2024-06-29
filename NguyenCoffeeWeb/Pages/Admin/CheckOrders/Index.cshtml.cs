using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Admin.CheckOrders
{
    public class IndexModel : PageModel
    {
        private readonly postgresContext _context;
        public IndexModel(postgresContext postgresContext)
        {
            _context = postgresContext;
        }
        public IList<OrderInTable> OrderInTables { get; set; } = new List<OrderInTable>();
        public async void OnGet()
        {

            if (_context.OrderInTables.Any())
            {
                OrderInTables = await _context.OrderInTables.Where(s => !s.IsPay).ToListAsync();
            }
        }
    }
}
