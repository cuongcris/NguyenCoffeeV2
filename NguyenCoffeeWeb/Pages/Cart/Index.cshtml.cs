using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NguyenCoffeeWeb.Helpers;
using NguyenCoffeeWeb.Models;
using System.Text;

namespace NguyenCoffeeWeb.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly NguyenCoffeeWeb.Models.postgresContext _context;


        public IndexModel(NguyenCoffeeWeb.Models.postgresContext context)
        {
            _context = context;
        }
        public IList<Item>? cart { get; set; } = new List<Item>();

        public IActionResult OnGet()
        {
            string accJson = HttpContext.Session.GetString("Account");
            
         
            if (accJson == null)
            {
                return  Redirect("/Authentication/Login");

            }
            Account acc = JsonConvert.DeserializeObject<Account>(accJson);
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");       
            cart = cart?.Where(x => x.UserId.Equals(acc.Id)).ToList();
            return  Page();


        }
        public async Task<IActionResult> OnPost()
        {
            string accJson = HttpContext.Session.GetString("Account")!;
            Account acc = JsonConvert.DeserializeObject<Account>(accJson)!;
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            cart = cart?.Where(x => x.UserId.Equals(acc.Id)).ToList();


            string orderOnlineId = GenerateUniqueString();
            _context.OrdersOnlines.Add(new OrdersOnline() { UserId = acc.Id, CreatedAt = DateTime.Now, 
                Id = orderOnlineId, ShippedDate = DateTime.Now.AddDays(5),
                PackagingDate = DateTime.Now.AddDays(5),Status = "Shipping" });
            await   _context.SaveChangesAsync();
            
            
            foreach (var item in cart!)
            {
                _context.OrderDetails.Add(new OrderDetail() {ProductId = item.Product.Id, OrderId = orderOnlineId, 
                    UnitPrice = (float)item.Product.UnitPrice!, Quanlity = item.Quantity  });
                await _context.SaveChangesAsync();
            }
            HttpContext.Session.Remove("cart");
            cart.Clear();
            ViewData["msg"] = "Payment Successfully!!!!!";
            return Page();
        }

        public static string GenerateUniqueString()
        {
            int length = 35;
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                stringBuilder.Append(characters[index]);
            }

            return stringBuilder.ToString();
        }

    }
 
}
