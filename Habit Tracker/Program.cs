using System;
using System.IO;
using System.Data.SQLite;

namespace Habit_Tracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //CreateTable();

            while (true)
            {
                MainMenu();
            }

        }
        public static void CreateTable(Database database)
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='habit'";

            database.OpenConnection();
            
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            SQLiteDataReader reader = myCommand.ExecuteReader();
            if (!reader.HasRows)
            {
                string table = "CREATE TABLE habit (id INTEGER PRIMARY KEY AUTOINCREMENT, quantity TEXT, date TEXT);";
                SQLiteCommand command = new SQLiteCommand(table, database.myConnection);
                command.ExecuteNonQuery();

                Console.WriteLine("Created Table \'habit\'");
            }

            database.CloseConnection();
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
                    DeleteRecord(databaseObject);
                    break;
                case "4":
                    UpdateRecord(databaseObject);
                    break;
                default:
                    Console.WriteLine("Invalid Entry.");
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

        public static void ViewRecords(Database database)
        {
            Console.WriteLine("\nDisplaying all records: ");

            string query = "SELECT rowid, * FROM habit";

            database.OpenConnection();
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            SQLiteDataReader result = myCommand.ExecuteReader();
            
            if (result.HasRows)
            {
                Console.WriteLine("Your Habit Count Logged... ");
                while (result.Read())
                {
                    Console.WriteLine("Id: {0} | Quantity: {1} | Date: {2}", result["id"], result["quantity"], result["date"]);
                }
            }

            database.CloseConnection();
        }

        public static void DeleteRecord(Database database)
        {
            Console.WriteLine("\nDeleting a Record...\nEnter ID for entry to delete: ");
            string delete_index = Console.ReadLine();

            string query = "DELETE FROM habit WHERE id=(@Id)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@Id", delete_index);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();

        }

        public static void UpdateRecord(Database database)
        {
            Console.WriteLine("\nUpdating a Record... ");
            Console.WriteLine("Please Enter the ID of the value to change.");
            string entry_id = Console.ReadLine();
            Console.WriteLine("Please Enter a new value: ");
            string entry_edit = Console.ReadLine();
            
            
            string query = "UPDATE habit SET quantity=(@quantity) WHERE id=(@id) ";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@quantity", entry_edit);
            myCommand.Parameters.AddWithValue("@id", entry_id);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
        }
    }
}
