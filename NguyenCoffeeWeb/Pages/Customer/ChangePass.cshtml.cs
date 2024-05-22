using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;
using System.Text.Json;

namespace NguyenCoffeeWeb.Pages.Authentication
{
    public class ChangePassModel : PageModel
    {
        readonly postgresContext dbContext;
        public ChangePassModel(postgresContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public string CurPassword { get; set; }

        [BindProperty]
        public string Newpassword { get; set; }

        [BindProperty]
        public string RePassword { get; set; }

        Account curAccount = new Account();
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            string accountSession = HttpContext.Session.GetString("Account")!;
            curAccount = JsonSerializer.Deserialize<Account>(accountSession)!;
            if (ModelState.IsValid)
            {
                if (!curAccount.Password.Equals(CurPassword))
                {
                    ViewData["msg"] = "Wrong current password";
                    return Page();
                }

                if (!RePassword.Equals(Newpassword))
                {
                    ViewData["msg"] = "Your confirm password is wrong";
                    return Page();
                }

                if (Newpassword.Equals(CurPassword))
                {
                    ViewData["msg"] = "New password must different with current password";
                    return Page();
                }

                curAccount.Password = Newpassword;
                dbContext.Accounts.Update(curAccount);
                await dbContext.SaveChangesAsync();

                HttpContext.Session.Remove("Account");
                HttpContext.Session.SetString("Account", JsonSerializer.Serialize(curAccount));


                bool cookieExists = Request.Cookies.ContainsKey("EmailInCookie");

                if (cookieExists)
                {
                    Response.Cookies.Delete("EmailInCookie");
                    Response.Cookies.Delete("PassInCookie");
                    int cookieExpirationDays = 7;
                    DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays);
                    Response.Cookies.Append("EmailInCookie", curAccount.Email, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = expirationTime
                    });

                    Response.Cookies.Append("PassInCookie", Newpassword, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = expirationTime
                    });
                }

                ViewData["msgOk"] = "Change Password successfully";
            }

            return Redirect("/Index");
        }
    }
}
