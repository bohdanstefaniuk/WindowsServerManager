using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
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

        private static IEnumerable<string> SplitSqlStatements(string sqlScript)
        {
            // Split by "GO" statements
            var statements = Regex.Split(
                sqlScript,
                @"^[\t\r\n]*GO[\t\r\n]*\d*[\t\r\n]*(?:--.*)?$",
                RegexOptions.Multiline |
                RegexOptions.IgnorePatternWhitespace |
                RegexOptions.IgnoreCase);

            // Remove empties, trim, and return
            return statements
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim(' ', '\r', '\n'));
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
            _connectionString = $@"Server={_dataSource}; Initial Catalog=master; Persist Security Info=True; MultipleActiveResultSets=True; Integrated Security=SSPI;";
            var sqlExpression = $@"ALTER DATABASE [{_db}] SET OFFLINE WITH ROLLBACK IMMEDIATE
									GO
									DROP DATABASE [{_db}]";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queries = SplitSqlStatements(sqlExpression);
                foreach (var query in queries)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task<bool> GetDatabaseExists()
        {
            var sqlExpression = $@"
                                DECLARE @dbname nvarchar(128)
                                SET @dbname = N'{_db}'
                                SELECT CASE WHEN EXISTS (
                                    SELECT name 
                                    FROM master.dbo.sysdatabases 
                                    WHERE ('[' + name + ']' = @dbname 
                                    OR name = @dbname)
                                )
                                THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END";

            var dbExists = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReaderAsync().Result;

                await reader.ReadAsync();
                dbExists = Convert.ToBoolean(reader.GetValue(0));

                reader.Close();
                connection.Close();
            }

            return dbExists;
        }
    }
}