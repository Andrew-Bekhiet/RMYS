using Android.Database.Sqlite;
using Android.Util;
using System.IO;

namespace RMYS
{
    public class Database
    {
        private SQLiteDatabase sqldb;
        private string sqldb_query;
        private string sqldb_message;
        private int LstInt;
        private string LstOrder;
        private string LstMode;
        public bool DatabaseAvailable
        {
            get { return sqldb_available; }
            set { sqldb_available = value; }
        }
        public Android.Database.ICursor DBCursor;
        public string Message
        {
            get { return sqldb_message; }
            set { sqldb_message = value; }
        }
        private bool sqldb_available;
        public Database()
        {
            sqldb_message = "";
            sqldb_available = false;
        }
        public Database(string sqldb_name, int tablN)
        {
            LstInt = tablN;
            try
            {
                sqldb_message = "";
                sqldb_available = false;
                CreateOpenDatabase(sqldb_name, tablN);
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
        }
        public void CreateOpenDatabase(string sqldb_name, int tableName)
        {
            LstInt = tableName;
            try
            {
                sqldb_message = "";
                string sqldb_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), sqldb_name);
                bool sqldb_exists = File.Exists(sqldb_path);
                sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path + ".db", null);
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                if (tableName == 1)
                {
                    sqldb_query = "CREATE TABLE IF NOT EXISTS Maintbl(ID INTEGER PRIMARY KEY AUTOINCREMENT, Type VARCHAR, Name VARCHAR, Place VARCHAR, Days VARCHAR, Timing VARCHAR, Custs VARCHAR);";
                }
                else if (tableName == 2)
                {
                    sqldb_query = "CREATE TABLE IF NOT EXISTS HolyBible(ID INTEGER PRIMARY KEY AUTOINCREMENT, Title VARCHAR, Date VARCHAR, Verse VARCHAR, Visor VARCHAR);";
                }
                sqldb.ExecSQL(sqldb_query);
                if (!sqldb_exists)
                {
                    sqldb_message = "Database: " + sqldb_name + " created";
                }
                else
                {
                    sqldb_message = "Database: " + sqldb_name + " opened";
                }
                sqldb_available = true;
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);

        }
        public void AddRecord(string Type, string Name, string Place, string Days, string Timing, string Customs)
        {
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                sqldb_query = "INSERT INTO Maintbl (Type, Name, Place, Days, Timing, Custs) VALUES ('" + Type + "','" + Name + "','" + Place + "','" + Days + "','" + Timing + "','" + Customs + "');";

                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "تمت الإضافة بنجاح";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
        }
        public void AddHBRecord(string Title, string Date, string Verse, string Visor)
        {
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                sqldb_query = "INSERT INTO HolyBible (Title, Date, Verse, Visor) VALUES ('" + Title + "','" + Date + "','" + Verse + "','" + Visor + "');";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "تمت الإضافة بنجاح";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
        }
        public void UpdateRecord(int iId, string Type, string Name, string Place, string Days, string Timing, string Custs)
        {
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                sqldb_query = "UPDATE Maintbl SET Type = '" + Type + "',Name ='" + Name + "', Place = '" + Place + "', Days = '" + Days + "', Timing = '" + Timing + "', Custs = '" + Custs + "' WHERE ID ='" + iId + "';";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "تم الحفظ بنجاح";

            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
        }
        public void UpdateHBRecord(int id, string Title, string Date, string Verse, string Visor)
        {
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                sqldb_query = "UPDATE HolyBible SET Title = '" + Title + "',Date = '" + Date + "',Verse ='" + Verse + "', Visor = '" + Visor + "' WHERE ID ='" + id + "';";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "تم تحديث " + Title;
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
        }
        public void DeleteRecord(int iId, int tablid)
        {
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                if (tablid == 1)
                {
                    sqldb_query = "DELETE FROM Maintbl WHERE ID LIKE '" + iId + "';";
                }
                else if (tablid == 2)
                {
                    sqldb_query = "DELETE FROM HolyBible WHERE ID LIKE '" + iId + "';";
                }
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "Record " + iId + " deleted";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
        }
        public Android.Database.ICursor GetRecordCursor(int tablN, string cOrder, string mode)
        {
            Android.Database.ICursor sqldb_cursor = null;
            LstInt = tablN;
            LstOrder = cOrder;
            LstMode = mode;
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                if (tablN == 1)
                {
                    if (cOrder == "") { sqldb_query = "SELECT* FROM Maintbl;"; } else { sqldb_query = "SELECT* FROM Maintbl ORDER BY " + cOrder + " " + mode + ";"; }
                }
                else if (tablN == 2)
                {
                    if (cOrder == "") { sqldb_query = "SELECT* FROM HolyBible;"; } else { sqldb_query = "SELECT* FROM HolyBible ORDER BY " + cOrder + " " + mode + ";"; }
                }
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (sqldb_cursor.Count == 0)
                {
                    sqldb_message = "Record not found";
                }
                else
                {
                    sqldb_message = "Record found";
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
            return sqldb_cursor;
        }
        public Android.Database.ICursor GetRecordCursor(string sColumn, string sValue, int tablN)
        {
            Android.Database.ICursor sqldb_cursor = null;
            LstInt = tablN;
            try
            {
                sqldb_query = "PRAGMA encoding='UTF-16'";
                sqldb.ExecSQL(sqldb_query);
                if (tablN == 1)
                {
                    sqldb_query = "SELECT* FROM Maintbl WHERE " + sColumn + " LIKE '" + sValue + "';";
                }
                else if (tablN == 2)
                {
                    sqldb_query = "SELECT* FROM HolyBible WHERE " + sColumn + " LIKE '" + sValue + "';";
                }
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (sqldb_cursor.Count == 0)
                {
                    sqldb_message = "Record not found";
                }
                else
                {
                    sqldb_message = "Record found";
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            Log.Debug("Database", sqldb_message);
            return sqldb_cursor;
        }
    }
}