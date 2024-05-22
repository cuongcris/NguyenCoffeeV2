using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;
using System.Text.Json;

namespace NguyenCoffeeWeb.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly postgresContext dbContext;
        public LoginModel(postgresContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public Account Account { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            Account? acc = await dbContext.Accounts
                .SingleOrDefaultAsync(a => a.Email.Equals(Account.Email) && a.Password.Equals(Account.Password));
            if (acc == null)
            {
                ViewData["msg"] = "Email or Password is invalid";
                return Page();
            }
            HttpContext.Session.SetString("Account", JsonSerializer.Serialize(acc));
            HttpContext.Session.SetString("Type", acc.Type.ToString());

            if (RememberMe)
            {
                int cookieExpirationDays = 7;
                DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays);
                Response.Cookies.Append("EmailInCookie", acc.Email, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = expirationTime
                });

                Response.Cookies.Append("PassInCookie", acc.Password, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = expirationTime
                });
            }

            if (acc.Type == 0) return Redirect("/Admin/Products");
            else return Redirect("/Index");
        }
    }
}
