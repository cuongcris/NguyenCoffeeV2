using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NguyenCoffeeWeb.Models;
using NguyenCoffeeWeb.Models.Dtos;

namespace NguyenCoffeeWeb.Pages.Cart
{
    public class CheckoutOrderModel : PageModel
    {
        public OrderBillDto OrderBill { get; set; }
        public async void OnGet(string? Id)
        {
            string coffeeOrderJson = HttpContext.Session.GetString("CoffeeOrderList");

            // Check if the JSON string exists
            if (coffeeOrderJson != null)
            {
                // Deserialize JSON into List<CoffeeOrder>
                var coffeeOrders = JsonConvert.DeserializeObject<List<CoffeeOrderDto>>(coffeeOrderJson);
                OrderBill = new OrderBillDto()
                { 
                    CoffeeOrderList = coffeeOrders,
                    ImagePath  =  "\\"+Id,
                };
            }
            else
            {
                OrderBill = null;
            }
        }
        public async void OnPost()
        {
            
        }
    }
}
