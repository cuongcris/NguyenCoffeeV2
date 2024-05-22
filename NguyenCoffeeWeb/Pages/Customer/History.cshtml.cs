using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Customer
{
    public class HistoryModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        public HistoryModel(NguyenCoffeeWeb.Models.postgresContext context)
        {
            _context = context;
        }

        public IList<OrdersOnline> OrdersOnline { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.OrdersOnlines != null)
            {
                string accountSession = HttpContext.Session.GetString("Account")!;
                var curAccount = JsonSerializer.Deserialize<Account>(accountSession)!;

                OrdersOnline = await _context.OrdersOnlines
                .Include(o => o.User).Where(o => o.UserId == curAccount.Id)
                .ToListAsync();
            }
        }
    }
}
