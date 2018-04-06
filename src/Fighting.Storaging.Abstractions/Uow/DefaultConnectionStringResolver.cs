using Fighting.DependencyInjection.Builder;

namespace Fighting.Storaging.Uow
{
    /// <summary>
    /// Default implementation of <see cref="IConnectionStringResolver"/>.
    /// Get connection string from <see cref="StorageConfiguration"/>,
    /// or "Default" connection string in config file,
    /// or single connection string in config file.
    /// </summary>
    [TransientDependency]
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        private readonly StorageConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultConnectionStringResolver"/> class.
        /// </summary>
        public DefaultConnectionStringResolver(StorageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            var defaultConnectionString = _configuration.DefaultNameOrConnectionString;
            if (!string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            //if (ConfigurationManager.ConnectionStrings["Default"] != null)
            //{
            //    return "Default";
            //}

            //if (ConfigurationManager.ConnectionStrings.Count == 1)
            //{
            //    return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            //}

            throw new FightingException("Could not find a connection string definition for the application. Set IAbpStartupConfiguration.DefaultNameOrConnectionString or add a 'Default' connection string to application .config file.");
        }
    }
}