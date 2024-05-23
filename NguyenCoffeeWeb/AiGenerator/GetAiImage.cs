using System.Text;

namespace NguyenCoffeeWeb.AiGenerator
{
    public class GetAiImage
    {
        static string[] models = { "dark-sushi-mix-v2-25", "anashel-rpg", "neverending-dream", "eimis-anime-diffusion-v1-0" };
        static string prompt = "Add a signature for the name 'Bach Duong'";
        static string controlnet = "lineart-1.1";
        static int seed = 0;
        static int guidance = 20;
        static int width = 512, height = 512;
        static string scheduler = "ddim";
        static string output_format = "";
        private static string Img2ImgApiUrl = "https://api.getimg.ai/v1/stable-diffusion/image-to-image";
        private static string controlnetApiUrl = "https://api.getimg.ai/v1/stable-diffusion/controlnet";
        public static string[] GenAndWriteImgs(string inputImage)
        {
            output_format = inputImage.Contains(".png") ? "png" : "jpg";
            string[] AiImageUrls = new string[4];
            var tasks = new List<int> { 0, 1, 2, 3 };
            Parallel.ForEach(tasks, async number =>
            {
                AiImageUrls[number] = await img2img(models[number], inputImage);
            });
            return AiImageUrls;
        }
        public static async Task<string> img2img(string model, string inputImage)
        {
            HttpClient client = client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer key-140Z9UMhIiriSfi1QY729IVM4Yr5hUqnh02KJO3Rde1e7lsQv4l8jrmM7LZWZjmfImQjwSGEi5ndyew2pLCtHDFhpQU4x50K");
            // key-vDReGhvtskhFt7Rk68iWMISbqcAhqeLCXuniXo4DSS7z1loUFu47bCNekGosvPtRgZ2XWxsReli9dWcC2LN4jvfrC1nHd1w
            string base64Img = Convert.ToBase64String(File.ReadAllBytes(inputImage));
            string jsonBody = JsonBodyControlnet(model, base64Img);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(controlnetApiUrl, content).Result;
            string strData = response.Content.ReadAsStringAsync().Result;

            var AiImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img\\AiImage");
            Directory.CreateDirectory(AiImageFolder);
            string filePath = $"{AiImageFolder}/{Guid.NewGuid()}_{ model }.{ output_format }";
            File.WriteAllBytes(filePath, Convert.FromBase64String(strData.Split("\"")[3]));
            return "img\\AiImage\\" + Path.GetFileName(filePath);
        }
        static string JsonBodyImg2Img(string model, string base64Img)
        {
            return "{\"model\":\"" + model + "\",\"prompt\":\"" + prompt + "\",\"negative_prompt\":\"Disfigured, blurry\",\"image\":\"" + base64Img + "\",\"strength\":0.5,\"steps\":25,\"guidance\":" + guidance + ",\"seed\":" + seed + ",\"scheduler\":\"" + scheduler + "\",\"output_format\":\"" + ((output_format == "jpg") ? "jpeg" : output_format) + "\"}";
        }
        static string JsonBodyControlnet(string model, string base64Img)
        {
            return "{\"model\":\"" + model + "\",\"controlnet\":\"" + controlnet + "\",\"prompt\":\"" + prompt + "\",\"negative_prompt\":\"Disfigured, blurry\",\"image\":\"" + base64Img + "\",\"strength\":1.5,\"width\":" + width + ",\"height\":" + height + ",\"steps\":25,\"guidance\":" + guidance + ",\"seed\":" + seed + ",\"scheduler\":\"" + scheduler + "\",\"output_format\":\"" + ((output_format == "jpg") ? "jpeg" : output_format) + "\"}";
        }
    }
}
