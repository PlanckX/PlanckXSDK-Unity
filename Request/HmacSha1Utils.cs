using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Planckx.Sdk.Request {
    internal class HmacSha1Utils {
        private const string AccessKey = "access_key";
        private const string TimestampUTC = "timestamp";
        private const string Nonce = "nonce";
        private const string Sign = "sign";

        internal static void AddSign(HttpHeaders headers, IList<KeyValuePair<string, string>> paramList, IOption option) {
            string timeMills = Convert.ToString(GenerateMills());
            string uuid = System.Guid.NewGuid().ToString();

            headers.Add(AccessKey, option.ApiKey);
            headers.Add(TimestampUTC, timeMills);
            headers.Add(Nonce, uuid);

            List<string> signList = new List<string> {
                string.Concat(AccessKey, "=", option.ApiKey),
                string.Concat(TimestampUTC, "=", timeMills),
                string.Concat(Nonce, "=", uuid)
            };

            if (paramList != null && paramList.Count > 0) {
                foreach (KeyValuePair<string, string> keyValue in paramList) {
                    signList.Add(string.Concat(keyValue.Key, "=", keyValue.Value));
                }
            }

            signList.Sort();
            var stringX = new StringBuilder();

            foreach (string section in signList) {
                stringX.Append(section).Append("&");
            }

            string sign = HmacSha1ForBase64(stringX.ToString().Substring(0, stringX.Length - 1), option.SecretKey);
            headers.Add(Sign, sign);
        }


        private static string HmacSha1ForBase64(string text, string key) {
            //HMACSHA1
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(key);

            byte[] dataBuffer = Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        private static long GenerateMills() {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }
    }
}
