using Fighting.Extensions.UnitOfWork.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Fighting.Extensions.UnitOfWork
{
    internal class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
    {
        public TransactionScopeOption Scope { get; set; }

        /// <inheritdoc/>
        public bool IsTransactional { get; set; }

        /// <inheritdoc/>
        public TimeSpan? Timeout { get; set; }

        /// <inheritdoc/>
        public bool IsTransactionScopeAvailable { get; set; }

        /// <inheritdoc/>
        public IsolationLevel? IsolationLevel { get; set; }

        public List<Func<Type, bool>> ConventionalUowSelectors { get; }

        public UnitOfWorkDefaultOptions()
        {
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;

            IsTransactionScopeAvailable = true;

            ConventionalUowSelectors = new List<Func<Type, bool>>
            {
                type => typeof(IRepository).IsAssignableFrom(type)
            };
        }
    }
}