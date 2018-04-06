using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Storaging.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class StorageContext : DbContext
    {

        protected StorageConfiguration StorageOptions { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected StorageContext(StorageConfiguration storageOptions, DbContextOptions options) : base(options)
        {
            StorageOptions = storageOptions ?? throw new ArgumentNullException(nameof(storageOptions));
        }
    }
}
