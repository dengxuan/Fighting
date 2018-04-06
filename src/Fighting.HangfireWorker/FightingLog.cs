﻿// This file is part of Hangfire.
// Copyright © 2016 Sergey Odinokov.
// 
// Hangfire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// Hangfire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with Hangfire. If not, see <http://www.gnu.org/licenses/>.

using Hangfire.Annotations;
using Hangfire.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using System;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Fighting.HangfireWorker
{
    internal class FightingLog : ILog
    {
        private static readonly Func<object, Exception, string> MessageFormatterFunc = MessageFormatter;
        private static readonly object[] EmptyArgs = new object[0];

        private readonly ILogger _targetLogger;

        public FightingLog([NotNull] ILogger targetLogger)
        {
            if (targetLogger == null) throw new ArgumentNullException(nameof(targetLogger));
            _targetLogger = targetLogger;
        }

        public bool Log(Hangfire.Logging.LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
            var targetLogLevel = ToTargetLogLevel(logLevel);

            // When messageFunc is null, Hangfire.Logging
            // just determines is logging enabled.
            if (messageFunc == null)
            {
                return _targetLogger.IsEnabled(targetLogLevel);
            }

            _targetLogger.Log(targetLogLevel, 0, CreateStateObject(messageFunc()), exception, MessageFormatterFunc);
            return true;
        }

        private static LogLevel ToTargetLogLevel(Hangfire.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Hangfire.Logging.LogLevel.Trace:
                    return LogLevel.Trace;
                case Hangfire.Logging.LogLevel.Debug:
                    return LogLevel.Debug;
                case Hangfire.Logging.LogLevel.Info:
                    return LogLevel.Information;
                case Hangfire.Logging.LogLevel.Warn:
                    return LogLevel.Warning;
                case Hangfire.Logging.LogLevel.Error:
                    return LogLevel.Error;
                case Hangfire.Logging.LogLevel.Fatal:
                    return LogLevel.Critical;
            }

            return LogLevel.None;
        }

        private static object CreateStateObject(string message)
        {
            return new FormattedLogValues(message, EmptyArgs);
        }

        private static string MessageFormatter(object state, Exception exception)
        {
            return state.ToString();
        }
    }
}