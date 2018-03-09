using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Json;
using Fighting.Security.Cryptography;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions
{
    public abstract class ExecuteHandler<TExecuter> : IExecuteHandler<TExecuter> where TExecuter : IExecuteMessage
    {
        private readonly string _command;

        private readonly ILogger<ExecuteHandler<TExecuter>> _logger;

        private readonly HttpClient _httpClient;

        private readonly DispatcherOptions _options;

        protected Tripledescrypt _crypter;

        public ExecuteHandler(DispatcherOptions options, ILoggerFactory loggerFactory, string command)
        {
            _options = options;
            _command = command;
            _logger = loggerFactory.CreateLogger<ExecuteHandler<TExecuter>>();
            _crypter = Tripledescrypt.Create(CipherMode.CBC, PaddingMode.PKCS7);
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
            string CipherText = _crypter.Encrypt(value, _options.SecretKey);
            string s = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", command, CipherText, timestamp, ldpVenderId, "1.0");
            return s.hmac_md5(_options.SecretKey.Substring(0, 16));
        }

        protected async Task<string> Send(TExecuter executer)
        {
            string value = BuildRequest(executer);
            string sign = Signature(_command, executer.LdpVenderId, value, out DateTime timestamp);
            ReqContent reqcon = new ReqContent()
            {
                version = "1.0",
                apiCode = _command,
                partnerId = executer.LdpVenderId,
                messageId = timestamp.ToString("yyyyMMddHHmm"),
                content = value,
                hmac = sign.ToLower()
            };
            string json = JsonExtensions.ToJsonString(reqcon);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = (await _httpClient.PostAsync("lot", content)).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            string msg = Encoding.UTF8.GetString(bytes);
            return msg;
        }

        protected abstract string BuildRequest(TExecuter executer);

        public abstract Task<IHandle> HandleAsync(TExecuter executer);
    }
}
