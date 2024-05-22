using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenCoffeeWeb.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("Account");
            HttpContext.Session.Remove("Type");
            Response.Cookies.Delete("EmailInCookie");
            Response.Cookies.Delete("PassInCookie");

            return RedirectToPage("/Index");
        }
    }
}
