using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WL.Context;
using WL.Model;
using WL.Operations;

namespace WL.UI
{
    public class MainMenu
    {
        public static List<Option> mainMenu;

        public MainMenu() { }

        public void Run()
        {
            Console.Clear();

            mainMenu = new List<Option>
            {
                new Option("Choose deck", () => new DecksMenu().Run()),
                new Option("Show all cards", () => new ShowAllCardsMenu().Run()),
                new Option("Add new card", () => new AddNewCardMenu().Run()),
                new Option("Exit", () => Environment.Exit(0)),
            };

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(mainMenu, mainMenu[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < mainMenu.Count)
                    {
                        index++;
                        WriteMenu(mainMenu, mainMenu[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(mainMenu, mainMenu[index]);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    mainMenu[index].Selected.Invoke();
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
            WriteMenu(mainMenu, mainMenu.First());
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
