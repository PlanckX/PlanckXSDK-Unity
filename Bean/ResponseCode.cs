using System;
using System.Collections.Generic;
using System.Text;

namespace Planckx.Sdk.Bean {
    public static class ResponseCode {
        public enum ResponseEnum {
            Successful = 200,
            Created = 201,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            APIUnFound = 500,
        }
    }
}
