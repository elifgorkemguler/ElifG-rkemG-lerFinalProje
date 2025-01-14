using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading.Tasks;
using Proje.Models;

namespace Proje.Services
{
    public static class DatabaseHelper
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitializeDatabaseAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ProjeDatabase.db");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<User>();
        }

        public static SQLiteAsyncConnection GetDatabaseConnection() => _database;
    }
}
