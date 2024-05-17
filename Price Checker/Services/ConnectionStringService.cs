using System;

namespace Price_Checker.Configuration
{
    internal static class ConnectionStringService
    {
        private static readonly Lazy<string> _connectionString = new Lazy<string>(() =>
        {
            var config = new DatabaseConfig();
            return $"server={config.Server};port={config.Port};uid={config.Uid};pwd={config.Pwd};database={config.Database}";
        });

        public static string ConnectionString => _connectionString.Value;
    }
}
