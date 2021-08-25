using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using WL.Context;
using WL.Model;
using WL.Operations;

namespace WL.UI
{
    public class ShowCardInfoWithOptionsMenu
    {
        public ShowCardInfoWithOptionsMenu()
        {
        }

        public Card card;

        public Deck deck;

        public bool isHidden = true;

        public ConsoleTable Table;

        public List<Option> showCardInfoOptions = new List<Option>();

        public ShowCardInfoWithOptionsMenu(Card _card)
        {
            card = _card;
        }

        public ShowCardInfoWithOptionsMenu(Card _card, Deck _deck)
        {
            card = _card;
            deck = _deck;           
        }

        public void Run()
        {
            using (var Context = new WLContext())
            {
                showCardInfoOptions.Add(new Option(" Back <--", () => new LearningMenu().Run()));


                showCardInfoOptions.Add(new Option(" Mark(unmark) card as memorized\n", () => new CardOperations().MarcUnmarkCardAsMemorized(card)));

                var Cards = Context.Cards
                    .Include(c => c.Category)
                    .ToList();

                var thisCard = new Card();

                foreach (var c in Cards)
                {
                    if (c.Id == card.Id)
                    {
                        thisCard = c;
                        break;
                    }
                }

                var thisCardCategoryName = "";

                if(!isHidden) 
                    thisCardCategoryName = thisCard.Category.Name;

                Table = new ConsoleTable("Front", "Back", "Category", "Memorized");
                Table.AddRow(card.FrontSide, card.BackSide, thisCard.Category.Name, card.IsMemorised);
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
