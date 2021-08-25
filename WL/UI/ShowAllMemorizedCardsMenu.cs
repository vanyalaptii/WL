using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Context;

namespace WL.UI
{
    public class ShowAllMemorizedCardsMenu
    {
        public ShowAllMemorizedCardsMenu() {}

        public static List<Option> showAllMemorizedCardsMenuOptions;

        public void Run()
        {
            Console.Clear();

            using (var Context = new WLContext())
            {

                showAllMemorizedCardsMenuOptions = new List<Option>
                {
                    new Option(" Back <--\n", () => new MainMenu().Run()),
                };

                var nullCheck = Context.Cards.FirstOrDefault();

                if (nullCheck == null)
                {
                    WriteTemporaryMessage("Cards list is empty");
                    new MainMenu().Run();
                }

                var allCards = Context.Cards.Where(c => c.IsMemorised == true).ToList();

                foreach (var c in allCards)
                {
                    showAllMemorizedCardsMenuOptions.Add(new Option(c.FrontSide, () => new ShowCardInfo(c).Run()));
                }

            }

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(showAllMemorizedCardsMenuOptions, showAllMemorizedCardsMenuOptions[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < showAllMemorizedCardsMenuOptions.Count)
                    {
                        index++;
                        WriteMenu(showAllMemorizedCardsMenuOptions, showAllMemorizedCardsMenuOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(showAllMemorizedCardsMenuOptions, showAllMemorizedCardsMenuOptions[index]);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    showAllMemorizedCardsMenuOptions[index].Selected.Invoke();
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
            WriteMenu(showAllMemorizedCardsMenuOptions, showAllMemorizedCardsMenuOptions.First());
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("All memorized cards\n");

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
