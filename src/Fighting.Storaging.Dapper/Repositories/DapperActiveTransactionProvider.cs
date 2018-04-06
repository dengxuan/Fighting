using Fighting.Storaging.Data.Transactions;
using Fighting.Storaging.Data.Transactions.Abstractions;
using System;
using System.Data;

namespace Fighting.Storaging.Dapper.Repositories
{
    public class DapperActiveTransactionProvider : IActiveTransactionProvider
    {
        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            throw new NotImplementedException();
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
