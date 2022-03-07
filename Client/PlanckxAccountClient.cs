using Planckx.Sdk.Bean;
using Planckx.Sdk.Request;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static Planckx.Sdk.Bean.ResponseCode;

namespace Planckx.Sdk.Client {
    public class PlanckxAccountClient {
        private const string ACCOUNT_BIND_URL = "/checkBind/";
        private IOption Option;

        public PlanckxAccountClient(IOption option) {
            Option = option ?? throw new ArgumentNullException(nameof(option));
        }
        public async Task<Tuple<ResponseEnum, CheckBind>> BindState(string playerId) {
            Option.RequestUrl = string.Concat(ACCOUNT_BIND_URL, playerId);
            string result;
            try {
                result = await HttpClientUtils.Get(Option);
            } catch (AggregateException ae) {
                ResponseEnum respEnum = ResponseEnum.Forbidden;
                ae.Handle((x) => {
                    if (x is RequestException re) {
                        respEnum = (ResponseEnum)Enum.Parse(typeof(ResponseEnum), re.Code.ToString());
                        throw re;
                    }
                    return false;
                });
                return new Tuple<ResponseEnum, CheckBind>(respEnum, null);
            }
            ResponseEnum responseEnum = HttpClientUtils.ParseResponse(result, out string dataJson);
            CheckBind accountBind = null;
            if (responseEnum == ResponseEnum.Successful && !string.IsNullOrEmpty(dataJson)) {
                accountBind = JsonConvert.DeserializeObject<CheckBind>(dataJson.ToString());
            }
            return new Tuple<ResponseEnum, CheckBind>(responseEnum, accountBind);
        }
    }
}
