using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        public DetailsModel(NguyenCoffeeWeb.Models.postgresContext context)
        {
            _context = context;
        }

        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (HttpContext.Session.GetString("Type") != "0")
            {
                return Redirect("/Index");
            }
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            Account? account = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                Account = account;
            }
            return Page();
        }
    }
}
