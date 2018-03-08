using Microsoft.Extensions.Options;

namespace Fighting.MessageServices.DependencyInjection.Builder
{
    internal class MessageServiceOptionsSetup : ConfigureOptions<MessageServiceOptions>
    {
        public MessageServiceOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(MessageServiceOptions options)
        {
        }
    }
}
