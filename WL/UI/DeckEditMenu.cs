using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Model;
using WL.Operations;
using WL.UI;

namespace WL.UI
{
    public class DeckEditMenu
    {
        public List<Option> decksEditMenuOptions = new List<Option>();

        private Deck deck;

        public DeckEditMenu(Deck _deck)
        {
            deck = _deck;
        }

        public void Run()
        {
            Console.Clear();

            decksEditMenuOptions.Add(new Option(" Back <--", () => new DecksMenu().Run()));
            decksEditMenuOptions.Add(new Option(" Delete this deck", () => new DeckOperations().RemoveDeck(deck)));
            decksEditMenuOptions.Add(new Option(" Show all cards in this deck", () => new ShowAllCardsInDeck().Run(deck)));
            decksEditMenuOptions.Add(new Option(" Add new card to deck\n", () => new AddNewCardToDeckMenu().Run(deck)));



            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(decksEditMenuOptions, decksEditMenuOptions[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < decksEditMenuOptions.Count)
                    {
                        index++;
                        WriteMenu(decksEditMenuOptions, decksEditMenuOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(decksEditMenuOptions, decksEditMenuOptions[index]);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    decksEditMenuOptions[index].Selected.Invoke();
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
            WriteMenu(decksEditMenuOptions, decksEditMenuOptions.First());
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine($"Deck Menu ({deck.Name})\n");

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