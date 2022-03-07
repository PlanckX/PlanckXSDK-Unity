using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Planckx.Sdk.Bean {
    public class CheckBind {
        [JsonProperty("isBind")] public bool Bind { get; set; }
        [JsonProperty("openUrl")] public string AuthAddress { get; set; }
        public override string ToString() => string.Concat("Bind=", Bind, ", AuthAddress=", AuthAddress);
    }
}
