using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;

        public IndexModel(NguyenCoffeeWeb.Models.postgresContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Type") != "0")
            {
                return Redirect("/Index");
            }

            if (_context.Categories != null)
            {
                Category = await _context.Categories.ToListAsync();
            }
            return Page();
        }
    }
}
