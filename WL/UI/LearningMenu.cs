using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Context;

namespace WL.UI
{
    public class LearningMenu
    {
        public static List<Option> learningMenuOptions;

        public LearningMenu() {}

        public void Run()
        {
            Console.Clear();

            learningMenuOptions = new List<Option>
            {
                new Option(" Back <--\n", () => new MainMenu().Run()),
                new Option(" Start lerning deck", () => new LearningDeckMenu().Run()),
                new Option(" Start lerning all cards", () => new LearningProcessMenu().Run()),
            };

            //using (var Context = new WLContext())
            //{

            //}

            // Set the default index of the selected item to be the first
            int index = 1;

            // Write the menu out
            WriteMenu(learningMenuOptions, learningMenuOptions[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < learningMenuOptions.Count)
                    {
                        index++;
                        WriteMenu(learningMenuOptions, learningMenuOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(learningMenuOptions, learningMenuOptions[index]);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    learningMenuOptions[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.ReadKey();

        }

        // Default action of all the options.  
        public void WriteTemporaryMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
            WriteMenu(learningMenuOptions, learningMenuOptions.First());
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("Learning Menu\n");

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }
    }
}
