using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;
using PagedList;

namespace NguyenCoffeeWeb.Pages.Admin.Dashboard
{
    public class AccountModel : PageModel
    {
		private readonly postgresContext _context;

		public AccountModel(postgresContext context)
		{
			_context = context;
		}

		public IPagedList<Account> Account { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }
		public void OnGet(int? pageNumber)
		{
			if (_context.Accounts != null)
			{
				var page = pageNumber ?? 1;
				var pageSize = 4;

				var accounts = from p in _context.Accounts
							   select p;

				if (!string.IsNullOrEmpty(SearchString))
				{
					accounts = accounts.Where(p => p.FullName !=null? p.FullName.ToLower().Contains(SearchString.ToLower()): p.Email.ToLower().Contains(SearchString.ToLower()) || p.Email.ToLower().Contains(SearchString.ToLower()));
				}

				Account = accounts.OrderBy(p => p.FullName).ToPagedList(page, pageSize);
			}
		}
	}
}
