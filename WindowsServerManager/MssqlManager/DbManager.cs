using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MssqlManager.Dto;

namespace MssqlManager
{
    public class DbManager
    {
        private string _connectionString;
        private string _db;
        private readonly string _dataSource;

        public DbManager(string dataSource)
        {
            _dataSource = dataSource;
        }

        /// <summary>
        /// Configure connection string
        /// </summary>
        /// <param name="db">Database name</param>
        public void ConfigureConnectionString(string db)
        {
            _db = db;
            _connectionString = $@"Server={_dataSource}; Initial Catalog={db}; Persist Security Info=True; MultipleActiveResultSets=True; Integrated Security=SSPI;";
        }

        /// <summary>
        /// Drop database from connection string
        /// </summary>
        /// <returns>async Task</returns>
        public async Task DropDatabaseAsync()
        {
            var sqlExpression = $@"ALTER DATABASE {_db} SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                                   GO
                                   DROP DATABASE {_db}";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
    }
}