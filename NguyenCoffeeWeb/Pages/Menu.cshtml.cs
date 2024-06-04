using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NguyenCoffeeWeb.Models;
using NguyenCoffeeWeb.Models.Dtos;

namespace NguyenCoffeeWeb.Pages
{
    public class MenuModel : PageModel
    {

        [BindProperty]
        public string CoffeeOrderList { get; set; }

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
        public async Task<IActionResult> OnPostAsync()
        {
            string? coffeeOrderListText = Request.Form["CoffeeOrderList"];
            /*string? totalPrice = Request.Form["totalPrice"];*/

            var coffeeOrders = ParseCoffeeOrderList(coffeeOrderListText);
            string coffeeOrderJson = JsonConvert.SerializeObject(coffeeOrders);
            HttpContext.Session.SetString("CoffeeOrderList", coffeeOrderJson);
    /*        HttpContext.Session.SetString("totalPrice", totalPrice);*/
            // Redirect to a confirmation page or perform other actions
            return RedirectToPage("AiHandleDemo/Index");
        }
        private List<CoffeeOrderDto> ParseCoffeeOrderList(string coffeeOrderListText)
        {
            var coffeeOrders = new List<CoffeeOrderDto>();

            // Split the text into individual lines
            string[] lines = coffeeOrderListText.Split("\n");

            foreach (string line in lines)
            {
                // Split each line into name and quantity parts
                string[] parts = line.Trim().Split('x');

                if (parts.Length < 2)
                {
                    // Ignore invalid lines
                    continue;
                }

                string coffeeName = parts[0];
                int quantity = int.Parse(parts[1]);

                // Create a CoffeeOrder object and add it to the list
                coffeeOrders.Add(new CoffeeOrderDto { Name = coffeeName, Quantity = quantity });
            }

            return coffeeOrders;
        }
    }
}
