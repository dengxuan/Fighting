﻿using Fighting.Extensions.UnitOfWork.Abstractions;
using System.Threading.Tasks;

namespace Fighting.Extensions.UnitOfWork
{
    /// <summary>
    /// Null implementation of unit of work.
    /// It's used if no component registered for <see cref="IUnitOfWork"/>.
    /// This ensures working ABP without a database.
    /// </summary>
    public sealed class NullUnitOfWork : UnitOfWork
    {
        public override void SaveChanges()
        {

        }

        public override Task SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        protected override void BeginUow()
        {

        }

        protected override void CompleteUow()
        {

        }

        protected override Task CompleteUowAsync()
        {
            return Task.FromResult(0);
        }

        protected override void DisposeUow()
        {

        }

        public NullUnitOfWork(IUnitOfWorkDefaultOptions defaultOptions) : base(defaultOptions)
        {
        }
    }
}
