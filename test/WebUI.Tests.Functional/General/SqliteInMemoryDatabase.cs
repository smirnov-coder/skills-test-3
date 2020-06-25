using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace WebUI.Tests.Functional
{
    public class SqliteInMemoryDatabase : IDisposable
    {
        private readonly string _connectionString = "Data Source=:memory:";

        public SQLiteConnection Connection { get; }

        public SqliteInMemoryDatabase()
        {
            Connection = new SQLiteConnection(_connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
