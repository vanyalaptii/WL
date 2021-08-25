using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Context;
using WL.Model;

namespace WL.UI
{
    public class LearningDeckMenu
    {
        public List<Option> LearningDeckMenuOptions = new List<Option>();

        public LearningDeckMenu() { }

        public void Run()
        {
            Console.Clear();

            using (var Context = new WLContext())
            {
                LearningDeckMenuOptions.Add(new Option(" Back <--\n", () => new MainMenu().Run()));
                //LearningDeckMenuOptions.Add(new Option(" Add new deck\n", () => new AddNewDeckMenu().Run()));

                var decks = new List<Deck>();

                decks = Context.Decks.ToList();

                foreach (var deck in decks)
                {
                    LearningDeckMenuOptions.Add(new Option(deck.Name, () => new LearningProcessMenu(deck).Run()));
                }

                // Set the default index of the selected item to be the first
                int index = 1;

                // Write the menu out
                WriteMenu(LearningDeckMenuOptions, LearningDeckMenuOptions[index]);

                // Store key info in here
                ConsoleKeyInfo keyinfo;
                do
                {
                    keyinfo = Console.ReadKey();

                    // Handle each key input (down arrow will write the menu again with a different selected item)
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        if (index + 1 < LearningDeckMenuOptions.Count)
                        {
                            index++;
                            WriteMenu(LearningDeckMenuOptions, LearningDeckMenuOptions[index]);
                        }
                    }

                    if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        if (index - 1 >= 0)
                        {
                            index--;
                            WriteMenu(LearningDeckMenuOptions, LearningDeckMenuOptions[index]);
                        }
                    }

                    // Handle different action for the option
                    if (keyinfo.Key == ConsoleKey.Enter)
                    {
                        LearningDeckMenuOptions[index].Selected.Invoke();
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
            WriteMenu(LearningDeckMenuOptions, LearningDeckMenuOptions.First());
        }

        private void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            Console.WriteLine("Choose Deck\n");

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
