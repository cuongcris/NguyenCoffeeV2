using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;

namespace NguyenCoffeeWeb.Pages.AiHandleDemo
{
    public class TokenModel : PageModel
    {
        public string? Token {  get; set; }
        public IActionResult OnGet(string token)
        {
            Token = token;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile imageFile, string token)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img\\customerUpload");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = token + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await image.SaveAsJpegAsync(filePath);
                }
            }
            return Page();
        }
    }
}
