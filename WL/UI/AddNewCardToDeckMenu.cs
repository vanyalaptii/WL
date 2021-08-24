using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Context;
using WL.Model;
using WL.Operations;

namespace WL.UI
{
    public class AddNewCardToDeckMenu
    {
        List<Option> addNewCardToDeckMenuOptions = new List<Option>();

        public AddNewCardToDeckMenu() {}

        public void Run(Deck deck)
        {
            using (var Context = new WLContext())
            {
                addNewCardToDeckMenuOptions.Add(new Option(" Back <--\n", () => new DeckEditMenu(deck).Run()));

                var deckInContext = Context.Decks.FirstOrDefault(d => d == deck);

                var nullCheck = Context.Cards.FirstOrDefault();

                if (nullCheck == null)
                {
                    WriteTemporaryMessage("Cards list is empty");
                    new DeckEditMenu(deck).Run();
                }

                var allCards = Context.Cards.ToList();

                foreach (var c in allCards)
                {
                    addNewCardToDeckMenuOptions.Add(new Option(c.FrontSide, () => new CardOperations().AddCardToDeck(c, deck)));
                }
            }

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(addNewCardToDeckMenuOptions, addNewCardToDeckMenuOptions[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < addNewCardToDeckMenuOptions.Count)
                    {
                        index++;
                        WriteMenu(addNewCardToDeckMenuOptions, addNewCardToDeckMenuOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(addNewCardToDeckMenuOptions, addNewCardToDeckMenuOptions[index]);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    addNewCardToDeckMenuOptions[index].Selected.Invoke();
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
            WriteMenu(addNewCardToDeckMenuOptions, addNewCardToDeckMenuOptions.First());
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("Cards\n");

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
