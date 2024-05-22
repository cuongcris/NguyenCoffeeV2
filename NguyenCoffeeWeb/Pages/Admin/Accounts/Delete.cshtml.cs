using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        private readonly IHubContext<SignalrServer> _signalHub;
        public DeleteModel(NguyenCoffeeWeb.Models.postgresContext context, IHubContext<SignalrServer> signalHub)
        {
            _context = context;
            _signalHub = signalHub;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }
            Account? account = await _context.Accounts.FindAsync(id);

            if (account != null)
            {
                Account = account;
                _context.Accounts.Remove(Account);
                await _context.SaveChangesAsync();
                await _signalHub.Clients.All.SendAsync("LoadAccount");
            }

            return RedirectToPage("./Index");
        }
    }
}
