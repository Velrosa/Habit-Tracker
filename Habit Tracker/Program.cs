using System;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;

namespace Habit_Tracker
{
    internal class Program
    {
        public static string dbString = "Data Source=database.sqlite3";

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
            
            string selector = Convert.ToString(Console.ReadKey(true).KeyChar);

            switch (selector)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    Console.Clear();
                    ViewRecords(selector);
                    break;
                case "2":
                    Console.Clear();
                    InsertRecord();
                    break;
                case "3":
                    Console.Clear();
                    ViewRecords(selector);
                    DeleteRecord(selector);
                    break;
                case "4":
                    Console.Clear();
                    ViewRecords(selector);
                    UpdateRecord(selector);
                    break;
                default:
                    Console.Write("Invalid Entry. press key to return... ");
                    Console.ReadKey();
                    break;

            }

        }

        public static string Validate(string entry, string type)
        {
            int valid_num = 0;

            
            if (type == "number" || type == "id")
            {
                bool isNumber = int.TryParse(entry, out valid_num);
                while (!isNumber || valid_num < 0)
                {
                    if (entry == "MENU")
                    {
                        return entry;
                    }
                    Console.Write("Invalid entry, Please enter a number: ");
                    entry = Console.ReadLine();
                    isNumber = int.TryParse(entry, out valid_num);
                }
            }
            
            if (type == "date")
            {
                while (true)
                {
                    if (entry == "MENU")
                    {
                        return entry;
                    }
                    else if (!Regex.IsMatch(entry, @"^\d{1,2}/\d{1,2}/\d{2,4}$"))
                    {
                        Console.Write("Invalid date, Please enter again (DD/MM/YY): ");
                        entry = Console.ReadLine();
                    }
                    else break;
                    
                }
            }
            return entry;

        }
        
        public static void InsertRecord()
        {
            Console.Write("Inserting a Habit Record...  Type MENU to return.\nPlease Enter the quantity: ");
            string quantity_value = Validate(Console.ReadLine(), "number");
            if (quantity_value == "MENU") { return; }
            
            Console.Write("Please Enter the Date (DD/MM/YY): ");
            string date_value = Validate(Console.ReadLine(), "date");
            if (date_value == "MENU") { return; }

            using (var con = new SQLiteConnection(dbString))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "INSERT INTO habit (quantity, date) VALUES (@quantity, @date)";
                    cmd.Parameters.AddWithValue("@quantity", quantity_value);
                    cmd.Parameters.AddWithValue("@date", date_value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ViewRecords(string selector)
        {
            Console.WriteLine("\nDisplaying all habit records:\n");

            using (var con = new SQLiteConnection(dbString))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT rowid, * FROM habit";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("  ID  | Quantity | Date");
                            Console.WriteLine("-----------------------------");
                            while (reader.Read())
                            {
                                Console.WriteLine("{0,5} | {1,8} | {2}", reader["id"], reader["quantity"], reader["date"]);
                            }
                        }
                    }
                }
            }
            
            if (selector == "1")
            {
                Console.Write("\nPress any key to return to menu... ");
                Console.ReadKey();
            }
             
        }

        public static void DeleteRecord(string selector)
        {
            Console.Write("\nDeleting a Record...  Type MENU to return.\nEnter ID for entry to delete: ");
            string delete_index = Validate(Console.ReadLine(), "id");
            if (delete_index == "MENU") { return; }

            using (var con = new SQLiteConnection(dbString))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "DELETE FROM habit WHERE id=(@Id)";
                    cmd.Parameters.AddWithValue("@Id", delete_index);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateRecord(string selector)
        {            
            Console.WriteLine("\nUpdating a Record...  Type MENU to return.");
            
            Console.Write("Please Enter the ID of the log to change: ");
            string entry_id = Console.ReadLine();
            if (entry_id == "MENU") { return; }
            
            Console.Write("Please Enter a new quantity: ");
            string entry_quantity = Validate(Console.ReadLine(), "number");
            if (entry_quantity == "MENU") { return; }
            
            Console.Write("Please Enter a new date (DD/MM/YY): ");
            string entry_date = Validate(Console.ReadLine(), "date");
            if (entry_date == "MENU") { return; }

            using (var con = new SQLiteConnection(dbString))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "UPDATE habit SET quantity=(@quantity), date=(@date) WHERE id=(@id) ";
                    cmd.Parameters.AddWithValue("@quantity", entry_quantity);
                    cmd.Parameters.AddWithValue("@date", entry_date);
                    cmd.Parameters.AddWithValue("@id", entry_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
