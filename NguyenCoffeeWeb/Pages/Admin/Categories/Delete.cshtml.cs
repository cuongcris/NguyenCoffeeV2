using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Categories
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
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (HttpContext.Session.GetString("Type") != "0")
            {
                return Redirect("/Index");
            }

            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            Category? category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            Category? category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                Category = category;
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
                await _signalHub.Clients.All.SendAsync("LoadCategories");
            }

            return RedirectToPage("./Index");
        }
    }
}
