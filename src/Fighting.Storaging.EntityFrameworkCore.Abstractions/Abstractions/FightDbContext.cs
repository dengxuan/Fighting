using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fighting.Storaging.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class FightDbContext : DbContext
    {

        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected FightDbContext(DbContextOptions options) : base(options)
        {
            Logger = NullLogger.Instance;
        }
    }
}
