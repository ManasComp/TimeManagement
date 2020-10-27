using SQLite;

namespace TimeManagement.Interfaces
{
    public interface ISqLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}