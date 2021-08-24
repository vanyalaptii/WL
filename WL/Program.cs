using System;
using WL.Context;
using WL.UI;

namespace WL
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var Context = new WLContext())
            {
                Context.Database.EnsureCreated();

                var mainMenu = new MainMenu();
                mainMenu.Run();
            }
        }
    }
}