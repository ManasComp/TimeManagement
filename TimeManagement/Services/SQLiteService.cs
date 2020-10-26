using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TimeManagement.Interfaces;
using TimeManagement.Models;

namespace TimeManagement.Services
{
    public class SqLiteService
    {
        private readonly SQLiteConnection _sqLiteConnection;
        private readonly PageService _pageService;

        public SqLiteService()
        {
            _pageService = new PageService();
            _sqLiteConnection = _pageService.DependencyServiceGet<ISqLite>().Result.GetConnection();
        }
        public async Task<int> Count<T>() where T : new()
        {
            int count = _sqLiteConnection.Table<T>().Count();
            return count;
        }

        public Task CreateTableAsync()
        {
            _sqLiteConnection.CreateTable<Activity>();
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
            return Task.CompletedTask;
        }

        public async Task<List<Activity>> ToListAsync()
        {
            List<Activity> items = _sqLiteConnection.Table<Activity>().ToList();
            return items;
        }

        public async Task InsertAsync(Activity item)
        {
            _sqLiteConnection.Insert(item);
        }
        
        public async Task UpdateAsync(List<DayProgram> item)
        {
            _sqLiteConnection.Update(item);
        }
    }
}