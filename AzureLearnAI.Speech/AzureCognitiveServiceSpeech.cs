using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AzureLearnAI.Speech;

public static class AzureCognitiveServiceSpeech
{
    // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
    static string? speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
    static string? speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

    static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
    {
        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                Console.WriteLine($"Speech synthesized for text: [{text}]");
                break;
            case ResultReason.Canceled:
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
            default:
                break;
        }
    }

    public static async Task TextToSpeech()
    {
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

        speechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

        using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
        {
            Console.WriteLine("Enter some text that you want to speak >");
            string? text = Console.ReadLine();

            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
            OutputSpeechSynthesisResult(speechSynthesisResult, text!);
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public static async Task FromMic()
    {
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);


        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var result = await speechRecognizer.RecognizeOnceAsync();
        Console.WriteLine($"RECOGNIZED: Text={result.Text}");
    }
}
