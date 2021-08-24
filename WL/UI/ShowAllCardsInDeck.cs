using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using WL.Context;
using WL.Model;

namespace WL.UI
{
    public class ShowAllCardsInDeck
    {
        public ShowAllCardsInDeck() { }

        public List<Option> showAllCardsInDeckOptions = new List<Option>();

        public Deck deck;

        public void Run(Deck _deck)
        {
            using (var Context = new WLContext())
            {
                deck = _deck;

                showAllCardsInDeckOptions.Add(new Option(" Back <--\n", () => new DeckEditMenu(deck).Run()));

                var allCards = Context.Cards.Include(c => c.Decks).ToList();
                    //.Where(c => c.Decks);

                foreach (var card in allCards)
                {
                    foreach( var d in card.Decks)
                    {
                        if (d.Deck == null) break;
                        if (d.Deck == deck)
                            showAllCardsInDeckOptions.Add(new Option($"{card.FrontSide}", () => new ShowCardMenu().Run(card)));
                    }
                }
            }

                // Set the default index of the selected item to be the first
                int index = 0;

                // Write the menu out
                WriteMenu(showAllCardsInDeckOptions, showAllCardsInDeckOptions[index]);

                // Store key info in here
                ConsoleKeyInfo keyinfo;
                do
                {
                    keyinfo = Console.ReadKey();

                    // Handle each key input (down arrow will write the menu again with a different selected item)
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        if (index + 1 < showAllCardsInDeckOptions.Count)
                        {
                            index++;
                            WriteMenu(showAllCardsInDeckOptions, showAllCardsInDeckOptions[index]);
                        }
                    }

                    if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        if (index - 1 >= 0)
                        {
                            index--;
                            WriteMenu(showAllCardsInDeckOptions, showAllCardsInDeckOptions[index]);
                        }
                    }

                    // Handle different action for the option
                    if (keyinfo.Key == ConsoleKey.Enter)
                    {
                        showAllCardsInDeckOptions[index].Selected.Invoke();
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
            WriteMenu(showAllCardsInDeckOptions, showAllCardsInDeckOptions.First());
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("Main Menu\n");

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