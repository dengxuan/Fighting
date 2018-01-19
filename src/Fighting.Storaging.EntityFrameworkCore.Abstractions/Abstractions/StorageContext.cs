using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Storaging.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class StorageContext : DbContext
    {

        protected StorageOptions StorageOptions { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected StorageContext(StorageOptions storageOptions)
        {
            StorageOptions = storageOptions ?? throw new ArgumentNullException(nameof(storageOptions));
        }
    }
}
