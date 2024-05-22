using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Products
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
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (HttpContext.Session.GetString("Type") != "0")
            {
                return Redirect("/Index");
            }

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            Product? product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _signalHub.Clients.All.SendAsync("LoadProducts");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
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

        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
