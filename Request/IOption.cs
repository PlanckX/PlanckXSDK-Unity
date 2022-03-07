using System;
using System.Collections.Generic;
using System.Text;

namespace Planckx.Sdk.Request {
    public interface IOption {
        string ApiKey { get; set; }
        string SecretKey { get; set; }
        string RestHost { get; }
        string RequestUrl { get; set; }
        bool Signature { get; }
    }
}
