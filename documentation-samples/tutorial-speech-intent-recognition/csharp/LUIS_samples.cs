/*
 *
 * The class calls from Speech service to LUIS endpoint.
 * For more info, check out the documentation:
 * https://aka.ms/luis-intent-recognition-tutorial
 * 
*/

using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Intent;

namespace MicrosoftSpeechSDKSamples
{
    class LuisSamples
    {

        public static async Task RecognitionWithLUIS()
        {
            // Create a LUIS endpoint key in the Azure portal, add the key on 
            // the LUIS publish page, and use again here. Do not use starter key!
            var luisSubscriptionKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            var luisRegion = "westus";
            var luisAppId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            var speechRegion ="";

            // region must be empty string
            // must use same LUIS guid in both places
            var config = SpeechConfig.FromSubscription(luisSubscriptionKey, speechRegion);

            // Create an intent recognizer using microphone as audio input.
            using (var recognizer = new IntentRecognizer(config))
            {

                // Create a LanguageUnderstandingModel to use with the intent recognizer
                var model = LanguageUnderstandingModel.FromAppId(luisAppId);

                // Add intents from your LU model to your intent recognizer
                recognizer.AddIntent(model, "None", "None");
                recognizer.AddIntent(model, "FindForm", "FindForm");
                recognizer.AddIntent(model, "GetEmployeeBenefits", "GetEmployeeBenefits");
                recognizer.AddIntent(model, "GetEmployeeOrgChart", "GetEmployeeOrgChart");
                recognizer.AddIntent(model, "MoveAssetsOrPeople", "MoveAssetsOrPeople");

                // Prompt the user to speak
                Console.WriteLine("Say something...");

                // Start recognition; will return the first result recognized
                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                // Checks result.
                if (result.Reason == ResultReason.RecognizedIntent)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent Id: {result.IntentId}.");
                    Console.WriteLine($"    Language Understanding JSON: {result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult)}.");
                }
                else if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent not recognized.");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
        }
    }
}
