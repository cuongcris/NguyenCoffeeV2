using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenCoffeeWeb.Models;

namespace NguyenCoffeeWeb.Pages.AiHandle
{
    public class IndexModel : PageModel
    {
        private readonly postgresContext _context;
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
            return new JsonResult(imagesList);
        }
    }
}
