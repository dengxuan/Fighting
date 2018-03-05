using Fighting.Storaging.Abstractions;
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
