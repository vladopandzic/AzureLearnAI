using AzureLearnAI.Speech;

class Program
{
    async static Task Main(string[] args)
    {
        //await AzureCognitiveServiceSpeech.TextToSpeech();

        await AzureCognitiveServiceSpeech.FromMic();
    }
}