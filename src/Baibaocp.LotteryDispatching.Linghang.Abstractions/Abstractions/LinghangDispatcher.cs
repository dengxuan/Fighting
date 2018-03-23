using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Json;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Linghang.Abstractions.Abstractions
{
    public abstract class LinghangDispatcher<TExecuteMessage> where TExecuteMessage : IDispatchMessage
    {
        private readonly string _command;

        private readonly ILogger<LinghangDispatcher<TExecuteMessage>> _logger;

        private readonly HttpClient _httpClient;

        private readonly DispatcherConfiguration _options;


        public LinghangDispatcher(DispatcherConfiguration options, ILogger<LinghangDispatcher<TExecuteMessage>> logger, string command)
        {
            _options = options;
            _command = command;
            _logger = logger;
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
            string Key = _options.SecretKey.ToMd5().ToUpper();
            string s = string.Format("{0}{1}{2}{3:yyyy-MM-dd HH:mm:ss}", command, Key, value, timestamp);
            return s.ToMd5().ToUpper();
        }

        protected async Task<string> Send(TExecuteMessage message)
        {
            string value = BuildRequest(message);
            string sign = Signature(_command, message.LdpMerchanerId, value, out DateTime timestamp);
            Head head = new Head {
                cmd = _command,
                userId = message.LdpMerchanerId,
                timeStamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                version = "1.0",
                sign = sign,
                status = 0,
                statusDes = 0
            };
            Content reqcon = new Content {
                head = head,
                body = value
            };
            string json = JsonExtensions.ToJsonString(reqcon);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = (await _httpClient.PostAsync("partner", content)).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            string msg = Encoding.UTF8.GetString(bytes);
            return msg;
        }

        protected bool Verify(string msg, out string CipherText)
        {
            CipherText = string.Empty;
            Content rescon = JsonConvert.DeserializeObject<Content>(msg);
            if (rescon.head.status.Equals(0))
            {
                string s = string.Format("{0}{1}{2}{3}{4}", rescon.apiCode, rescon.content, rescon.messageId, rescon.resCode, rescon.resMsg);
                string sign = s.ToMd5().ToUpper();
                if (rescon.head.sign != sign)
                {
                    return false;
                }
                CipherText = rescon.body;
            }
            else
            {
                return false;
            }
            return true;
        }

        protected abstract string BuildRequest(TExecuteMessage message);
    }
}
