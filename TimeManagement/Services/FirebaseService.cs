using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagement.Services
{
    public class FirebaseService
    {
        private const string _url = "https://timemanegment-74160.firebaseio.com/";
        
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient(_url);
        }

        public async Task PostAsync<T>(string child, T item)
        {
            await _firebaseClient.Child(child).PostAsync(item);
        }
        
        public async Task PostAsyncList<T>(string child, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                await _firebaseClient.Child(child).PostAsync(item);
            }
        }

        public async Task<List<T>> OnceAsync<T>(string child) where T : class
        {
            return (await _firebaseClient.Child(child).OnceAsync<T>()).Select(e => e.Object as T).ToList();
        }
    }
}