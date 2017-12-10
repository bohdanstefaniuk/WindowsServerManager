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
        private readonly string _connectionString;

        public FeatureManager(string dataSource, string db)
        {
            _connectionString = $@"Server={dataSource}; Initial Catalog={db}; Persist Security Info=True; MultipleActiveResultSets=True; Integrated Security=SSPI;";
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
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetValue(0);
                        var code = reader.GetValue(1);
                        var state = reader.GetValue(2);

                        features.Add(new FeatureDto
                        {
                            Id = (Guid)id,
                            Code = (string)code,
                            State = (bool)state
                        });
                    }
                }

                reader.Close();
            }

            return features;
        }
    }
}
