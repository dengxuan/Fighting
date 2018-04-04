using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Fighting.Storaging.Uow.Abstractions;

namespace Fighting.Storaging.EntityFrameworkCore
{
    public class DbContextTypeMatcher : DbContextTypeMatcher<StorageContext>
    {
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(currentUnitOfWorkProvider)
        {
        }
    }
}
