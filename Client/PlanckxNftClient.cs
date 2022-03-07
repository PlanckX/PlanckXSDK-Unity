using Planckx.Sdk.Bean;
using Planckx.Sdk.Request;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static Planckx.Sdk.Bean.ResponseCode;

namespace Planckx.Sdk.Client {
    public class PlanckxNftClient {
        private const string ALL_NFT_URL = "/NFT/list";
        private const string PLAYER_NFT_URL = "/NFT/player/list/";
        private const string TOKEN_NFT_URL = "/NFT/token/";
        private IOption Option;

        public PlanckxNftClient(IOption option) {
            Option = option ?? throw new ArgumentNullException(nameof(option));
        }
        public async Task<(ResponseEnum, List<NftAsset>)> AllNfts() {
            List<NftAsset> resultNfts = null;
            Option.RequestUrl = ALL_NFT_URL;
            string result = await this.Execution(Option);
            ResponseEnum responseEnum = HttpClientUtils.ParseResponse(result, out string dataJson);
            if (responseEnum == ResponseEnum.Successful && !string.IsNullOrEmpty(dataJson)) {
                resultNfts = JsonConvert.DeserializeObject<List<NftAsset>>(dataJson.ToString());
            }
            return (responseEnum, resultNfts);
        }
        public async Task<(ResponseEnum, List<NftAsset>)> NftByPlayer(string playerId) {
            List<NftAsset> resultNfts = null;
            Option.RequestUrl = string.Concat(PLAYER_NFT_URL, playerId);
            string result = await this.Execution(Option);
            ResponseEnum responseEnum = HttpClientUtils.ParseResponse(result, out string dataJson);
            if (responseEnum == ResponseEnum.Successful && !string.IsNullOrEmpty(dataJson)) {
                resultNfts = JsonConvert.DeserializeObject<List<NftAsset>>(dataJson.ToString());
            }
            return (responseEnum, resultNfts);
        }
        public async Task<(ResponseEnum, NftAsset)> NftByTokenId(string tokenId) {
            NftAsset resultNft = null;
            Option.RequestUrl = string.Concat(TOKEN_NFT_URL, tokenId);
            string result = await this.Execution(Option);
            ResponseEnum responseEnum = HttpClientUtils.ParseResponse(result, out string dataJson);
            if (responseEnum == ResponseEnum.Successful && !string.IsNullOrEmpty(dataJson)) {
                resultNft = JsonConvert.DeserializeObject<NftAsset>(dataJson.ToString());
            }
            return (responseEnum, resultNft);
        }
        private async Task<string> Execution(IOption option) {
            try {
                string result = await HttpClientUtils.Get(option);
                return result;
            } catch (AggregateException ae) {
                ae.Handle((x) => {
                    if (x is RequestException re) {
                        throw re;
                    }
                    return false;
                });
            }
            return null;
        }
    }
}
