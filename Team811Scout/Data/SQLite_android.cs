using SQLite;
using System.IO;
using Xamarin.Forms;

//this creates a new connection to the database
//the file for the database is stored on the device at the path name given

[assembly: Dependency(typeof(Team811Scout.Data.SQLite_android))]
namespace Team811Scout.Data
{
    public class SQLite_android:ISQLite
    {
        public SQLite_android() { }
        
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "scoutdb.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLiteConnection(path);

            return conn;
        }
       
    }
}