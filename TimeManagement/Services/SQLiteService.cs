using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TimeManagement.Models;
using Xamarin.Forms;

namespace TimeManagement.Services
{
    public class SqLiteService
    {
        private SQLiteConnection _sqLiteConnection;
        private readonly PageService _pageService;

        public SqLiteService()
        {
            _pageService = new PageService();
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
        }
        public async Task<int> Count<T>() where T : new()
        {
            int count = _sqLiteConnection.Table<T>().Count();
            _sqLiteConnection.Close();
            return count;
        }

        public Task CreateTableAsync()
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            _sqLiteConnection.CreateTable<Activity>();
            _sqLiteConnection.Close();
            return Task.CompletedTask;
        }

        public async Task DropTableAsync()
        {
            _sqLiteConnection.DropTable<Activity>();
            _sqLiteConnection.Close();
            await _pageService.SetIsCartTableCreated(false);
        }

        public Task DeleteAllAsync()
        {
            _sqLiteConnection.DeleteAll<Activity>();
            _sqLiteConnection.Commit();
            _sqLiteConnection.Close();
            return Task.CompletedTask;
        }

        public async Task<List<Activity>> ToListAsync()
        {
            _sqLiteConnection = DependencyService.Get<ISqLite>().GetConnection();
            List<Activity> items = _sqLiteConnection.Table<Activity>().ToList();
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
            _sqLiteConnection.Update(item);
            _sqLiteConnection.Commit();
            _sqLiteConnection.Close();
        }
    }
}