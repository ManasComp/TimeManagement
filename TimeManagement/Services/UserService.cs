using System;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderApp.Model;
using FoodOrderApp.Services;

// [assembly:Dependency(typeof(UserService))]//Dependency
namespace TimeManagement.Services
{
    class UserService//it provides Login and Registration to the FirebaseDatabase
    {
        private readonly FirebaseService _firebaseService;
        private readonly string _userChild = "Users";
        public UserService()
        {
            _firebaseService = new FirebaseService();
        }

        public async Task<bool> RegisterUser(string uname, string passwd)
        {
            if (await IsUserExists(uname) == false)
            {
                await _firebaseService.PostAsync(_userChild, new User() { Username = uname, Password = passwd, Id = Guid.NewGuid().ToString() });
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private async Task<bool> IsUserExists(string uname)
        {
            User user=(await _firebaseService.OnceAsync<User>(_userChild))
                .FirstOrDefault(u => u.Username == uname);
           
            return (user != null);
        }

        public async Task<bool> Login(string uname, string passwd)
        {
            var user = (await _firebaseService.OnceAsync<User>(_userChild))
                .Where(u => u.Username == uname)
                .FirstOrDefault(u => u.Password == passwd);
            return (user != null);
        }
    }
}
