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

        public FeatureManager(string dataSource)
        {
            _dataSource = dataSource;
        }

        public void ConfigureConnectionString(string db)
        {
            _connectionString = $@"Server={_dataSource}; Initial Catalog={db}; Persist Security Info=True; MultipleActiveResultSets=True; Integrated Security=SSPI;";
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
                SqlDataReader reader;
                try
                {
                    reader = command.ExecuteReaderAsync().Result;
                }
                catch (Exception e)
                {
                    throw;
                }

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
            }

            return features;
        }

        public async Task SetFeaturesState(List<FeatureDto> features)
        {
            var groupByFeatureState = features.GroupBy(x => x.State);
            foreach (var group in groupByFeatureState)
            {
                var featuresId = group.Select(x => x.Id);
                var featuresIdString = string.Join("','", featuresId);
                var state = group.Key;

                await SetFeatureState(featuresIdString, state);
            }
        }

        private async Task SetFeatureState(string featuresId, bool state)
        {
            const string sqlExpression = @"UPDATE AdminUnitFeatureState
                                           SET FeatureState = @featureState
                                           WHERE FeatureId IN (@featuresId)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter featureStateParam = new SqlParameter("@featureState", state);
                command.Parameters.Add(featureStateParam);
                SqlParameter featuresIdParam = new SqlParameter("@featuresId", featuresId);
                command.Parameters.Add(featuresIdParam);

                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
    }
}
