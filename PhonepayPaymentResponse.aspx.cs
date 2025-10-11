using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class PhonepayPaymentResponse : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        PhonepayPaymentResponseS();
    }
    public void PhonepayPaymentResponseS()
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string requestBody;
        string Amount;
        using (System.IO.Stream body = Request.InputStream)
        {
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            requestBody = reader.ReadToEnd();
        }

        // Parse the callback string as a query string
        NameValueCollection queryParams = System.Web.HttpUtility.ParseQueryString(requestBody);
        // Extract individual parameters from the callback
        string paymentStatus = queryParams["code"];
        string merchantId = queryParams["merchantId"];
        string transactionId = queryParams["transactionId"];
        Amount = queryParams["amount"];
        string providerReferenceId = queryParams["providerReferenceId"];
        string checksum = queryParams["checksum"];
        string SALTKEY = "";
        string result = "";
        int SALTKEYINDEX = 1;
        decimal Reqamount = Convert.ToDecimal(Convert.ToDecimal(Amount) / 100);
        string IsTest = "True"; // Assuming IsTest is fetched from configuration

        string apiUrl = "";

        if (IsTest == "True")
        {
            SALTKEY = "11a38631-782d-488b-91af-eeea041b85bd";
            SALTKEYINDEX = 1;
            apiUrl = $"https://api.phonepe.com/apis/hermes/pg/v1/status/{merchantId}/{transactionId}";
        }
        else
        {
            SALTKEY = "099eb0cd-02cf-4e2a-8aca-3e6c6aff0399";
            SALTKEYINDEX = 1;
            apiUrl = $"https://api-preprod.phonepe.com/apis/pg-sandbox/pg/v1/status/{merchantId}/{transactionId}";
        }

        string STRSHA256 = $"/pg/v1/status/{merchantId}/{transactionId}{SALTKEY}";
        string ChecksumValue = "";

        using (SHA256 sha = SHA256.Create())
        {
            // Convert the input string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(STRSHA256);
            // Compute the hash
            byte[] hashBytes = sha.ComputeHash(inputBytes);
            // Convert the hash bytes to a hexadecimal string
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            ChecksumValue = hashString + "###" + SALTKEYINDEX;
        }

        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("X-VERIFY", ChecksumValue);
            httpWebRequest.Headers.Add("X-MERCHANT-ID", merchantId);

            string sql_req = "insert into ApiReqResponse(ReqID,Formno,Request,RectimeStamp,AMount,merchantId,transactionId,providerReferenceId,checksum)";
            sql_req += " Values('" + sResult.Trim() + "','" + Convert.ToInt32(Session["FormNo"]) + "','" + apiUrl + "',getdate(),'" + Reqamount + "','" + merchantId + "',";
            sql_req += "'" + transactionId + "','" + providerReferenceId + "','" + ChecksumValue + "')";
            int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
            result = reader.ReadToEnd();

        }
        catch (Exception ex)
        {
            // Handle exception as needed
            result = "";
        }

        DataSet dsstatus = ConvertJsonStringToDataSet(result);

        if (dsstatus.Tables.Count > 0)
        {
            string PCStatus = "";

            if (Convert.ToBoolean(dsstatus.Tables[0].Rows[0]["success"]) && Convert.ToString(dsstatus.Tables[0].Rows[0]["code"]).ToUpper() == "PAYMENT_SUCCESS")
            {
                PCStatus = "PAID";
            }
            else if (Convert.ToBoolean(dsstatus.Tables[0].Rows[0]["success"]) && Convert.ToString(dsstatus.Tables[0].Rows[0]["code"]).ToUpper() == "PAYMENT_PENDING")
            {
                PCStatus = "PENDING";
            }
            else if (!Convert.ToBoolean(dsstatus.Tables[0].Rows[0]["success"]) && Convert.ToString(dsstatus.Tables[0].Rows[0]["code"]).ToUpper() == "PAYMENT_ERROR")
            {
                PCStatus = "FAILED";
            }

            StringBuilder sb = new StringBuilder();
            string uniqueRefID = "";
            string tDate = string.Empty;
            string rs = string.Empty;
            string rrnNo = string.Empty;
            string rsv = string.Empty;
            string mandatoryField = string.Empty;
            string requestedId = string.Empty;

            if (PCStatus.ToUpper() == "PAID")
            {
                sb.AppendLine("<PaymentRequest>");
                sb.AppendLine("<Amount>" + Convert.ToString(Reqamount) + "</Amount>");
                sb.AppendLine("<OrderID>" + transactionId + "</OrderID>");
                sb.AppendLine("<TID>" + uniqueRefID + "</TID>");
                sb.AppendLine("<TransactionDate>" + tDate + "</TransactionDate>");
                sb.AppendLine("<RS>" + rs + "</RS>");
                sb.AppendLine("<RSV>" + rsv + "</RSV>");
                sb.AppendLine("<RRN>" + rrnNo + "</RRN>");
                sb.AppendLine("<MandatoryField>" + mandatoryField + "</MandatoryField>");
                sb.AppendLine("<Status>SUCCESS</Status>");
                sb.AppendLine("<Response>" + result.Replace("&", "&amp;") + "</Response>");
                sb.AppendLine("<ID>1</ID>");
                sb.AppendLine("</PaymentRequest>");

                string sql_res = "Update ApiReqResponse  Set Response = '" + result.Trim() + "',TxnHash = '" + sb.ToString() + "' Where ReqID = '" + sResult.Trim() + "' ";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                //paymentDb = new PaymentHandler();
                //DataSet ds = paymentDb.SavePaymentRequest(sb.ToString());

                string scrname = "Fund transferred successfully in your wallet with transaction no. " + Convert.ToString(transactionId);
                Response.Redirect("ActivationThankYou.aspx");
            }
            else
            {
                sb.AppendLine("<PaymentRequest>");
                sb.AppendLine("<Amount>" + Convert.ToString(Reqamount) + "</Amount>");
                sb.AppendLine("<TransactionDate>" + tDate + "</TransactionDate>");
                sb.AppendLine("<OrderID>" + transactionId + "</OrderID>");
                sb.AppendLine("<Status>" + PCStatus + "</Status>");
                sb.AppendLine("<Response>" + result.Replace("&", "&amp;") + "</Response>");
                sb.AppendLine("<ID>1</ID>");
                sb.AppendLine("</PaymentRequest>");

                //paymentDb = new PaymentHandler();
                //DataSet ds = paymentDb.SavePaymentRequest(sb.ToString());

                string scrname = "FAILED";
                Response.Redirect("index.aspx");
            }
        }

        Response.Redirect("index.aspx");
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
}