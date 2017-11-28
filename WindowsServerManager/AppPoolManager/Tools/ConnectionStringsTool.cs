namespace AppPoolManager.Tools
{
    internal class ConnectionStringsTool
    {
        public static string GetSectionFromString(string connectionString, string key)
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            return (string)builder[key];
        }
    }
}
