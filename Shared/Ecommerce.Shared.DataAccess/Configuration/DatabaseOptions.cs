namespace Ecommerce.Shared.DataAccess.Configuration
{
    public class DatabaseOptions
    {
        public DatabaseProviderType Provider { get; set; } = DatabaseProviderType.PostgreSQL;
        public string ConnectionString { get; set; } = string.Empty;
        public bool EnableRetryOnFailure { get; set; } = true;
        public int MaxRetryCount { get; set; } = 3;
        public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);
        
        // Schema configuration for modules
        public Dictionary<string, string> ModuleSchemas { get; set; } = new();
        public string DefaultSchema { get; set; } = "public";
    }

    public enum DatabaseProviderType
    {
        PostgreSQL,
        MongoDB,
        MySQL,
    }
}
