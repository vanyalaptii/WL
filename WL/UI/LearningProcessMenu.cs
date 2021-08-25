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
    public class LearningProcessMenu
    {
        public Deck deck = null;

        public ConsoleTable Table;

        public string backside;

        public Card currentCard;

        public List<Option> LearningProcessMenuOptions = new List<Option>();

        public LearningProcessMenu() {}

        public LearningProcessMenu(Deck _deck)
        {
            deck = _deck;
        }

        public void Run()
        {
            List<Card> cardsToLearn = new List<Card>();

            LearningProcessMenuOptions.Add(new Option(" Back <--", () => new LearningMenu().Run()));
            LearningProcessMenuOptions.Add(new Option(" Show card back side", () => WriteTemporaryMessage(backside)));
            LearningProcessMenuOptions.Add(new Option(" Mark(unmark) as memorized\n", () => new CardOperations().MarcUnmarkCardAsMemorized(currentCard)));
            
            using (var Context = new WLContext())
            {
                if (deck == null)
                {
                    cardsToLearn = Context.Cards.Where(c => c.IsMemorised == false).ToList();
                }
                else
                {
                    var allCards = Context.Cards
                        .Where(c => c.IsMemorised == false)
                        .Include(c => c.Decks)
                        .ThenInclude(c => c.Deck)
                        .ToList();

                    foreach (var card in allCards)
                    {
                        foreach (var d in card.Decks)
                        {
                            if (d.Deck == null) continue;
                            if (d.Deck.Id == deck.Id) cardsToLearn.Add(card);
                        }
                    }
                }

                foreach (var crd in cardsToLearn)
                {
                    var allCards = Context.Cards
                    .Include(c => c.Category)
                    .ToList();

                    var thisCard = new Card();

                    foreach (var c in allCards)
                    {
                        if (c.Id == crd.Id)
                        {
                            thisCard = c;
                            break;
                        }
                    }

                    currentCard = crd;

                    backside = crd.BackSide;

                    Table = new ConsoleTable("Front", "Category", "Memorized");
                    Table.AddRow(crd.FrontSide, thisCard.Category.Name, crd.IsMemorised);


                    

                    // Set the default index of the selected item to be the first
                    int index = 1;

                    // Write the menu out
                    WriteMenu(LearningProcessMenuOptions, LearningProcessMenuOptions[index]);

                    //Table.Write();

                    // Store key info in here
                    ConsoleKeyInfo keyinfo;
                    do
                    {
                        keyinfo = Console.ReadKey();

                        // Handle each key input (down arrow will write the menu again with a different selected item)
                        if (keyinfo.Key == ConsoleKey.DownArrow)
                        {
                            if (index + 1 < LearningProcessMenuOptions.Count)
                            {
                                index++;
                                WriteMenu(LearningProcessMenuOptions, LearningProcessMenuOptions[index]);
                            }
                        }

                        if (keyinfo.Key == ConsoleKey.UpArrow)
                        {
                            if (index - 1 >= 0)
                            {
                                index--;
                                WriteMenu(LearningProcessMenuOptions, LearningProcessMenuOptions[index]);
                            }
                        }

                        // Handle different action for the option
                        if (keyinfo.Key == ConsoleKey.Enter)
                        {
                            LearningProcessMenuOptions[index].Selected.Invoke();
                            index = 0;
                        }
                    }

                    while (keyinfo.Key != ConsoleKey.Spacebar);
                }

                WriteTemporaryMessage("Well Done!");

                new LearningDeckMenu().Run();
            }

        }
        // Default action of all the options.  
        public void WriteTemporaryMessage(string message)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("\t");
            Console.Write(message.ToUpper());
            Thread.Sleep(3000);
            WriteMenu(LearningProcessMenuOptions, LearningProcessMenuOptions.First());
        }

        private void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("Guess?\n");

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
            Console.WriteLine();
            Console.WriteLine("Press SPACEBAR to look the next word!");
        }
    }
}
