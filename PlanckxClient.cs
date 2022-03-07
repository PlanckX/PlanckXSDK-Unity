using Planckx.Sdk.Client;
using Planckx.Sdk.Request;

namespace Planckx {
    public static class PlanckxClient {
        public static PlanckxAccountClient AccountClient(string apiKey, string secretKey) {
            IOption option = new PlanckxOption() {
                ApiKey = apiKey,
                SecretKey = secretKey
            };
            return new PlanckxAccountClient(option);
        }
        public static PlanckxNftClient NftClient(string apiKey, string secretKey) {
            IOption option = new PlanckxOption() {
                ApiKey = apiKey,
                SecretKey = secretKey
            };
            return new PlanckxNftClient(option);
        }
    }
}
