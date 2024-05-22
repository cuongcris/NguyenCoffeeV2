using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages
{
    public class MenuModel : PageModel
    {
        private readonly postgresContext _context;
        public MenuModel(postgresContext postgresContext)
        {
            _context = postgresContext;
        }
        public IList<Product> Products { get; set; }
        public void OnGet()
        {
            if (_context.Products != null)
            {
                Products = _context.Products.Where(p=>p.IsOnline==false).ToList();

            }
        }
    }
}
