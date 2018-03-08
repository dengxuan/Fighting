using System;
using System.Security.Cryptography;
using System.Text;

namespace Fighting.Security.Extensions
{
    /// <summary>
    /// 散列扩展
    /// </summary>
    public static class HashExtensions
    {

        #region Hash

        /// <summary>
        /// 用指定编码方式验证Hash是否正确
        /// </summary>
        /// <param name="hash">HashAlgorithm对象</param>
        /// <param name="input">待验证数据</param>
        /// <param name="hashOfValue">待验证哈希值</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        private static bool VerifyHash(HashAlgorithm hash, string input, string hashOfValue, Encoding encoding)
        {
            // 哈希待验证字符串
            string hashOfInput = GetHash(hash, input, encoding);

            // 获取忽略大小写比较的字符串比较器
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hashOfValue) == 0;
        }

        /// <summary>
        /// 获取HASH值
        /// </summary>
        /// <param name="hash">HASH对象</param>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希结果</returns>
        private static string GetHash(HashAlgorithm hash, string input, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(input);
            byte[] hashBytes = hash.ComputeHash(bytes);
            string hashString = BitConverter.ToString(hashBytes);
            return hashString.Replace("-", "");
        }

        /// <summary>
        /// 获取HASH值
        /// </summary>
        /// <param name="hash">HASH对象</param>
        /// <param name="bytes">待哈希字节数组</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希结果</returns>
        private static string GetHash(HashAlgorithm hash, byte[] bytes)
        {
            byte[] hashBytes = hash.ComputeHash(bytes);
            string hashString = BitConverter.ToString(hashBytes);
            return hashString.Replace("-", "");
        }

        /// <summary>
        /// 获取base64编码后的hash值
        /// </summary>
        /// <param name="hash">HASH对象</param>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希结果</returns>
        private static string GetBase64Hash(HashAlgorithm hash, string input, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(input);
            byte[] hashBytes = hash.ComputeHash(bytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        #endregion

        #region MD5

        /// <summary>
        /// 以UTF8的编码方式获取字符串的MD5哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <returns>哈希后的MD5值</returns>
        public static string ToMd5(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                return GetHash(md5, input, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 获取字符串的md5哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">哈希编码方式</param>
        /// <returns>哈希后的MD5值</returns>
        public static string ToMd5(this string input, Encoding encoding)
        {
            using (MD5 md5 = MD5.Create())
            {
                return GetHash(md5, input, encoding);
            }
        }

        /// <summary>
        /// 验证MD5
        /// </summary>
        /// <param name="input">待验证字符串</param>
        /// <param name="hash">待验证的MD5哈希值</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifyMd5(this string input, string hash)
        {
            using (MD5 md5 = MD5.Create())
            {
                return VerifyHash(md5, input, hash, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 验证MD5
        /// </summary>
        /// <param name="input">待验证字符串</param>
        /// <param name="hash">待验证的MD5哈希值</param>
        /// <param name="encoding">编码</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifyMd5(this string input, string hash, Encoding encoding)
        {
            using (MD5 md5 = MD5.Create())
            {
                return VerifyHash(md5, input, hash, encoding);
            }
        }

        #endregion

        #region SHA1

        /// <summary>
        /// 以UTF8的编码方式获取字符串的SHA1哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <returns>哈希后的SHA1值</returns>
        public static string ToSha1(this string input)
        {
            return ToSha1(input, Encoding.UTF8);
        }

        /// <summary>
        /// 以UTF8的编码方式获取字符串的SHA1哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <returns>哈希后的SHA1值</returns>
        public static string ToSha1(this byte[] input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return GetHash(sha1, input);
            }
        }

        /// <summary>
        /// 以指定的编码方式获取字符串的SHA1哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希后的SHA1值</returns>
        public static string ToSha1(this string input, Encoding encoding)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return GetHash(sha1, input, encoding);
            }
        }

        /// <summary>
        /// 用UTF8的编码方式验证SHA1是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA1值</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha1(this string input, string hash)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return VerifyHash(sha1, input, hash, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 用指定编码方式验证SHA1是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA1</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha1(this string input, string hash, Encoding encoding)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return VerifyHash(sha1, input, hash, encoding);
            }
        }

        #endregion

        #region SHA256

        /// <summary>
        /// 以UTF8的编码方式获取字符串的SHA256哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <returns>哈希后的SHA256值</returns>
        public static string ToSha256(this string input)
        {
            return ToSha256(input, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的编码方式获取字符串的SHA256哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希后的SHA256值</returns>
        public static string ToSha256(this string input, Encoding encoding)
        {
            using (SHA256 sha1 = SHA256.Create())
            {
                return GetHash(sha1, input, encoding);
            }
        }

        /// <summary>
        /// 用UTF8的编码方式验证SHA256是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA256值</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha256(this string input, string hash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return VerifyHash(sha256, input, hash, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 用指定编码方式验证SHA256是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA256</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha256(this string input, string hash, Encoding encoding)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return VerifyHash(sha256, input, hash, encoding);
            }
        }

        #endregion

        #region SHA384

        /// <summary>
        /// 以UTF8的编码方式获取字符串的SHA384哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <returns>哈希后的SHA1值</returns>
        public static string ToSha384(this string input)
        {
            return ToSha384(input, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的编码方式获取字符串的SHA384哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希后的SHA384值</returns>
        public static string ToSha384(this string input, Encoding encoding)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                return GetHash(sha384, input, encoding);
            }
        }

        /// <summary>
        /// 用UTF8的编码方式验证SHA384是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA384值</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha384(this string input, string hash)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                return VerifyHash(sha384, input, hash, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 用指定编码方式验证SHA384是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA384</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha384(this string input, string hash, Encoding encoding)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                return VerifyHash(sha384, input, hash, encoding);
            }
        }

        #endregion

        #region SHA512

        /// <summary>
        /// 以UTF8的编码方式获取字符串的SHA512哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <returns>哈希后的SHA512值</returns>
        public static string ToSha512(this string input)
        {
            return ToSha512(input, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的编码方式获取字符串的SHA512哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希后的SHA512值</returns>
        public static string ToSha512(this string input, Encoding encoding)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                return GetHash(sha512, input, encoding);
            }
        }

        /// <summary>
        /// 用UTF8的编码方式验证SHA384是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA384值</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha512(this string input, string hash)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                return VerifyHash(sha512, input, hash, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 用指定编码方式验证SHA512是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="hash">待验证SHA512</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifySha512(this string input, string hash, Encoding encoding)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                return VerifyHash(sha512, input, hash, encoding);
            }
        }

        #endregion

        #region HMACSHA1


        /// <summary>
        /// 以UTF8的编码方式获取字符串的HMACSHA1哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>哈希后的HMACSHA1值</returns>
        public static string ToBase64Hmacsha1(this string input, string key)
        {
            return ToBase64Hmacsha1(input, key, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的编码方式获取字符串的HMAC哈希值
        /// </summary>
        /// <param name="input">待哈希字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希后的HMAC值</returns>
        public static string ToBase64Hmacsha1(this string input, string key, Encoding encoding)
        {
            using (HMACSHA1 hmac = new HMACSHA1(encoding.GetBytes(key)))
            {
                return GetBase64Hash(hmac, input, encoding);
            }
        }

        /// <summary>
        /// 用UTF8的编码方式验证HMACSHA1是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="key">密钥</param>
        /// <param name="hash">待验证HMACSHA1值</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifyHmac(this string input, string key, string hash)
        {
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                return VerifyHash(hmac, input, hash, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 用指定编码方式验证HMACSHA1是否正确
        /// </summary>
        /// <param name="input">待验证数据</param>
        /// <param name="key">密钥</param>
        /// <param name="hash">待验证HMACSHA1</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>验证结果,匹配则返回true,不区分大小写</returns>
        public static bool VerifyHmac(this string input, string key, string hash, Encoding encoding)
        {
            using (HMACSHA1 hmac = new HMACSHA1(encoding.GetBytes(key)))
            {
                return VerifyHash(hmac, input, hash, encoding);
            }
        }

        #endregion

        /**
　　 *
　　 * hmac_md5口令加密算法
　　 * 
　　 */
        public static string hmac_md5(this string timespan, string password)
        {
            byte[] b_tmp;
            byte[] b_tmp1;
            if (password == null)
            {
                return null;
            }
            byte[] digest = new byte[512];
            byte[] k_ipad = new byte[64];
            byte[] k_opad = new byte[64];
            byte[] source = Encoding.UTF8.GetBytes(password);
            MD5 shainner = new MD5CryptoServiceProvider();
            for (int i = 0; i < 64; i++)
            {
                k_ipad[i] = 0 ^ 0x36;
                k_opad[i] = 0 ^ 0x5c;
            }
            try
            {
                if (source.Length > 64)
                {
                    shainner = new MD5CryptoServiceProvider();
                    source = shainner.ComputeHash(source);
                }
                for (int i = 0; i < source.Length; i++)
                {
                    k_ipad[i] = (byte)(source[i] ^ 0x36);
                    k_opad[i] = (byte)(source[i] ^ 0x5c);
                }
                b_tmp1 = System.Text.ASCIIEncoding.ASCII.GetBytes(timespan);
                b_tmp = adding(k_ipad, b_tmp1);
                shainner = new MD5CryptoServiceProvider();
                digest = shainner.ComputeHash(b_tmp);
                b_tmp = adding(k_opad, digest);
                shainner = new MD5CryptoServiceProvider();
                digest = shainner.ComputeHash(b_tmp);
                return digest.byteToHexStr();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /**
     　　 *
     　　 * 填充byte
     　　 * 
     　　 */
        static byte[] adding(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }

        public static string byteToHexStr(this byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
    }
}
