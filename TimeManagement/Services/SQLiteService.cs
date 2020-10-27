using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TimeManagement.Interfaces;
using TimeManagement.Models;

namespace TimeManagement.Services
{
    public class SqLiteService
    {
        private readonly SQLiteAsyncConnection _sqLiteConnection;
        private readonly PageService _pageService;

        public SqLiteService()
        {
            _pageService = new PageService();
            _sqLiteConnection = _pageService.DependencyServiceGet<ISqLite>().Result.GetConnection();
        }
        public async Task<int> Count<T>() where T : new()
        {
            int count = _sqLiteConnection.Table<T>().ToListAsync().Result.Count;
            return count;
        }

        public Task CreateTableAsync()
        {
            _sqLiteConnection.CreateTableAsync<Activity>();
            return Task.CompletedTask;
        }

        public async Task DropTableAsync()
        {
            await _sqLiteConnection.DropTableAsync<Activity>();
            await _pageService.SetIsCartTableCreated(false);
        }

        public Task DeleteAllAsync()
        {
            _sqLiteConnection.DeleteAllAsync<Activity>();
            return Task.CompletedTask;
        }

        public async Task<List<Activity>> ToListAsync()
        {
            List<Activity> items = _sqLiteConnection.Table<Activity>().ToListAsync().Result;
            return items;
        }

        public async Task InsertAsync(Activity item)
        {
            await _sqLiteConnection.InsertAsync(item);
        }
        
        public async Task UpdateAsync(List<DayProgram> item)
        {
            await _sqLiteConnection.UpdateAsync(item);
        }
    }
}