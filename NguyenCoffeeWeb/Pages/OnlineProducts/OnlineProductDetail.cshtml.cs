using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NguyenCoffeeWeb.Helpers;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.OnlineProducts
{
	public class OnlineProductDetailModel : PageModel
	{
		private readonly NguyenCoffeeWeb.Models.postgresContext _context;


		public List<Item> cart;
		public OnlineProductDetailModel(NguyenCoffeeWeb.Models.postgresContext context)
		{
			_context = context;
		}

		public Product Product { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(Guid? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			else
			{
				Product = product;
			}
			return Page();
		}

		public IActionResult OnPost()
		{
			var productId = Request.Form["productId"];

			var product = _context.Products.FirstOrDefault(m => m.Id.ToString().Equals(productId));
			string accJson = HttpContext.Session.GetString("Account");
			if ((accJson == null))
			{
				return Redirect("/Authentication/Login");
			}
			Account acc = JsonConvert.DeserializeObject<Account>(accJson);
			cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
			if (cart == null)
			{
				cart = new List<Item>();
				cart.Add(new Item()
				{
					Product = product,
					Quantity = 1,
					UserId = acc.Id
				});
				SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			}
			else
			{
				var index = Exists(cart, product.Id);
				if (index == -1)
				{
					cart.Add(new Item()
					{
						Product = product,
						Quantity = 1,
						UserId = acc.Id
					});
				}
				else
				{
					var newQuantity = cart[index].Quantity + 1;
					cart[index].Quantity = newQuantity;
				}
				SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			}
			return Redirect("/Cart");
		}
		private int Exists(List<Item> cart, Guid id)
		{
			for (int i = 0; i < cart.Count; i++)
			{

				if (cart[i].Product.Id.Equals(id))
				{
					return i;
				}
			}
			return -1;
		}

	}
}
