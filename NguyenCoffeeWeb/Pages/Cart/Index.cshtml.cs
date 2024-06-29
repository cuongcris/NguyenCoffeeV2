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
        public string OrderId { get; set; }
        public string OrderOnlineId { get; set; }
        public IList<CardItem>? cart { get; set; } = new List<CardItem>();

        public IActionResult OnGet()
        {
            string accJson = HttpContext.Session.GetString("Account");
            
         
            if (accJson == null)
            {
                return  Redirect("/Authentication/Login");

            }
            Account acc = JsonConvert.DeserializeObject<Account>(accJson);
            cart = SessionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "cart");       
            cart = cart?.Where(x => x.UserId.Equals(acc.Id)).ToList();
            OrderId = acc.Id+ "card";
            return  Page();


        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                string accJson = HttpContext.Session.GetString("Account")!;
                Account acc = JsonConvert.DeserializeObject<Account>(accJson)!;
                cart = SessionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "cart");
                cart = cart?.Where(x => x.UserId.Equals(acc.Id)).ToList();
                OrderOnlineId = GenerateUniqueString();
                _context.OrdersOnlines.Add(new OrdersOnline()
                {
                    UserId = acc.Id,
                    CreatedAt = DateTime.Now,
                    Id = OrderOnlineId,
                    ShippedDate = DateTime.Now.AddDays(5),
                    PackagingDate = DateTime.Now.AddDays(5),
                    Status = "Shipping"
                });
                await _context.SaveChangesAsync();
                foreach (var item in cart!)
                {
                    var newOrder = new OrderDetail()
                    {
                        ProductId = item.Product.Id,
                        OrderId = OrderOnlineId,
                        UnitPrice = (float)item.Product.UnitPrice!,
                        Quanlity = item.Quantity
                    };
                    _context.OrderDetails.Add(newOrder);
                    await _context.SaveChangesAsync();
                }
                HttpContext.Session.Remove("cart");
                cart.Clear();
                ViewData["msg"] = "Payment Successfully!!!!!";
                return Page();
            }
            catch {
                return RedirectToPage("/Authentication/Login");

            }
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
