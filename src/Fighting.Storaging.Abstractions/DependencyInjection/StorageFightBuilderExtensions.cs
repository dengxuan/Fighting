using Fighting.DependencyInjection.Builder;
using System;

namespace Fighting.DependencyInjection
{
    public static class StorageFightBuilderExtensions
    {
        public static FightBuilder ConfigureStorage(this FightBuilder fightBuilder, Action<StorageBuilder> setupAction)
        {
            StorageBuilder builder = new StorageBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
