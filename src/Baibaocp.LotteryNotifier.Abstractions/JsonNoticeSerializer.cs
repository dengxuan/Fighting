using Baibaocp.LotteryNotifier.Abstractions;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Baibaocp.LotteryNotifier
{
    internal class JsonNoticeSerializer : INoticeSerializer
    {

        /// <inheritdoc />
        public byte[] Serialize<T>(T @object)
        {
            if (@object == null) throw new ArgumentNullException(nameof(@object));

            var @string = JsonConvert.SerializeObject(@object);
            return Encoding.UTF8.GetBytes(@string);
        }

        /// <inheritdoc />
        public object Deserialize(Type type, byte[] bytes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            var @string = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(@string, type);
        }
    }
}
