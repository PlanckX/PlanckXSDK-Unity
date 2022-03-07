using Planckx.Sdk.Bean;
using Planckx.Sdk.Client;
using System;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using static Planckx.Sdk.Bean.ResponseCode;
using System.Net;

namespace Planckx.Sdk.Request {
    internal class HttpClientUtils {
        private static readonly HttpClient HttpClientInstance;

        static HttpClientUtils() {
            HttpClientInstance = new HttpClient();
        }
        public static async Task<string> Get(IOption option, IList<KeyValuePair<string, string>> formData = null) {
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(option.RequestUrl),
                Method = HttpMethod.Get,
            };
            HmacSha1Utils.AddSign(request.Headers, formData, option);
            HttpResponseMessage resp = await HttpClientInstance.SendAsync(request);
            try {
                resp.EnsureSuccessStatusCode();
                string token = await resp.Content.ReadAsStringAsync();
                return token;
            } catch (HttpRequestException e) {
                if (resp != null) {
                    throw new RequestException((int)resp.StatusCode, resp.StatusCode.ToString(), e);
                }
                throw new RequestException(e.Message, e);
            }
        }
        public static ResponseCode.ResponseEnum ParseResponse(string jsonResult, out string dataJson) {
            var jo = JObject.Parse(jsonResult);
            ResponseEnum response;
            string code = (string)jo["code"];
            if (string.IsNullOrEmpty(code)) { //issue: 让后端返回值里的code字段不能为null
                response = ResponseEnum.APIUnFound;
            } else {
                response = (ResponseEnum)Enum.Parse(typeof(ResponseEnum), code);
            }
            dataJson = null;
            if (response == ResponseEnum.Successful) {
                dataJson = jo["data"].ToString();
            }
            return response;
        }
    }
}
