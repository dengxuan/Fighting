using Fighting.MessageServices.DependencyInjection.Builder;
using Fighting.DependencyInjection.Builder;
using System;

namespace Fighting.MessageServices.DependencyInjection
{
    public static class MessageServiceFightBuilderExtensions
    {
        public static FightBuilder ConfigureMessageServices(this FightBuilder fightBuilder, Action<MessageServiceBuilder> setupAction)
        {
            MessageServiceBuilder builder = new MessageServiceBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
