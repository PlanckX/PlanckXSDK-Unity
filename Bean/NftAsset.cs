using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Planckx.Sdk.Bean {
    public class NftAsset {
        [JsonProperty("gameId")] public string GameId { get; set; }
        [JsonProperty("nftName")] public string Name { get; set; }
        [JsonProperty("nftId")] public string NftId { get; set; }
        [JsonProperty("nftType")] public int Type { get; set; }
        [JsonProperty("authorAddress")] public string CreatorAddress { get; set; }
        [JsonProperty("ownerAddress")] public string OwnerAddress { get; set; }
        [JsonProperty("nftContent")] public string Content { get; set; }
        [JsonProperty("nftDescription")] public string Description { get; set; }
        [JsonProperty("tokenId")] public string TokenId { get; set; }
        [JsonProperty("nftData")] public string Addition { get; set; }
        public override string ToString() => new StringBuilder()
                .Append("gameId=").Append(GameId)
                .Append("name=").Append(Name)
                .Append("type=").Append(Type)
                .Append("creatorAddress=").Append(CreatorAddress)
                .Append("ownerAddress=").Append(OwnerAddress)
                .Append("content=").Append(Content)
                .Append("description=").Append(Description)
                .Append("tokenId=").Append(TokenId)
                .Append("addition=").Append(Addition)
                .ToString();
    }
}
