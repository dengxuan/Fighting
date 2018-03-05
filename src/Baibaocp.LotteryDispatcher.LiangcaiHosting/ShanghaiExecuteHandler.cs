using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.MessageServices;
using Baibaocp.LotteryDispatcher.MessageServices.Messages;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Shanghai
{
    public abstract class ShanghaiExecuteHandler<TExecuter> : IExecuteHandler<TExecuter> where TExecuter : ExecuteMessage
    {
        private readonly string _command;

        private readonly ILogger<ShanghaiExecuteHandler<TExecuter>> _logger;

        private readonly HttpClient _httpClient;

        private readonly ShanghaiDispatcherOptions _options;

        public ShanghaiExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory, string command)
        {
            _options = options;
            _command = command;
            _logger = loggerFactory.CreateLogger<ShanghaiExecuteHandler<TExecuter>>();
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
