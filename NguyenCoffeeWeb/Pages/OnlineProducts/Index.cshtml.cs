using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace NguyenCoffeeWeb.Pages.OnlineProducts
{
    public class IndexModel : PageModel
    {
        private readonly postgresContext _context;

        public IndexModel(postgresContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products
                .Include(p => p.Category).Where(p=>p.IsOnline==true).ToListAsync();
            }
        }
        public IActionResult OnGetLoadMoreProducts(int skip, int take)
        {
            var moreProducts = _context.Products.Where(p=>p.IsOnline==true).Skip(skip).Take(take).ToList();
            return new JsonResult(moreProducts);
        }
        public IActionResult OnGetSearchByName(string name)
        {
            var searchResults =  _context.Products
                .Where(p => p.IsOnline == true && p.ProductName.ToLower().Contains(name.ToLower()))
                .ToList();
            return new JsonResult(searchResults);

        }
    }
    }