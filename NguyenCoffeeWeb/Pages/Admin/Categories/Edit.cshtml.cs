using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;
        private readonly IHubContext<SignalrServer> _signalHub;
        public EditModel(NguyenCoffeeWeb.Models.postgresContext context, IHubContext<SignalrServer> signalHub)
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
            Category = category;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _signalHub.Clients.All.SendAsync("LoadCategories");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(Guid id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
