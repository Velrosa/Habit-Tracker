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

            Console.Clear();
            
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
                    Console.Clear();
                    ViewRecords(databaseObject, selector);
                    break;
                case "2":
                    Console.Clear();
                    InsertRecord(databaseObject);
                    break;
                case "3":
                    Console.Clear();
                    DeleteRecord(databaseObject, selector);
                    break;
                case "4":
                    Console.Clear();
                    UpdateRecord(databaseObject, selector);
                    break;
                default:
                    Console.WriteLine("Invalid Entry.");
                    Console.ReadKey();
                    break;

            }

        }

        public static void InsertRecord(Database database)
        {
            Console.WriteLine("Inserting a Habit Record...\nPlease Enter the quantity: ");
            string quantity_value = Console.ReadLine();
            Console.WriteLine("Please Enter the Date: ");
            string date_value = Console.ReadLine();


            string query = "INSERT INTO habit (quantity, date) VALUES (@quantity, @date)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@quantity", quantity_value);
            myCommand.Parameters.AddWithValue("@date", date_value);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();

            //Console.WriteLine("New Record Added : {0}", result);
        }

        public static void ViewRecords(Database database, string selector)
        {
            Console.WriteLine("\nDisplaying all habit records:\n");

            string query = "SELECT rowid, * FROM habit";

            database.OpenConnection();
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            SQLiteDataReader result = myCommand.ExecuteReader();
            
            if (result.HasRows)
            {
                Console.WriteLine("ID | Quantity | Date");
                Console.WriteLine("--------------------");
                while (result.Read())
                {
                    Console.WriteLine("{0}  | {1}        | {2}", result["id"], result["quantity"], result["date"]);
                }
            }

            database.CloseConnection();
            
            if (selector == "1")
            {
                Console.WriteLine("\nPress any key to continue... ");
                Console.ReadKey();
            }
             
        }

        public static void DeleteRecord(Database database, string selector)
        {
            ViewRecords(database, selector);
            
            Console.WriteLine("\nDeleting a Record...\nEnter ID for entry to delete: ");
            string delete_index = Console.ReadLine();

            string query = "DELETE FROM habit WHERE id=(@Id)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@Id", delete_index);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();

        }

        public static void UpdateRecord(Database database, string selector)
        {
            ViewRecords(database, selector);
            
            Console.WriteLine("\nUpdating a Record... ");
            Console.WriteLine("Please Enter the ID of the log to change.");
            string entry_id = Console.ReadLine();
            Console.WriteLine("Please Enter a new quantity: ");
            string entry_edit = Console.ReadLine();
            Console.WriteLine("Please Enter a new date: ");
            string entry_date = Console.ReadLine();
            
            
            string query = "UPDATE habit SET quantity=(@quantity), date=(@date) WHERE id=(@id) ";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@quantity", entry_edit);
            myCommand.Parameters.AddWithValue("@date", entry_date);
            myCommand.Parameters.AddWithValue("@id", entry_id);

            myCommand.ExecuteNonQuery();
            database.CloseConnection();
        }
    }
}
