using System;
using System.Collections.Generic;
using System.Text;

namespace Planckx.Sdk.Client {
    public class RequestException : Exception {
        public int Code { get; }

        public RequestException(int code, string message) : base(message) {
            Code = code;
        }
        public RequestException(int code, string message, Exception innerException) : base(message, innerException) {
            Code = code;
        }
        public RequestException(string message, Exception innerException) : base(message, innerException) {
            Code = 0;
        }
        public override string ToString() => string.Concat(Code, " : ", Message);
    }
}
