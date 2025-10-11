<%@ WebHandler Language="C#" Class="QrCodehenr" %>

using System;
using System.Web;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AjaxControlToolkit;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.SqlServer.Server;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web.UI;
using System.Web.UI.WebControls;
public class QrCodehenr : IHttpHandler
{
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string appId = "a4tcpLeAUW9Zaogy";
    string appSecret = "165d35b8912071d978c8c14911207131";
    private string url = "https://ccpayment.com/ccpayment/v2/getUserCoinAsset";
    string callbackurlsurl = "https://ccpayment.com/ccpayment/v2/getUserDepositRecordList";
    string userId = "";
    string UserformNoV = "";
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            var obj = new DAL();
            var Dt = new DataTable();
            var paymentId = context.Request["payment_id"];
            userId = context.Request["idno"];
            UserformNoV = GetFormNo_1(userId);
            double TokenAMount = Convert.ToDouble(context.Request["tokenamount"]);
            double reqamount = Convert.ToDouble(context.Request["reqamount"]);
            var json = GetResponseCheckloop(paymentId, userId, UserformNoV, TokenAMount,reqamount);
            context.Response.Write(json);
        }
        catch (Exception ex)
        {

        }
    }
    private string GetResponseCheckloop(string paymentId, string userid, string formNoV, double TokenAMount, double reqamount)
    {
        string response = string.Empty;
        M_QRCode obg = new M_QRCode();
        CallbacksRes objnes = new CallbacksRes();
        string strs = "";
        string strCheck = "";
        string tokenName = "";
        decimal FinalAmount;
        DataTable dtCheck = new DataTable();
        DataSet ds = new DataSet();
        DataSet data = new DataSet();
        string str = "";
        string statusApi = "";
        int resultRR = 0;
        DataSet dsLogin = new DataSet();
        DataTable dsLoginToAddress = new DataTable();
        string responseString = string.Empty;
        string toWalletAddress = "";
        string orderIV = "";
        int i = 0;
        try
        {
            string vi = paymentId;
            string Balance = CheckBalance(vi, userId, formNoV);
            if (Balance != "0")
            {
                string responseS = SendRequest(appId, appSecret, callbackurlsurl, userId, formNoV);
                double originalValue;

                try
                {
                    data = ConvertJsonStringToDataSet(responseS);
                    DataView dv = new DataView(data.Tables[2]);
                    dv.RowFilter = "toAddress = '" + paymentId.Trim() + "'";
                    dsLoginToAddress = dv.ToTable();
                    if (dsLoginToAddress.Rows.Count > 0)
                    {
                        foreach (DataRow row in dsLoginToAddress.Rows)
                        {
                            toWalletAddress = row["toAddress"].ToString();
                            string fromWalletAddress = row["fromAddress"].ToString();
                            originalValue = Convert.ToDouble(row["amount"]);
                            FinalAmount = (decimal)(originalValue / Math.Pow(10, 18));
                            decimal accurateTotalAmount = decimal.Parse(FinalAmount.ToString(), NumberStyles.Float);
                            string hash = row["txId"].ToString();
                            string contractAddress = row["contract"].ToString().Trim();
                            tokenName = row["coinSymbol"].ToString();
                            orderIV = row["recordId"].ToString();
                            if (tokenName.ToUpper() == "SUMMIT-SOL")
                            {
                                strCheck = " EXEC Sp_CheckTxnHAsh__ '" + hash.ToString() + "'";
                                dtCheck = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strCheck).Tables[0];
                                if (dtCheck.Rows.Count == 0)
                                {
                                    decimal allowedDifference = 5m;
                                    bool result = IsAmountLessThanOrEqualToBalance(formNoV,Convert.ToDecimal(TokenAMount),decimal.Parse(originalValue.ToString(), NumberStyles.Float), allowedDifference);
                                    if (result)
                                    {
                                        string txnInsert = "INSERT INTO TrnvoucherTxnHash(Formno, From_walletAddress, To_walletAddress, SuMMITAMOUNT, Txnhash, To_PrivateKey,ReqFrom)";
                                        txnInsert += " VALUES('" + formNoV + "','" + fromWalletAddress + "','" + toWalletAddress + "',";
                                        txnInsert += "'" + decimal.Parse(originalValue.ToString(), NumberStyles.Float) + "','" + hash + "','','SUMMIT-SOL')";
                                        int xI = 0;
                                        try
                                        {
                                            xI = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, txnInsert);
                                        }
                                        catch (SqlException Ex)
                                        {
                                            if (Ex.Number == 2627) // Unique constraint violation error number
                                            {
                                                xI = 0;
                                            }
                                        }

                                        if (xI > 0)
                                        {
                                            strCheck = " EXEC Sp_CheckTxnHAsh '" + hash.ToString() + "'";
                                            dtCheck = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strCheck).Tables[0];
                                            if (dtCheck.Rows.Count != 0)
                                            {
                                                if (contractAddress == "A3vBwL3Pd6nkBqVo87D6TLWYG4ekWuDeqCoLFeyvRMcb")
                                                {
                                                    strs = string.Empty;
                                                    strs = "INSERT INTO ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData, TypeB, FromID, ToID) ";
                                                    strs += "VALUES('" + formNoV + "','" + orderIV + "','" + toWalletAddress + "','" + toWalletAddress + "',";
                                                    strs += "'" + callbackurlsurl + "','" + str + "','" + statusApi + "',GETDATE(),'Re-Transcation SUMMITSOL','" + hash + "',";
                                                    strs += "'" + decimal.Parse(originalValue.ToString(), NumberStyles.Float) + "','','QrCodeSUMMITSOL','" + toWalletAddress + "','" + fromWalletAddress + "');";
                                                    strs += " EXEC sp_FundAddUpdate_FundUpdatejoshmart '" + formNoV + "','" + decimal.Parse(reqamount.ToString(), NumberStyles.Float) + "',";
                                                    strs += "'" + orderIV + "','" + toWalletAddress + "','" + hash + "','" + fromWalletAddress + "','" + toWalletAddress + "';";
                                                    string queryStr = "";
                                                    queryStr = " BEGIN TRY BEGIN TRANSACTION " + strs + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                                                    int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, queryStr);
                                                    if (x > 0)
                                                    {
                                                        resultRR = 1;
                                                        string sqlResToken = "";
                                                        string tokenResponse = "successful";
                                                        if (tokenResponse.ToUpper().Trim() == "SUCCESSFUL")
                                                        {
                                                            sqlResToken = "UPDATE TrnvoucherTxnHash SET Is_Pay = 'A', Update_DATe = GETDATE() WHERE To_walletAddress = '" + toWalletAddress.Trim() + "' AND Is_Pay = 'P'";
                                                            int xResToken = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sqlResToken);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strs = string.Empty;
                                                    strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                                    strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                                    int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                                }
                                            }
                                            else
                                            {
                                                strs = string.Empty;
                                                strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                                strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                                int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                            }


                                        }
                                    }
                                    else
                                    {
                                        decimal remaining = Convert.ToDecimal(TokenAMount) - Convert.ToDecimal(originalValue);
                                        objnes.Response = "TokenRe";
                                        objnes.balance = originalValue.ToString();
                                        objnes.Rembalance = remaining.ToString();
                                        response = JsonConvert.SerializeObject(objnes);
                                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Have Deposited " + Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) + " Please Deposit Remaining Token " + remaining + "');", true);
                                    }
                                }
                                else
                                {
                                    strs = string.Empty;
                                    strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                    strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                    int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                }
                                //string txnInsert = "INSERT INTO TrnvoucherTxnHash(Formno, From_walletAddress, To_walletAddress, SuMMITAMOUNT, Txnhash, To_PrivateKey,ReqFrom)";
                                //txnInsert += " VALUES('" + formNoV + "','" + fromWalletAddress + "','" + toWalletAddress + "',";
                                //txnInsert += "'" + decimal.Parse(originalValue.ToString(), NumberStyles.Float) + "','" + hash + "','','SUMMIT-SOL')";
                                //int xI = 0;
                                //try
                                //{
                                //    xI = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, txnInsert);
                                //}
                                //catch (SqlException Ex)
                                //{
                                //    if (Ex.Number == 2627) // Unique constraint violation error number
                                //    {
                                //        xI = 0;
                                //    }
                                //}
                                //if (xI > 0)
                                //{
                                //    strCheck = " EXEC Sp_CheckTxnHAsh__ '" + hash.ToString() + "'";
                                //    dtCheck = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strCheck).Tables[0];
                                //    if (dtCheck.Rows.Count != 0)
                                //    {
                                //        if (contractAddress == "A3vBwL3Pd6nkBqVo87D6TLWYG4ekWuDeqCoLFeyvRMcb")
                                //        {
                                //            strs = string.Empty;
                                //            strs = "INSERT INTO ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData, TypeB, FromID, ToID) ";
                                //            strs += "VALUES('" + formNoV + "','" + orderIV + "','" + toWalletAddress + "','" + toWalletAddress + "',";
                                //            strs += "'" + callbackurlsurl + "','" + str + "','" + statusApi + "',GETDATE(),'Re-Transcation SUMMITSOL','" + hash + "',";
                                //            strs += "'" + decimal.Parse(originalValue.ToString(), NumberStyles.Float) + "','','QrCodeSUMMITSOL','" + toWalletAddress + "','" + fromWalletAddress + "');";
                                //            strs += " EXEC sp_FundAddUpdate_FundUpdatejoshmart '" + formNoV + "','" + decimal.Parse(originalValue.ToString(), NumberStyles.Float) + "',";
                                //            strs += "'" + orderIV + "','" + toWalletAddress + "','" + hash + "','" + fromWalletAddress + "','" + toWalletAddress + "';";
                                //            string queryStr = "";
                                //            queryStr = " BEGIN TRY BEGIN TRANSACTION " + strs + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                                //            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, queryStr);
                                //            if (x > 0)
                                //            {
                                //                resultRR = 1;
                                //                string sqlResToken = "";
                                //                string tokenResponse = "successful";
                                //                if (tokenResponse.ToUpper().Trim() == "SUCCESSFUL")
                                //                {
                                //                    sqlResToken = "UPDATE TrnvoucherTxnHash SET Is_Pay = 'A', Update_DATe = GETDATE() WHERE To_walletAddress = '" + toWalletAddress.Trim() + "' AND Is_Pay = 'P'";
                                //                    int xResToken = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sqlResToken);
                                //                }
                                //            }
                                //        }
                                //        else
                                //        {
                                //            strs = string.Empty;
                                //            strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                //            strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                //            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        strs = string.Empty;
                                //        strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                //        strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                //        int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                //    }
                                //}
                            }
                        }
                    }
                    if (resultRR > 0)
                    {
                        //string message = "Payment Successfully Added in Wallet.!";
                        //string urlStr = "pointWallet.aspx";
                        //string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                        objnes.Response = "Success";
                        response = JsonConvert.SerializeObject(objnes);
                    }

                }
                catch (Exception ex)
                {
                    string errorQry = "";
                    string errorMsg = "";

                    try
                    {
                        errorMsg = ex.Message;
                    }
                    catch (Exception eXX) { }
                    finally
                    {
                        if (errorMsg != "") { } else { errorMsg = ""; }
                    }

                    errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) ";
                    errorQry += "VALUES('" + errorMsg + "', GETDATE(),'" + callbackurlsurl + "','" + toWalletAddress.Trim() + "','" + str + "','" + formNoV + "')";
                    int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
                    objnes.Response = "Failed";
                    response = JsonConvert.SerializeObject(objnes);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later.');", true);
                }
            }
            else
            {
                objnes.Response = "Failed";
                response = JsonConvert.SerializeObject(objnes);
            }
        }
        catch (Exception ex)
        {
            objnes.Response = "Failed";
            response = JsonConvert.SerializeObject(objnes);
        }

        return response;
    }
    public bool IsAmountLessThanOrEqualToBalance(string formno, decimal amount, decimal apiBalance, decimal allowedDifference)
    {
        decimal tabAmount = 0;
        decimal difference = 0;
        string s = "";
        DataTable dt;
        dt = new DataTable();

        s = "select top 1 * from TrnTokenAmount where formno = '" + formno + "' order by id desc";
        dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];
        if (dt.Rows.Count > 0)
        {
            tabAmount = Convert.ToDecimal(dt.Rows[0]["TokenAmount"]);
            difference = Math.Abs(amount - apiBalance);
        }
        return difference <= allowedDifference;
    }
    public string SendRequest(string appId, string appSecret, string url, string userId, string formNoV)
    {
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        var content = new
        {
            userId = userId
        };
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string body = Newtonsoft.Json.JsonConvert.SerializeObject(content);
        string signText = appId + timestamp;
        if (body.Length != 2)
        {
            signText += body;
        }
        else
        {
            body = string.Empty;
        }
        string serverSign;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
        {
            serverSign = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(signText))).Replace("-", "").ToLower();
        }
        string responseContent = string.Empty;
        using (var httpClient = new HttpClient())
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("Appid", appId);
                httpClient.DefaultRequestHeaders.Add("Sign", serverSign);
                httpClient.DefaultRequestHeaders.Add("Timestamp", timestamp.ToString());
                var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
                string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType)";
                sqlReq += " VALUES('" + sResult.Trim() + "','" + formNoV.Trim() + "','" + url.Trim() + "','" + body.Trim() + "','SUMMITADDRESSLIST')";
                int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).GetAwaiter().GetResult();
                responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + responseContent.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND ApiType = 'SUMMITADDRESSLIST' ";
                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));
            }
            catch (Exception ex)
            {
                string errorQry = "";
                string errorMsg = "";

                try
                {
                    errorMsg = ex.Message;
                }
                catch (Exception eXX) { }
                finally
                {
                    if (errorMsg != "") { } else { errorMsg = ""; }
                }

                errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno,ForType) ";
                errorQry += "VALUES('" + errorMsg + "', GETDATE(),'" + url + "','" + userId.Trim() + "','" + responseContent + "','" + formNoV + "','SUMMITADDRESSLIST')";
                int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
            }
        }

        return responseContent;
    }

    public string CheckBalance(string address, string UserID, string formNoV)
    {
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        string responseString = string.Empty;
        string availableAmount = string.Empty;
        var content = new
        {
            userId = UserID,
            coinId = 1978
        };
        string jsonBody = JsonConvert.SerializeObject(content);
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string signText = appId + timestamp;
        if (!string.IsNullOrEmpty(jsonBody))
        {
            signText += jsonBody;
        }
        string serverSign = GenerateSignature(signText, appSecret);
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Appid", appId);
            client.DefaultRequestHeaders.Add("Sign", serverSign);
            client.DefaultRequestHeaders.Add("Timestamp", timestamp.ToString());
            StringContent httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType)";
            sqlReq += " VALUES('" + sResult.Trim() + "','" + formNoV.Trim() + "','" + url.Trim() + "','" + jsonBody.Trim() + "','SUMMITBALANCECHECK')";
            int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));
            try
            {
                HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
                string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + responseString.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND ApiType = 'SUMMITBALANCECHECK' ";
                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));
                var result = JsonConvert.DeserializeObject<dynamic>(responseString);
                if (result != null && result.code != null && result.code == 10000)
                {
                    if (result.data != null && result.data.asset != null && result.data.asset.available != null)
                    {
                        availableAmount = result.data.asset.available.ToString();
                    }
                    else
                    {
                        availableAmount = "0";
                    }
                }
                else
                {
                    availableAmount = "0";
                }
            }
            catch (Exception ex)
            {
                string errorQry = "";
                string errorMsg = "";

                try
                {
                    errorMsg = ex.Message;
                }
                catch (Exception eXX) { }
                finally
                {
                    if (errorMsg != "") { } else { errorMsg = ""; }
                }

                errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno,ForType) ";
                errorQry += "VALUES('" + errorMsg + "', GETDATE(),'" + url + "','" + userId.Trim() + "','" + responseString + "','" + formNoV + "','SUMMITBALANCECHECK')";
                int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later.');", true);
                availableAmount = "0";
            }
        }

        return availableAmount;
    }
    private string GetFormNo_1(string Uname)
    {
        try
        {
            // Initialize the DAL object with the connection string
            var obj = new DAL();
            // SQL query to fetch the data from the table
            string str = "select * from m_membermaster where idno = '" + Uname + "'";
            DataTable dt = new DataTable();
            string FormNo = "0";
            // Get data from the database
            dt = obj.GetData(str);
            // Check if any rows were returned
            if (dt.Rows.Count > 0)
            {
                FormNo = Convert.ToInt32(dt.Rows[0]["FormNo"]).ToString();
            }

            // Return the FormNo
            return FormNo;
        }
        catch (Exception ex)
        {
            // Handle the exception as necessary
            return "0"; // Return default value in case of an error
        }
    }
    private static string GenerateSignature(string data, string key)
    {
        using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        DataSet ds = new DataSet();
        XmlDocument xd = new XmlDocument();

        // Add root node to the JSON string for proper XML structure
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";

        // Deserialize JSON string to XML
        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);

        // Read the XML into the DataSet
        ds.ReadXml(new XmlNodeReader(xd));

        return ds;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
