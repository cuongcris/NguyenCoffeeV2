using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        private readonly IHubContext<SignalrServer> _signalHub;
        public CreateModel(NguyenCoffeeWeb.Models.postgresContext context, IHubContext<SignalrServer> signalHub)
        {
            _context = context;
            _signalHub = signalHub;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Type") != "0")
            {
                return Redirect("/Index");
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            await _signalHub.Clients.All.SendAsync("LoadProducts");

            return RedirectToPage("./Index");
        }
    }
}
