﻿using System;
using System.IO;
using SQLite;
using TimeManagement.iOS;
using Xamarin.Forms;

[assembly:Dependency(typeof(SqLiteIOs))]
namespace TimeManagement.iOS
{
    public class SqLiteIOs:ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFileName = "MyDatabase.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libratyPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libratyPath, sqliteFileName);
            var cn = new SQLiteConnection(path);
            return cn;
        }
    }
}