using System;
using System.IO;
using System.Runtime.InteropServices;
using SQLite;
using TimeManagement.Droid;
using TimeManagement.Interfaces;
using Xamarin.Forms;

[assembly:Dependency(typeof(SqLiteAndroid))]
namespace TimeManagement.Droid
{
    public class SqLiteAndroid:ISqLite
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var sqliteFileName = "MyDatabase.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFileName);
            var cn = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return cn;
        }
    }
}