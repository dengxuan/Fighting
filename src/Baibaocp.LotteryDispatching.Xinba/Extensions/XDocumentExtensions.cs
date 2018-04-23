using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Xinba.Extensions
{
    /// <summary>
    /// 转换类扩展
    /// </summary>
    public static class XDocumentExtensions
    {
        /// <summary>
        /// 将XDocument转换为string
        /// </summary>
        /// <param name="doc">带转换文档对象</param>
        /// <param name="options">转换格式</param>
        /// <param name="detectEncodingFromByteOrderMarks">是否包含UTF8 BOM</param>
        /// <returns>转换后的字符串</returns>
        public static string ToString(this XDocument doc, SaveOptions options, bool detectEncodingFromByteOrderMarks)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms, options);
                ms.Seek(0, SeekOrigin.Begin);
                string retval = string.Empty;
                using (StreamReader reader = new StreamReader(ms, detectEncodingFromByteOrderMarks))
                {
                    retval = reader.ReadToEnd();
                }
                return retval;
            }
        }

        /// <summary>
        /// 将XDocument 写入内存流里面
        /// </summary>
        /// <param name="doc">XDocument对象</param>
        /// <param name="stream">待写入的流对象</param>
        /// <param name="options">保存选项</param>
        /// <param name="detectEncodingFromByteOrderMarks">是否包含UTF8 BOM</param>
        /// <returns>不包含BOM头的文档流</returns>
        public static void WriteTo(this XDocument doc, MemoryStream stream, SaveOptions options, bool detectEncodingFromByteOrderMarks)
        {
            WriteTo(doc, stream, options, new UTF8Encoding(detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        /// 将XDocument 写入内存流里面
        /// </summary>
        /// <param name="doc">XDocument对象</param>
        /// <param name="stream">待写入的流对象</param>
        /// <param name="options">保存选项</param>
        /// <param name="encoding">编码</param>
        /// <returns>文档流</returns>
        public static void WriteTo(this XDocument doc, MemoryStream stream, SaveOptions options, Encoding encoding)
        {
            StreamWriter writer = new StreamWriter(stream, encoding);
            doc.Save(writer, options);
        }

        /// <summary>
        /// 将字符串清除 '\r', '\n', '\0' 以后转换为xml
        /// </summary>
        /// <param name="xmlString">待转换字符串</param>
        /// <returns>XElement</returns>
        public static XDocument ParseXml(this string xmlString)
        {
            //清掉垃圾字符,再转XML
            xmlString = xmlString.Trim(new Char[] { '\r', '\n', '\0' });
            XDocument body = XDocument.Parse(xmlString);
            return body;
        }
    }
}
