using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MssqlManager.Dto;

namespace MssqlManager
{
    public class FeatureManager
    {
        private string _connectionString;
        private readonly string _dataSource;
        private string _db;

        public FeatureManager(string dataSource)
        {
            _dataSource = dataSource;
        }

        public void ConfigureConnectionString(string db)
        {
            _connectionString = $@"Server={_dataSource}; Initial Catalog={db}; Persist Security Info=True; MultipleActiveResultSets=True; Integrated Security=SSPI;";
        }

        public async Task<bool> GetFeatureTableExist()
        {
            var sqlExpression = $@"
                                SELECT CASE WHEN EXISTS (
                                    SELECT *  FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_NAME = 'Feature'
                                )
                                THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END";

            var tableExists = false; 
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReaderAsync().Result;

                await reader.ReadAsync();
                tableExists = Convert.ToBoolean(reader.GetValue(0));

                reader.Close();
                connection.Close();
            }

            return tableExists;
        }

        public async Task<IEnumerable<FeatureDto>> GetFeatures()
        {
            const string sqlExpression = @"SELECT f.Id, f.Code, a.FeatureState 
                                         FROM Feature as f
                                         INNER JOIN AdminUnitFeatureState as a
	                                        ON f.Id = a.FeatureId";
            var features = new List<FeatureDto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReaderAsync().Result;
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        features.Add(new FeatureDto
                        {
                            Id = (Guid)reader.GetValue(0),
                            Code = (string)reader.GetValue(1),
                            State = Convert.ToBoolean(reader.GetValue(2))
                        });
                    }
                }

                reader.Close();
                connection.Close();
            }

            return features;
        }

        public async Task SetFeaturesState(List<FeatureDto> features)
        {
            var groupByFeatureState = features.GroupBy(x => x.State);
            foreach (var group in groupByFeatureState)
            {
                var featuresId = group.Select(x => x.Id);
                var state = group.Key;

                await SetFeatureState(featuresId.ToArray(), state);
            }
        }

        private async Task SetFeatureState(Guid[] featuresId, bool state)
        {
            var featuresIdParameters = new string[featuresId.Length];
            SqlCommand command = new SqlCommand();
            for (var i = 0; i < featuresId.Length; i++)
            {
                featuresIdParameters[i] = $"@Feature{i}Id";
                command.Parameters.AddWithValue(featuresIdParameters[i], featuresId[i]);
            }

            var featuresIdParametersString = string.Join(", ", featuresIdParameters);
            var sqlExpression = $@"UPDATE AdminUnitFeatureState
                                    SET FeatureState = {Convert.ToInt32(state)}
                                    WHERE FeatureId in ({featuresIdParametersString})";

            command.CommandText = sqlExpression;
            command.Connection = new SqlConnection(_connectionString);
            await command.Connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            command.Connection.Close();
            command.Dispose();
        }
    }
}
