using System.Collections.Generic;
using System.Threading.Tasks;
using TimeManagement.Models;

namespace TimeManagement
{
    public interface ISqLiteService
    {
        int Count<T>() where T : new();
        Task CreateTableAsync();
        Task DeleteAllAsync();
        Task<List<DayProgram>> ToListAsync();
        Task InsertAsync(DayProgram item);
        Task UpdateAsync(DayProgram item);
    }
}