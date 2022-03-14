using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

namespace PlanckX {
    internal class PlanckXNetworking {
        private const string AccessKey = "access_key";
        private const string TimestampUTC = "timestamp";
        private const string Nonce = "nonce";
        private const string Sign = "sign";
        private const string ALL_NFT_URL = "NFT/list/";
        private const string PLAYER_NFT_URL = "NFT/player/list/";
        private const string TOKEN_NFT_URL = "NFT/token/";
        private const string API_ADDR = "https://api.planckx.io/v1/api/sdk/";
        public string APIKey {
            get;
            set;
        }
        public string SecretKey {
            get;
            set;
        }
        public PlanckXNetworking(string APIKey, string SecretKey) {
            this.APIKey = APIKey;
            this.SecretKey = SecretKey;
        }
        private static string HmacSha1ForBase64(string text, string key) {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(key);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }
        private UnityWebRequest AddSign(UnityWebRequest req, List<KeyValuePair<string, string>> paramList = null) {
            string timeMills = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
            string uuid = System.Guid.NewGuid().ToString();
            req.SetRequestHeader(AccessKey, APIKey);
            req.SetRequestHeader(TimestampUTC, timeMills);
            req.SetRequestHeader(Nonce, uuid);
            List<string> signList = new List<string> {
                string.Concat(AccessKey, "=", APIKey),
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
            for (int i = 0; i < signList.Count; ++i) {
                if (i == signList.Count - 1) {
                    stringX.Append(signList[i]);
                } else {
                    stringX.Append(signList[i]).Append("&");
                }
            }
            string sign = HmacSha1ForBase64(stringX.ToString(), SecretKey);
            req.SetRequestHeader(Sign, sign);
            return req;
        }
        public UnityWebRequest GenerateCheckBindRequest(string playerId) {
            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(SecretKey)) {
                Debug.LogError("APIKey or SecretKey is null");
                return null;
            }
            var req = UnityWebRequest.Get(API_ADDR + "checkBind/" + playerId);
            req = AddSign(req);
            return req;
        }
        public UnityWebRequest GenerateGetAllGameNFTRequest() {
            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(SecretKey)) {
                Debug.LogError("APIKey or SecretKey is null");
                return null;
            }
            var req = UnityWebRequest.Get(API_ADDR + ALL_NFT_URL);
            req = AddSign(req);
            return req;
        }
        public UnityWebRequest GenerateGetNFTByPlayerIdRequest(string playerId) {
            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(SecretKey)) {
                Debug.LogError("APIKey or SecretKey is null");
                return null;
            }
            var req = UnityWebRequest.Get(API_ADDR + PLAYER_NFT_URL + playerId);
            req = AddSign(req);
            return req;
        }
        public UnityWebRequest GenerateGetNFTByTokenIdRequest(string tokenId) {
            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(SecretKey)) {
                Debug.LogError("APIKey or SecretKey is null");
                return null;
            }
            var req = UnityWebRequest.Get(API_ADDR + TOKEN_NFT_URL + tokenId);
            req = AddSign(req);
            return req;
        }
    }
}
