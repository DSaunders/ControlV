using System;
using System.Dynamic;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TextCopy;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ControlV.Actions
{
    public class JsonFormat : IAction
    {
        public string DisplayName => "Format JSON string";
        public string ResultText => "Your JSON string has been formatted";

        public Task<bool> CanRun(string clipboardContent)
        {
            var isJson = IsValidJson(clipboardContent);
            if (!isJson)
                return Task.FromResult(false);

            Console.WriteLine("Found a JSON string");
            if (clipboardContent.Contains(Environment.NewLine))
            {
                Console.WriteLine("String is already formatted");
                return Task.FromResult(false);
            }

            Console.WriteLine("String is not yet formatted");
            return Task.FromResult(true);
        }

        public async Task Run(string clipboardContent)
        {
            var parsedJson = JsonSerializer.Deserialize<ExpandoObject>(clipboardContent);
            var options = new JsonSerializerOptions() { WriteIndented = true };
            var formatted = JsonSerializer.Serialize(parsedJson, options);
            Console.WriteLine("Replacing clipboard contents with formatted JSON");
            await ClipboardService.SetTextAsync(formatted);
        }

        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
