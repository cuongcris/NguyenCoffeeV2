using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Categories
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

            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Categories == null || Category == null)
            {
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            await _signalHub.Clients.All.SendAsync("LoadCategories");
            return RedirectToPage("./Index");
        }
    }
}
