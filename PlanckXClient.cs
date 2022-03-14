using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PlanckX {
    public class PlanckXClient : MonoBehaviour {
        [SerializeField] private string APIKey = "";
        [SerializeField] private string SecretKey = "";
        private PlanckX.PlanckXNetworking plxNetworking;
        private void Awake() {
            if (plxNetworking == null) {
                plxNetworking = new PlanckXNetworking("", "");
            }
            if (!string.IsNullOrEmpty(APIKey) && !string.IsNullOrEmpty(SecretKey)) {
                SetAPIKeyAndSecretKey(APIKey, SecretKey);
            }
        }
        public void SetAPIKeyAndSecretKey(string APIKey, string SecretKey) {
            if (plxNetworking == null) {
                plxNetworking = new PlanckXNetworking(APIKey, SecretKey);
            }
            this.APIKey = APIKey;
            this.SecretKey = SecretKey;
            plxNetworking.APIKey = APIKey;
            plxNetworking.SecretKey = SecretKey;
        }
        private IEnumerator SendRequest(UnityWebRequest req, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError) {
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success) {
                onSuccess?.Invoke(req);
            } else {
                onError?.Invoke(req);
            }
        }
        public void CheckBindAsync(string playerId, Action<bool, string> onSuccess, Action<UnityWebRequest> onError = null) {
            var bindReq = plxNetworking.GenerateCheckBindRequest(playerId);
            if (bindReq == null) {
                return;
            }
            StartCoroutine(SendRequest(bindReq, (req) => {
                var jsonRes = JObject.Parse(req.downloadHandler.text);
                onSuccess((bool)jsonRes["data"]["isBind"], (string)jsonRes["data"]["openUrl"]);
            }, onError));
        }
        public void GetAllGameNFTAsync(Action<List<NftAsset>> onSuccess, Action<UnityWebRequest> onError = null) {
            var allNFTReq = plxNetworking.GenerateGetAllGameNFTRequest();
            if (allNFTReq == null) {
                return;
            }
            StartCoroutine(SendRequest(allNFTReq, (req) => {
                var jsonRes = JObject.Parse(req.downloadHandler.text);
                var r = JsonConvert.DeserializeObject<List<NftAsset>>(jsonRes["data"].ToString());
                onSuccess?.Invoke(r);
            }, onError));
        }
        public void GetNFTByPlayerIdAsync(string playerId, Action<List<NftAsset>> onSuccess, Action<UnityWebRequest> onError = null) {
            var nftByPlayerIdReq = plxNetworking.GenerateGetNFTByPlayerIdRequest(playerId);
            if (nftByPlayerIdReq == null) {
                return;
            }
            StartCoroutine(SendRequest(nftByPlayerIdReq, (req) => {
                var jsonRes = JObject.Parse(req.downloadHandler.text);
                var r = JsonConvert.DeserializeObject<List<NftAsset>>(jsonRes["data"].ToString());
                onSuccess?.Invoke(r);
            }, onError));
        }
        public void GetNFTByTokenIdAsync(string tokenId, Action<NftAsset> onSuccess, Action<UnityWebRequest> onError = null) {
            var nftByTokenIdReq = plxNetworking.GenerateGetNFTByTokenIdRequest(tokenId);
            if (nftByTokenIdReq == null) {
                return;
            }
            StartCoroutine(SendRequest(nftByTokenIdReq, (req) => {
                var jsonRes = JObject.Parse(req.downloadHandler.text);
                var r = JsonConvert.DeserializeObject<NftAsset>(jsonRes["data"].ToString());
                onSuccess?.Invoke(r);
            }, onError));
        }
    }
}
