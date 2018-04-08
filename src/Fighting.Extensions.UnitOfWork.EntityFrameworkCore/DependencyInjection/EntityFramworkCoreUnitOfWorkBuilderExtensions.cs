using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore.DependencyInjection
{
    public static class EntityFramworkCoreUnitOfWorkBuilderExtensions
    {
        public static DbContextOptionsBuilder UseEntityFrameworkCore(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder;
        }
    }
}
