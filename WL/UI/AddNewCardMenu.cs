using System;
using System.Collections.Generic;
using System.Linq;
using WL.Context;
using WL.Model;

namespace WL.UI
{
    public class AddNewCardMenu
    {
        public AddNewCardMenu() {}

        public void Run()
        {
            using (var Context = new WLContext())
            {
                var newCard = new Card();

                Console.Clear();

                Console.WriteLine("Input a new card front text..\n");
                Console.Write("\t");
                newCard.FrontSide = Console.ReadLine().ToLower();
                Console.WriteLine();

                Console.WriteLine("Input a new card back text..\n");
                Console.Write("\t");
                newCard.BackSide = Console.ReadLine().ToLower();
                Console.WriteLine();

                Console.WriteLine("Input a card category..\n");
                Console.Write("\t");
                var category = Console.ReadLine().ToLower();
                Console.WriteLine();

                var catInCont = Context.Categories.FirstOrDefault(c => c.Name.ToLower() == category.ToLower());

                if (catInCont == null)
                {
                    Context.Categories.Add(new Category() { Name = category });
                    Context.SaveChanges();
                    catInCont = Context.Categories.FirstOrDefault(c => c.Name.ToLower() == category.ToLower());
                    newCard.Category = catInCont;
                }
                else
                {
                    newCard.Category = catInCont;
                }

                Context.Cards.Add(newCard);
                Context.SaveChanges();

                new MainMenu().Run();
            }
        }
    }
}