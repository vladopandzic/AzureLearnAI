using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ComputerVisionQuickstart
{
    class Program
    {


        static string? subscriptionKey = Environment.GetEnvironmentVariable("VISION_KEY");
        static string? endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");

        private const string ANALYZE_LOCAL_IMAGE = "celebrities.jpg";

        private const string ANALYZE_URL_IMAGE = "https://img.adriagate.com/cdn/new/photos/2175831-15/house-lara-vrboska-island-hvar-croatia-11851_0_550.jpg";

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

 
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
       

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
   
        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();


            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Color
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
          
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

         
            Console.WriteLine("Summary:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            Console.WriteLine();
        
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
        }

        public static async Task AnalyzeImageLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - LOCAL IMAGE");
            Console.WriteLine();

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Tags
            };

            Console.WriteLine($"Analyzing the local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();

            using (Stream analyzeImageStream = File.OpenRead(localImage))
            {
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(analyzeImageStream, visualFeatures: features);

                if (null != results.Description && null != results.Description.Captions)
                {
                    Console.WriteLine("Summary:");
                    foreach (var caption in results.Description.Captions)
                    {
                        Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                    }
                    Console.WriteLine();
                }

                if (null != results.Tags)
                {
                    Console.WriteLine("Tags:");
                    foreach (var tag in results.Tags)
                    {
                        Console.WriteLine($"{tag.Name} {tag.Confidence}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}