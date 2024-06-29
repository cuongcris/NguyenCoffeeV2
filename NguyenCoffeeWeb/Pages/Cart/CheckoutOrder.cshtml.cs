using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NguyenCoffeeWeb.Models;
using NguyenCoffeeWeb.Models.Dtos;

namespace NguyenCoffeeWeb.Pages.Cart
{
    public class CheckoutOrderModel : PageModel
    {
        private readonly postgresContext _postgresContext;
        public CheckoutOrderModel(postgresContext postgresContext)
        {
            _postgresContext = postgresContext;
        }
        [BindProperty]
        public OrderBillDto OrderBill { get; set; }

        private void InitializeOrderBill(string? id)
        {
            string coffeeOrderJson = HttpContext.Session.GetString("CoffeeOrderList");
            if(id == null)
            {
                var userId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("userId", userId);
            }

            if (coffeeOrderJson != "[]")
            {
                var coffeeOrders = JsonConvert.DeserializeObject<List<CoffeeOrderDto>>(coffeeOrderJson);
                OrderBill = new OrderBillDto
                {
                    CoffeeOrderList = coffeeOrders,
                    ImagePath = id == null ? null : "\\" + id
                };
            }
            else
            {
                ViewData["OrderSuccess"] = "";
                ViewData["EmptyOrder"] = "Không có đơn hàng nào!";
                OrderBill = null;
            }
        }

        public async Task OnGet(string? id)
        {
            InitializeOrderBill(id);
        }
        public async Task<IActionResult> OnPost(string id)
        {
            InitializeOrderBill(id);
            var userId = HttpContext.Session.GetString("userId");
            if(userId == null)
            {
                userId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("userId", userId);
            }
            // Check if the JSON string exists
            if (OrderBill != null)
            {
                var order = new OrderInTable()
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    ImageAi = OrderBill.ImagePath,
                    TableNumber = 1,
                    UserName = "User",
                    UserId = Guid.Parse(userId),
                    IsPay = false
                };
                await  _postgresContext.OrderInTables.AddAsync(order);
                await _postgresContext.SaveChangesAsync();
            }
            HttpContext.Session.Remove("CoffeeOrderList");
            HttpContext.Session.Remove("userId");
            ViewData["EmptyOrder"] = "";
            OrderBill = null;
            ViewData["OrderSuccess"] = "Đã gửi order thành công, nhân viên sẽ kiểm tra hãy đợi trong giây lát!";
            return Page();

        }
    }
}
