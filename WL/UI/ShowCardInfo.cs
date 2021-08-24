using System;
using WL.Model;
using ConsoleTables;
using System.Collections.Generic;
using WL.Operations;
using System.Threading;
using System.Linq;
using WL.Context;
using Microsoft.EntityFrameworkCore;

namespace WL.UI
{
    public class ShowCardInfo
    {
        public Card card;

        public Deck deck;

        public ConsoleTable Table;

        public List<Option> showCardInfoOptions = new List<Option>();

        public ShowCardInfo(Card _card)
        {
            card = _card;
        }

        public ShowCardInfo(Card _card, Deck _deck)
        {
            card = _card;
            deck = _deck;
        }

        public void Run()
        {
            using (var Context = new WLContext())
            { 
                showCardInfoOptions.Add(new Option(" Back <--", () => new ShowAllCardsInDeck().Run(deck)));
                showCardInfoOptions.Add(new Option(" Mark(unmark) card as memorized\n", () => new CardOperations().MarcUnmarkCardAsMemorized(card)));

                //var thisCard = Context.Cards.FirstOrDefault(c => c == card).;

                var cat = Context.Categories
                    .Include(c => c.Cards)
                    .ToList();

                Category thisCategory = new Category();

                //TODO: Find out why not loading category

                foreach(var c in cat)
                {
                    foreach(var crd in c.Cards)
                    {
                        if (crd == card)
                        {
                            thisCategory = crd.Category;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                Table = new ConsoleTable("Front", "Back", "Category", "Memorized");
                Table.AddRow(card.FrontSide, card.BackSide, thisCategory.Name, card.IsMemorised);

                Console.Clear();
            }

    


            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(showCardInfoOptions, showCardInfoOptions[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < showCardInfoOptions.Count)
                    {
                        index++;
                        WriteMenu(showCardInfoOptions, showCardInfoOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(showCardInfoOptions, showCardInfoOptions[index]);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    showCardInfoOptions[index].Selected.Invoke();
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
            WriteMenu(showCardInfoOptions, showCardInfoOptions.First());
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine($"Card \"{card.FrontSide}\" information\n");

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

            Table.Write();
        }
    }
}
