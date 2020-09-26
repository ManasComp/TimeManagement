﻿using System;
 using FoodOrderApp.Model;
using System.Linq;
 using System.Runtime.InteropServices;
 using System.Threading.Tasks;
 using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
 using TimeManagement.Services;
 using Xamarin.Forms;

// [assembly:Dependency(typeof(UserService))]//Dependency
namespace FoodOrderApp.Services
{
    class UserService//it provides Login and Registration to the FirebaseDatabase
    {
        private readonly FirebaseService _firebaseService;
        public UserService()
        {
            _firebaseService = new FirebaseService();
        }

        public async Task<bool> RegisterUser(string uname, string passwd)
        {
            if (await IsUserExists(uname) == false)
            {
                await _firebaseService.PostAsync("Users", new User() { Username = uname, Password = passwd, Id=Guid.NewGuid().ToString()});
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private async Task<bool> IsUserExists(string uname)
        {
            User user=(await _firebaseService.OnceAsync<User>("Users"))
                .FirstOrDefault(u => u.Username == uname);
           
            return (user != null);
        }

        public async Task<bool> Login(string uname, string passwd)
        {
            var user = (await _firebaseService.OnceAsync<User>("Users"))
                .Where(u => u.Username == uname)
                .FirstOrDefault(u => u.Password == passwd);
            return (user != null);
        }
    }
}
