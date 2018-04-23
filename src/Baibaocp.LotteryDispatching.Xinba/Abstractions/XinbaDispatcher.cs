using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Xinba.Extensions;
using Fighting.Security.Cryptography;
using Fighting.Security.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Xinba.Abstractions
{
    public abstract class XinbaDispatcher<TDispatchMessage> where TDispatchMessage : IDispatchMessage
    {
        private readonly ILogger<XinbaDispatcher<TDispatchMessage>> _logger;

        private readonly HttpClient _httpClient;

        private readonly DispatcherConfiguration _options;

        protected Tripledescrypt _crypter;

        public XinbaDispatcher(DispatcherConfiguration options, string command, ILogger<XinbaDispatcher<TDispatchMessage>> logger)
        {
            _options = options;
            _logger = logger;
            _crypter = Tripledescrypt.Create(CipherMode.CBC, PaddingMode.PKCS7);
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.Deflate
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(options.Url)
            };
        }

        /// <summary>
        /// 验证信息
        /// </summary>
        /// <param name="value">加密消息体</param>
        /// <returns>返回MD5值</returns>
        private string Signature(string value)
        {
            return value.ToMd5();
        }


        protected bool Verify(XDocument rescontent, out XDocument content)
        {
            content = new XDocument();
            XElement xml = rescontent.Root;
            XElement signElement = xml.Element("head").Element("md");
            XElement bodyElement = xml.Element("body");
            if (bodyElement.Value.VerifyMd5(signElement.Value) == false)
            {
                return false;
            }
            XElement result = xml.Element("head").Element("result");
            if (result.Value != "0")
            {
                return false;
            }
            content = _crypter.Decrypt(bodyElement.Value, _options.SecretKey).ParseXml();
            return true;
        }

        protected async Task<XDocument> Send(TDispatchMessage message,string command)
        {
            XDocument value = BuildRequest(message);
            string CipherText = _crypter.Encrypt(value.Root.ToString(SaveOptions.DisableFormatting), _options.SecretKey);
            string sign = Signature(CipherText);
            XDocument Message = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement head = new XElement("head");
            head.Add(new XElement("version", "2600"));
            head.Add(new XElement("command", command));
            head.Add(new XElement("venderId", message.LdpMerchanerId));
            head.Add(new XElement("messageId", message.LdpOrderId));
            head.Add(new XElement("md", sign));
            XElement body = new XElement("body", CipherText);
            Message.Add(new XElement("message", head, body));
            MemoryStream ms = new MemoryStream();
            Message.WriteTo(ms, SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces, false);
            HttpContent content = new StreamContent(ms);
            HttpResponseMessage responseMessage = (await _httpClient.PostAsync("", content)).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            string msg = Encoding.UTF8.GetString(bytes);
            return XDocument.Load(msg);
        }

        protected abstract XDocument BuildRequest(TDispatchMessage message);
    }
}
