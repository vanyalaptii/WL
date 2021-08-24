using System;
using WL.Context;
using WL.Model;

namespace WL.UI
{
    public class AddNewDeckMenu
    {
        public AddNewDeckMenu() {}

        public void Run()
        {
            using (var Context = new WLContext())
            {
                var newDeck = new Deck();

                Console.Clear();
                Console.WriteLine("Input a new deck name..\n");
                newDeck.Name = Console.ReadLine();
                Context.Decks.Add(newDeck);
                Context.SaveChanges();
                new DecksMenu().Run();
            }
        }
    }
}
