using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Habit_Tracker
{
    internal class Database
    {
        public SQLiteConnection myConnection;

        public Database()
        {
            myConnection = new SQLiteConnection("Data Source=database.sqlite3");

                if (!File.Exists("database.sqlite3"))
                {
                    SQLiteConnection.CreateFile("database.sqlite3");

                    myConnection.Open();
                    string table = "CREATE TABLE habit (id INTEGER PRIMARY KEY AUTOINCREMENT, quantity TEXT, date TEXT);";
                    SQLiteCommand command = new SQLiteCommand(table, myConnection);
                    command.ExecuteNonQuery();
                    myConnection.Close();
                }

        }
    }
}
