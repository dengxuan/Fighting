﻿using Fighting.Caching.Abstractions;
using Newtonsoft.Json;
using System;

namespace Fighting.Caching.Redis
{
    internal class JsonCachingSerializer: ICachingSerializer
    {
        private readonly CachingOptions _options;

        public JsonCachingSerializer(CachingOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public byte[] Serialize<T>(T @object)
        {
            if (@object == null) throw new ArgumentNullException(nameof(@object));

            var @string = JsonConvert.SerializeObject(@object);
            return _options.Encoding.GetBytes(@string);
        }

        /// <inheritdoc />
        public object Deserialize(Type type, byte[] bytes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            var @string = _options.Encoding.GetString(bytes);
            return JsonConvert.DeserializeObject(@string, type);
        }
    }
}
