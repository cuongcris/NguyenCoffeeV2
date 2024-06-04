using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NguyenCoffeeWeb.AiGenerator;
using NguyenCoffeeWeb.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NguyenCoffeeWeb.Pages.AiHandleDemo
{
    public class IndexModel : PageModel
    {
        public string[] ImagePaths { get; set; }

        public ActionResult OnGet()
        {
            ImagePaths = new string[4] { "img/DemoImage/demo1.jpg", "img/DemoImage/demo2.jpg", "img/DemoImage/demo3.jpg", "img/DemoImage/demo4.jpg" };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img\\customerUpload");
                Directory.CreateDirectory(uploadsFolder);
                
                using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                {
                    //start - nếu size ảnh > 1024px thì giảm pixel ảnh
                    int overfedWidth = image.Width - 1024, overfedHeight = image.Height - 1024;
                    if (overfedWidth > 0 || overfedHeight > 0)
                    {
                        float rate = (overfedWidth > overfedHeight) ? image.Width / 1024f : image.Width / 1024f;
                        image.Mutate(x => x.Resize((int)(image.Width / rate), (int)(image.Height / rate)));
                    }
                    //end

                    var accJson = HttpContext.Session.GetString("Account"); 
                    var acc = accJson != null ? JsonConvert.DeserializeObject<Account>(accJson) : null;
                    var userId = Guid.Empty;
                    if (acc != null) {
                        userId = acc.Id;
                    }
                    else
                    {
                        userId = Guid.NewGuid();
                        HttpContext.Session.SetString("userId",userId.ToString());
                    }
                    var uniqueFileName = userId + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    await image.SaveAsJpegAsync(filePath);
                    //ImagePaths = await GetAiImage.GenAndWriteImgsId(filePath, userId);

                    ImagePaths = GetAiImage.GenAndWriteImgs(filePath,userId);
                }
            }
            return Page();
        }

    }
}