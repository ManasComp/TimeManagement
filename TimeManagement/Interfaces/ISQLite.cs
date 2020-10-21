using SQLite;

namespace TimeManagement.Interfaces
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection();
    }
}