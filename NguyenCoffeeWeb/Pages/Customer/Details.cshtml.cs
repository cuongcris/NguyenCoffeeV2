using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Customer
{
    public class DetailsModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        public DetailsModel(NguyenCoffeeWeb.Models.postgresContext context)
        {
            _context = context;
        }

        public IList<OrderDetail> OrderDetail { get;set; } = default!;

        public async Task OnGetAsync(string orderID)
        {
            if (_context.OrderDetails != null)
            {
                OrderDetail = await _context.OrderDetails
                .Include(o => o.Product).Where(o => o.OrderId.Equals(orderID))
                .ToListAsync();
            }
        }
    }
}
