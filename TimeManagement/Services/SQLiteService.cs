using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TimeManagement;
using TimeManagement.Models;
using Xamarin.Forms;

namespace FoodOrderApp.Services.DatabaseService
{
    public class SqLiteService
    {
        private SQLiteConnection _sqLiteConnection;

        public int Count<T>() where T : new()
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            int count = _sqLiteConnection.Table<T>().Count();
            _sqLiteConnection.Close();
            return count;
        }

        public Task CreateTableAsync()
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            _sqLiteConnection.DropTable<Activity>();
            _sqLiteConnection.CreateTable<Activity>();
            _sqLiteConnection.Close();
            return Task.CompletedTask;
        }

        public Task DeleteAllAsync()
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            _sqLiteConnection.DeleteAll<Activity>();
            _sqLiteConnection.Commit();
            _sqLiteConnection.Close();
            return Task.CompletedTask;
        }

        public async Task<List<Activity>> ToListAsync()
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            var items = _sqLiteConnection.Table<Activity>().ToList();
            _sqLiteConnection.Close();
            return items;
        }

        public async Task InsertAsync(Activity item)
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            _sqLiteConnection.Insert(item);
            _sqLiteConnection.Commit();
            _sqLiteConnection.Close();
        }
        
        public async Task UpdateAsync(List<DayProgram> item)
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            _sqLiteConnection.Update(item);
            _sqLiteConnection.Commit();
            _sqLiteConnection.Close();
        }
    }
}