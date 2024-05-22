using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        public IndexModel(NguyenCoffeeWeb.Models.postgresContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Type") != "0")
            {
                return Redirect("/Index");
            }

            if (_context.Accounts != null)
            {
                Account = await _context.Accounts.ToListAsync();
            }
            return Page();
        }
    }
}
