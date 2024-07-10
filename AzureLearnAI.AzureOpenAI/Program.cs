using Azure.AI.OpenAI;
using OpenAI.Chat;

class Program
{
    async static Task Main(string[] args)
    {

        var openAiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");

        AzureOpenAIClient client = new AzureOpenAIClient(new Uri("https://azure-open-ai-vl.openai.azure.com/"), new Azure.AzureKeyCredential(openAiKey));


        var chatClient = client.GetChatClient("gpt-35-turbo");

        List<ChatMessage> chatMessages = [];

        string? userPromt = "";

        do
        {
            Console.Write("User:");
            userPromt = Console.ReadLine();

            Console.WriteLine();

            //chatMessages = [];
            
            chatMessages.Add(new UserChatMessage(userPromt));


            var chatResponse = await chatClient.CompleteChatAsync(chatMessages, new ChatCompletionOptions()
            {

                Temperature = (float)0.7,
                MaxTokens = 800,

            });


            var value = chatResponse.Value;

            Console.WriteLine("System:" + value);

            Console.WriteLine();


        } while (!string.IsNullOrEmpty(userPromt));
    }
}