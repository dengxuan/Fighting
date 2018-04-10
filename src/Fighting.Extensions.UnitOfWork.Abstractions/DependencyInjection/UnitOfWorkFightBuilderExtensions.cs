using Fighting.DependencyInjection.Builder;
using Fighting.Extensions.UnitOfWork.DependencyInjection.Builder;
using System;

namespace Fighting.Extensions.UnitOfWork.DependencyInjection
{
    public static class UnitOfWorkFightBuilderExtensions
    {
        public static FightBuilder AddUnitOfWork(this FightBuilder fightBuilder, Action<UnitOfWorkBuilder> buildAction)
        {
            UnitOfWorkBuilder builder = new UnitOfWorkBuilder(fightBuilder.Services);
            buildAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
