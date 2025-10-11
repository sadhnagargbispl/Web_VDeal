using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;

public partial class PhonePayAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Amount = "1"; // Assume the amount is passed as a query string parameter
            if (!string.IsNullOrEmpty(Amount))
            {
                ProcessPayment(Amount);
            }
        }
    }
    private void ProcessPayment(string Amount)
    {
        var TransId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        var ortxnid = "MT" + TransId;
        var merchantUserId = "MUID" + TransId;
        string msg = string.Empty;
        string url = string.Empty;
        var req = new PhonePayPayementgateway.PGRequest
        {
            merchantTransactionId = ortxnid,
            merchantUserId = merchantUserId,
            amount = (long)(Convert.ToDecimal(Amount) * 100),
            paymentInstrument = new PhonePayPayementgateway.paymentInstrument
            {
                type = "PAY_PAGE"
            },
            mobileNumber = Convert.ToString("8955817887")
        };

        string SALTKEY = "";
        int SALTKEYINDEX = 1;
        string apiUrl = "";
        bool IsTest = false; // Assume this is coming from your configuration

        if (IsTest )
        {
            req.merchantId = "M22DNJUWSBZDC";
            req.redirectUrl = "https://utility.stanveeservices.com/Addfund/PhonepayPaymentResponse";
            req.redirectMode = "POST";
            req.callbackUrl = "https://utility.stanveeservices.com/Addfund/PhonepayPaymentResponse";
            SALTKEY = "11a38631-782d-488b-91af-eeea041b85bd";
            apiUrl = "https://api.phonepe.com/apis/hermes/pg/v1/pay";
        }
        else
        {
            req.merchantId = "PGTESTPAYUAT";
            req.redirectUrl = "https://localhost:44332/Addfund/PhonepayPaymentResponse";
            req.redirectMode = "POST";
            req.callbackUrl = "https://localhost:44332/Addfund/PhonepayPaymentResponse";
            SALTKEY = "099eb0cd-02cf-4e2a-8aca-3e6c6aff0399";
            apiUrl = "https://api-preprod.phonepe.com/apis/hermes/pg/v1/pay";

        }

        var payload = JsonConvert.SerializeObject(req);
        var payloadbytes = Encoding.UTF8.GetBytes(payload);
        var payloadencode = Convert.ToBase64String(payloadbytes);
        var STRSHA256 = payloadencode + "/pg/v1/pay" + SALTKEY;
        string ChecksumValue;
        using (SHA256 sha = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(STRSHA256);
            byte[] hashBytes = sha.ComputeHash(inputBytes);
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            ChecksumValue = hashString + "###" + SALTKEYINDEX;
        }

        var Jobj = new PhonePayPayementgateway.jsonbodyreq
        {
            request = payloadencode
        };

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        try
        {
            HttpWebRequest request = WebRequest.Create(apiUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Headers.Add("X-VERIFY", ChecksumValue);
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(JsonConvert.SerializeObject(Jobj));
            }

            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            using (var responseReader = new StreamReader(webResponse.GetResponseStream()))
            {
                string responseBody = responseReader.ReadToEnd();
                var pgres = JsonConvert.DeserializeObject<PhonePayPayementgateway.PAYMENT_INITIATED_response>(responseBody);

                if (pgres.Success)
                {
                    var Rurl = pgres.Data.InstrumentResponse.RedirectInfo.Url;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<PaymentRequest>");
                    sb.AppendLine($"<Formno>{Convert.ToString(Session["Formno"])}</Formno>");
                    sb.AppendLine($"<Amount>{Amount}</Amount>");
                    sb.AppendLine($"<OrderID>{ortxnid}</OrderID>");
                    sb.AppendLine("<TID></TID>");
                    sb.AppendLine($"<TDate>{DateTime.Now:yyyy-MM-dd}</TDate>");
                    sb.AppendLine("<RS></RS>");
                    sb.AppendLine("<RSV></RSV>");
                    sb.AppendLine("<MandatoryField></MandatoryField>");
                    sb.AppendLine("<Status></Status>");
                    sb.AppendLine("<ID>0</ID>");
                    sb.AppendLine("</PaymentRequest>");

                    //PaymentHandler paymentDb = new PaymentHandler();
                    //DataSet ds = paymentDb.SavePaymentRequest(sb.ToString());

                    //// Redirect to the payment URL
                    //Response.Redirect(Rurl);
                }
                else
                {
                    msg = "Something went wrong";
                }
            }
        }
        catch (Exception ex)
        {
            msg = "Something went wrong: " + ex.Message;
        }

        // Optionally, handle messages or URL redirections as needed
        // Response.Write($"<script>alert('{msg}');</script>");
    }
}