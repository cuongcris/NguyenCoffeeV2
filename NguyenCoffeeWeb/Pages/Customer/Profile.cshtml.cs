using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb.Models;
using System.Text.Json;

namespace NguyenCoffeeWeb.Pages.Authentication
{
    public class ProfileModel : PageModel
    {
        readonly postgresContext dbContext;
        public ProfileModel(postgresContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public Account Account { get; set; }

        public void OnGet()
        {
            string accountSession = HttpContext.Session.GetString("Account")!;
            var curAccount = JsonSerializer.Deserialize<Account>(accountSession)!;
            Account = curAccount;
        }
        public async Task<IActionResult> OnPost()
        {
            string accountSession = HttpContext.Session.GetString("Account")!;
            var curAccount = JsonSerializer.Deserialize<Account>(accountSession)!;

            Account.CreatedAt = curAccount.CreatedAt;
            Account.Id = curAccount.Id;
            Account.Email = curAccount.Email;
            Account.Password = curAccount.Password;
            Account.Type = curAccount.Type;

            dbContext.Attach(Account).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            HttpContext.Session.Remove("Account");
            HttpContext.Session.SetString("Account", JsonSerializer.Serialize(Account));

            ViewData["msgOk"] = "Change Profile successfully";

            return Page();
        }
    }
}
