using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Products
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
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            Product? product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                Product = product;
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();
                await _signalHub.Clients.All.SendAsync("LoadProducts");
            }

            return RedirectToPage("./Index");
        }
    }
}
