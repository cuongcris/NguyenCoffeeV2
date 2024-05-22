using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.Authentication
{
    public class RegisterModel : PageModel
    {
        readonly postgresContext dbContext;
        public RegisterModel(postgresContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public Account Account { get; set; }
        [BindProperty]
        public string RePassword { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            List<Account> accounts = dbContext.Accounts.ToList();
            Account acc = accounts.FirstOrDefault(a => a.Email == Account.Email);

            if (acc != null)
            {
                ViewData["msg"] = "Email already exists";
                return Page();
            }

            if (!RePassword.Equals(Account.Password))
            {
                ViewData["msg"] = "Your confirm password is wrong";
                return Page();
            }

            Account.Type = 1;
            Account.CreatedAt = DateTime.Now;
            dbContext.Accounts.Add(Account);
            await dbContext.SaveChangesAsync();
            return Redirect("~/Authentication/Login");
        }
    }
}
