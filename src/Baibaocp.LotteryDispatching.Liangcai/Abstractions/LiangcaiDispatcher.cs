using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Liangcai.Liangcai
{
    public abstract class LiangcaiDispatcher<TDispatchMessage> where TDispatchMessage : IDispatchMessage
    {
        private readonly ILogger<LiangcaiDispatcher<TDispatchMessage>> _logger;

        private readonly HttpClient _httpClient;

        private readonly DispatcherConfiguration _options;

        public LiangcaiDispatcher(DispatcherConfiguration options, string command, ILogger<LiangcaiDispatcher<TDispatchMessage>> logger)
        {
            _options = options;
            _logger = logger;
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.Deflate
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

        protected async Task<string> Send(TDispatchMessage message, string command)
        {
            string value = BuildRequest(message);
            string sign = Signature(command, message.LdpMerchanerId, value, out DateTime timestamp);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wAgent", message.LdpMerchanerId),
                new KeyValuePair<string, string>("wAction", command),
                new KeyValuePair<string, string>("wMsgID", timestamp.ToString("yyyyMMddHHmm")),
                new KeyValuePair<string, string>("wSign",sign.ToLower()),
                new KeyValuePair<string, string>("wParam",value),
            });
            HttpResponseMessage responseMessage = (await _httpClient.PostAsync("lot", content)).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            string msg = Encoding.GetEncoding("GB2312").GetString(bytes);
            return msg;
        }

        protected abstract string BuildRequest(TDispatchMessage message);
    }
}
