using SQLite;
//get a connection
namespace Team811Scout
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}