using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.AiGenerator;
using NguyenCoffeeWeb.Models;
using System.IO.MemoryMappedFiles;

namespace NguyenCoffeeWeb.Pages.AiHandle
{
    public class IndexModel : PageModel
    {
        private readonly postgresContext _context;
        private GetAiImage getAiImage;
        public IndexModel(postgresContext postgresContext)
        {
            _context = postgresContext;
        }
        public IList<ImageAi> imagesList { get; set; }

        public void OnGet()
        {
            //imagesList = _context.ImageAis.ToList();
        }

        public IActionResult OnGetLoadImages()
        {
            // Handle POST request to load images
            imagesList = _context.ImageAis.ToList();
            var s = new JsonResult(imagesList);
            return s;
        }
        public async Task<IActionResult> OnPostLoadImages(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors (optional: return appropriate error response)
                return BadRequest(ModelState);
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select an image file to upload.");
            }
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    string fileContent = await reader.ReadToEndAsync();
                }
                // Use IFormFile to efficiently handle uploaded images
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        byte[] imageData = memoryStream.ToArray();
                        string base64Img = Convert.ToBase64String(imageData);
                        var userId = Guid.Parse("958ca7a9-568e-4fd0-81f2-74373e574000"); // Replace with your logic
                        //await getAiImage.imgToimgUpdateDb(file.FileName, base64Img, userId,_context);
                    }
                }
                return OnGetLoadImages();
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully (log errors, return appropriate error response)
                return StatusCode(500, ex.Message);
            }
        }
    }
}
