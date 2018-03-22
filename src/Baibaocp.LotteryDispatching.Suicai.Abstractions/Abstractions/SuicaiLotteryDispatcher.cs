using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Json;
using Fighting.Security.Cryptography;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions
{
    public abstract class SuicaiLotteryDispatcher<TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        private readonly string _command;

        private readonly ILogger<SuicaiLotteryDispatcher<TExecuteMessage>> _logger;

        private readonly HttpClient _httpClient;

        private readonly DispatcherConfiguration _options;

        protected Tripledescrypt _crypter;

        public SuicaiLotteryDispatcher(DispatcherConfiguration options, ILogger<SuicaiLotteryDispatcher<TExecuteMessage>> logger, string command)
        {
            _options = options;
            _command = command;
            _logger = logger;
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
            
            string s = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", command, value, timestamp, ldpVenderId, "1.0");
            return s.hmac_md5(_options.SecretKey.Substring(0, 16));
        }

        protected async Task<string> Send(TExecuteMessage message)
        {
            string value = BuildRequest(message);
            string CipherText = _crypter.Encrypt(value, _options.SecretKey);
            string sign = Signature(_command, message.LdpMerchanerId, CipherText, out DateTime timestamp);
            ReqContent reqcon = new ReqContent()
            {
                version = "1.0",
                apiCode = _command,
                partnerId = message.LdpMerchanerId,
                messageId = timestamp.ToString("yyyyMMddHHmm"),
                content = CipherText,
                hmac = sign.ToLower()
            };
            string json = JsonExtensions.ToJsonString(reqcon);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = (await _httpClient.PostAsync("partner", content)).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            string msg = Encoding.UTF8.GetString(bytes);
            return msg;
        }

        protected bool Verify(string msg,out string CipherText)
        {
            CipherText = string.Empty;
            ResContent rescon = JsonConvert.DeserializeObject<ResContent>(msg);
            if (rescon.resCode.Equals("0"))
            {
                string s = string.Format("{0}{1}{2}{3}{4}", rescon.apiCode, rescon.content, rescon.messageId, rescon.resCode, rescon.resMsg);
                string hmac = s.hmac_md5(_options.SecretKey.Substring(0, 16)).ToLower();
                if (rescon.hmac != hmac)
                {
                    return false;
                }
                CipherText = _crypter.Decrypt(rescon.content, _options.SecretKey);
            }
            else {
                return false;
            }
            return true; 
        }

        protected abstract string BuildRequest(TExecuteMessage message);
    }
}
