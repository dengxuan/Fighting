﻿using System.Data;

namespace Fighting.Storaging.Data.Transactions.Abstractions
{
    public interface IActiveTransactionProvider
    {
        /// <summary>
        ///     Gets the active transaction or null if current UOW is not transactional.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args);

        /// <summary>
        ///     Gets the active database connection.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args);
    }
}
