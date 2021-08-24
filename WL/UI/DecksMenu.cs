using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Context;
using WL.Model;

namespace WL.UI
{
    public class DecksMenu
    {
        public List<Option> decksMenuOptions = new List<Option>();

        public DecksMenu() { }

        public void Run()
        {
            Console.Clear();

            using (var Context = new WLContext())
            {
                decksMenuOptions.Add(new Option(" Back <--", () => new MainMenu().Run()));
                decksMenuOptions.Add(new Option(" Add new deck\n", () => new AddNewDeckMenu().Run()));

                //var mainMenu = new MainMenu();

                var decks = new List<Deck>();

            
                decks = Context.Decks.ToList();

                foreach (var d in decks)
                {
                    decksMenuOptions.Add(new Option(d.Name, () => new DeckEditMenu(d).Run()));
                }

                // Set the default index of the selected item to be the first
                int index = 0;

                // Write the menu out
                WriteMenu(decksMenuOptions, decksMenuOptions[index]);

                // Store key info in here
                ConsoleKeyInfo keyinfo;
                do
                {
                    keyinfo = Console.ReadKey();

                    // Handle each key input (down arrow will write the menu again with a different selected item)
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        if (index + 1 < decksMenuOptions.Count)
                        {
                            index++;
                            WriteMenu(decksMenuOptions, decksMenuOptions[index]);
                        }
                    }

                    if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        if (index - 1 >= 0)
                        {
                            index--;
                            WriteMenu(decksMenuOptions, decksMenuOptions[index]);
                        }
                    }

                    // Handle different action for the option
                    if (keyinfo.Key == ConsoleKey.Enter)
                    {
                        decksMenuOptions[index].Selected.Invoke();
                        index = 0;
                    }
                }
                while (keyinfo.Key != ConsoleKey.X);

                Console.ReadKey();

            }

        }

        // Default action of all the options.  
        private void WriteTemporaryMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
            WriteMenu(decksMenuOptions, decksMenuOptions.First());
        }

        private void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("Decks Menu\n");

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
