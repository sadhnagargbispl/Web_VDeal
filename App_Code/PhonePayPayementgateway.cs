using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PhonePayPayementgateway
/// </summary>
public class PhonePayPayementgateway
{

    public class PGRequest
    {
        public string merchantId { get; set; }
        public string merchantTransactionId { get; set; }
        public long amount { get; set; }
        public string merchantUserId { get; set; }
        public string redirectUrl { get; set; }
        public string redirectMode { get; set; }
        public string callbackUrl { get; set; }
        public paymentInstrument paymentInstrument { get; set; }
        public string mobileNumber { get; set; }
    }

    public class paymentInstrument
    {
        public string type { get; set; }
    }
    public class jsonbodyreq
    {
        public string request { get; set; }
    }

    public class PAYMENT_INITIATED_response
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }
    public class Data
    {
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("merchantTransactionId")]
        public string MerchantTransactionId { get; set; }

        [JsonProperty("instrumentResponse")]
        public InstrumentResponse InstrumentResponse { get; set; }
    }
    public class InstrumentResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("redirectInfo")]
        public RedirectInfo RedirectInfo { get; set; }
    }

    public class RedirectInfo
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

}
