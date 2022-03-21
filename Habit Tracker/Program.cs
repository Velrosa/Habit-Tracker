using System;
using System.IO;
using System.Data.SQLite;

namespace Habit_Tracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database databaseObject = new Database();
            MainMenu();
            string selector = Convert.ToString(Console.ReadLine());
            Console.WriteLine(selector);
        }

        public static void MainMenu()
        {
            Console.WriteLine("\nMAIN MENU\n\n" +
                "What would you like to do?\n\n" +
                "Type 0 to Close Application.\n" +
                "Type 1 to View All Records.\n" +
                "Type 2 to Insert Record.\n" +
                "Type 3 to Delete Record.\n" +
                "Type 4 to Update Record.\n");
        }
    }
}
