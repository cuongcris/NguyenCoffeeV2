using Newtonsoft.Json;
using NguyenCoffeeWeb.Models;
using System.Text;

namespace NguyenCoffeeWeb.AiGenerator
{
    public class GetAiImage
    {
        static readonly string[] models = { "dark-sushi-mix-v2-25", "anashel-rpg", "neverending-dream", "eimis-anime-diffusion-v1-0" };
        static readonly string prompt = "Add a signature for the name 'Bach Duong'";
        static readonly string controlnet = "lineart-1.1";
        static readonly int seed = 0;
        static readonly int guidance = 20;
        static readonly int width = 512, height = 512;
        static readonly string scheduler = "ddim";
        static string output_format = "";
       // private static string Img2ImgApiUrl = "https://api.getimg.ai/v1/stable-diffusion/image-to-image";
        private static readonly string controlnetApiUrl = "https://api.getimg.ai/v1/stable-diffusion/controlnet";
        public static string[] GenAndWriteImgs(string inputImage,Guid userId)
        {
            output_format = inputImage.Contains(".png") ? "png" : "jpg";
            string[] AiImageUrls = new string[4];
            var tasks = new List<int> { 0, 1, 2, 3 };
            Parallel.ForEach(tasks, async number =>
            {
                AiImageUrls[number] = await ImgToImg(models[number], inputImage,userId);
            });
            return AiImageUrls;
        }
       

        public static async Task<string> ImgToImg(string model, string inputImage,Guid userId)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer key-43PKW9uZJhBk50iLfF22mE80O89zhykiOO3M5jQvCgDFOxecPX0ZnvW1itMrL8itY45LresdX9bPuCutNnGnHgzUnirV41Wb");
            // key-vDReGhvtskhFt7Rk68iWMISbqcAhqeLCXuniXo4DSS7z1loUFu47bCNekGosvPtRgZ2XWxsReli9dWcC2LN4jvfrC1nHd1w
            string base64Img = Convert.ToBase64String(File.ReadAllBytes(inputImage));
            string jsonBody = JsonBodyControlnet(model, base64Img);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(controlnetApiUrl, content).Result;
            string strData =  response.Content.ReadAsStringAsync().Result;
            ImageResponse imageResponse = JsonConvert.DeserializeObject<ImageResponse>(strData);

            var AiImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img\\AiImage\\"+ userId);
            Directory.CreateDirectory(AiImageFolder);
            string filePath = $"{AiImageFolder}/{Guid.NewGuid()}_{ model }.{ output_format }";
            File.WriteAllBytes(filePath, Convert.FromBase64String(imageResponse.Image));
            return "img\\AiImage\\"+ userId+"\\" + Path.GetFileName(filePath);
        }
        static string JsonBodyControlnet(string model, string base64Img)
        {
            return "{\"model\":\"" + model + "\",\"controlnet\":\"" + controlnet + "\",\"prompt\":\"" + prompt + "\",\"negative_prompt\":\"Disfigured, blurry\",\"image\":\"" + base64Img + "\",\"strength\":1.5,\"width\":" + width + ",\"height\":" + height + ",\"steps\":25,\"guidance\":" + guidance + ",\"seed\":" + seed + ",\"scheduler\":\"" + scheduler + "\",\"output_format\":\"" + ((output_format == "jpg") ? "jpeg" : output_format) + "\"}";
        }
       
    }
}
