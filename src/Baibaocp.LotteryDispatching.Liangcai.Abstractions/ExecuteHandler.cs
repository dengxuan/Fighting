﻿using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Liangcai
{
    public abstract class ExecuteHandler<TExecuter> : IExecuteHandler<TExecuter> where TExecuter : IExecuter
    {
        private readonly string _command;

        private readonly ILogger<ExecuteHandler<TExecuter>> _logger;

        private readonly HttpClient _httpClient;

        private readonly DispatcherOptions _options;

        public ExecuteHandler(DispatcherOptions options, ILoggerFactory loggerFactory, string command)
        {
            _options = options;
            _command = command;
            _logger = loggerFactory.CreateLogger<ExecuteHandler<TExecuter>>();
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(options.Url)
            };
        }

        private string Signature(string command, string ldpVenderId, string value, out DateTime timestamp)
        {
            timestamp = DateTime.Now;
            string text = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", ldpVenderId, command, timestamp, value, _options.SecretKey);
            return text.ToMd5();
        }

        protected async Task<string> Send(TExecuter executer)
        {
            string value = BuildRequest(executer);
            string sign = Signature(_command, executer.LdpVenderId, value, out DateTime timestamp);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wAgent", executer.LdpVenderId),
                new KeyValuePair<string, string>("wAction",_command),
                new KeyValuePair<string, string>("wMsgID", timestamp.ToString("yyyyMMddHHmm")),
                new KeyValuePair<string, string>("wSign",sign.ToLower()),
                new KeyValuePair<string, string>("wParam",value),
            });
            HttpResponseMessage responseMessage = (await _httpClient.PostAsync("lot", content)).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            string msg = Encoding.GetEncoding("GB2312").GetString(bytes);
            return msg;
        }

        protected abstract string BuildRequest(TExecuter executer);

        public abstract Task<MessageHandle> HandleAsync(TExecuter executer);
    }
}