using System;
using System.IO;
using System.Data.SQLite;

namespace Habit_Tracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                MainMenu();
            }

        }
            

        public static void MainMenu()
        {
            Database databaseObject = new Database();

            Console.WriteLine("\nMAIN MENU\n\n" +
                "What would you like to do?\n\n" +
                "Type 0 to Close Application.\n" +
                "Type 1 to View All Records.\n" +
                "Type 2 to Insert Record.\n" +
                "Type 3 to Delete Record.\n" +
                "Type 4 to Update Record.\n");
            
            string selector = Convert.ToString(Console.ReadLine());

            switch (selector)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    ViewRecords(databaseObject);
                    break;
                case "2":
                    InsertRecord(databaseObject);
                    break;
                case "3":
                    Console.WriteLine("Deleting a Record... ");
                    break;
                case "4":
                    Console.WriteLine("Updating a Record... ");
                    break;
                default:
                    Console.WriteLine("Invalid Entry.");
                    break;

            }

        }

        public static void InsertRecord(Database database)
        {
            Console.WriteLine("Inserting a Record...\nPlease Enter the time and date of your habit: ");
            string entry_value = Console.ReadLine();
            
            string query = "INSERT INTO loggedhours ('hours') VALUES (@hours)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@hours", entry_value);
            var result = myCommand.ExecuteNonQuery();
            database.CloseConnection();

            Console.WriteLine("New Record Added : {0}", result);
        }

        public static void ViewRecords(Database database)
        {
            Console.WriteLine("Displaying all records: ");

            string query = "SELECT * FROM loggedhours";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
#
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("Hours: {0}", result["hours"]);
                }
            }

            database.CloseConnection();
        }
    }
}
