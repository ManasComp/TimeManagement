using SQLite;

namespace TimeManagement
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection();
    }
}