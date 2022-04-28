using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextCopy;

// TODO: ability to pass a shortcut command to bypass multiple options (e.g. 'cv fmt') to jump to a format option
// TODO: this might mean each action has a 'category' (unit tests should make sure that we don't end up with multiple commands for a given input, e.g. a json string)

namespace ControlV
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var originalColour = Console.ForegroundColor;

            var textColour = ConsoleColor.DarkGray;
            Console.ForegroundColor = textColour;

            Console.WriteLine("Scanning clipboard contents..");

            var text = await ClipboardService.GetTextAsync();
            if (string.IsNullOrEmpty(text))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The clipboard is empty");
                Console.ForegroundColor = originalColour;
                return;
            }

            var actions = new List<IAction>();

            foreach (var action in ActionPipeline.Actions)
            {
                if (await action.CanRun(text))
                {
                    actions.Add(action);
                }
            }

            if (actions.Count == 1)
            {
                Console.WriteLine($"Running action '{actions.First().DisplayName}'");

                await actions.First().Run(text);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine($"    ** {actions.First().ResultText} **");
                Console.WriteLine();

                Console.ForegroundColor = textColour;
                
                Console.WriteLine("Done");
            }
            else if (!actions.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Couldn't find anything to do with the clipboard contents");
                Console.ForegroundColor = textColour;
            }

            Console.ForegroundColor = originalColour;

        }

    }
}