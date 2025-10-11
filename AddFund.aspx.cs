using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class AddFund : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnProceedToPay));
            this.BtnconfirmBtotton.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnconfirmBtotton));
            this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
            this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    Session["OtpCount"] = 0;
                    Session["OtpTime"] = null;
                    Session["Retry"] = null;
                    Session["OTP_"] = null;
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    public string GenerateRandomStringJoining(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = string.Empty;

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }

        return sResult;
    }
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        string scrname = "";
        if (TxtUserID.Text.ToString() == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter User ID.!');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        if (TxtPassword.Text.ToString() == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Password.!');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        FUND_LOGIN_CHECK();
    }
    private void FUND_LOGIN_CHECK()
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (var httpClient = new HttpClient())
            {
                var hostname = "sbgrewards.io";
                var referCode = TxtUserID.Text;
                var password = TxtPassword.Text;
                Session["UserPassowrd"] = password;
                var postData = "{\"refer_code\":\"" + referCode + "\",\"password\":\"" + password + "\"}";
                StringContent content = new StringContent(postData, Encoding.UTF8, "application/json");
                var byteArray = Encoding.UTF8.GetBytes(postData);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sbgrewards.ai/api/sbglogin");
                request.Headers.Host = hostname;
                request.Content = content;
                request.Content.Headers.ContentLength = byteArray.Length;
                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType) ";
                sql_req += "VALUES ('" + sResult + "', '" + Convert.ToInt32(Session["FormNo"]) + "', 'https://sbgrewards.ai/api/sbglogin', '" + postData + "', 'LOGIN')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_req));
                var response = httpClient.SendAsync(request).Result;
                // Log after receiving the response
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + apiResponse.Trim() + "' WHERE ReqID = '" + sResult + "' AND ApiType = 'LOGIN'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
                    if (x_res > 0)
                    {
                        DataSet Ds = new DataSet();
                        string sql_User = "";
                        Ds = ConvertJsonStringToDataSet(apiResponse);
                        if (Ds.Tables[0].Rows[0]["success"].ToString().ToUpper() == "TRUE")
                        {
                            sql_User = "Insert into UserInfo(TrueStatus,FormNo,UserId,FirstName,DOJ,Email,MobileNo,City,IsActive,kitid,noofid,kitstatus,coupon,shoppoint,Apistatus,ewallet,Request,Response,ApiType)";
                            sql_User += "VALUES('" + Ds.Tables[0].Rows[0]["success"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','" + Ds.Tables[1].Rows[0]["UserId"] + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["FirstName"] + "','" + Ds.Tables[1].Rows[0]["DOJ"] + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["Email"] + "','" + Ds.Tables[1].Rows[0]["MobileNo"] + "','" + Ds.Tables[1].Rows[0]["City"] + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["IsActive"] + "','" + Ds.Tables[1].Rows[0]["kitid"] + "','" + Ds.Tables[1].Rows[0]["noofid"] + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["kitstatus"] + "','" + Ds.Tables[1].Rows[0]["coupon"] + "','" + Ds.Tables[1].Rows[0]["shoppoint"] + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["status"] + "','" + Ds.Tables[1].Rows[0]["ewallet"] + "','https://sbgrewards.ai/api/sbglogin','" + apiResponse.Trim() + "','LOGIN')";
                            int x_User = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_User));
                            if (x_User > 0)
                            {
                                HdnFirstName.Value = Ds.Tables[1].Rows[0]["FirstName"].ToString();
                                Hdndoj.Value = Ds.Tables[1].Rows[0]["DOJ"].ToString();
                                HdnEmail.Value = Ds.Tables[1].Rows[0]["Email"].ToString();
                                HdnMobileNo.Value = Ds.Tables[1].Rows[0]["MobileNo"].ToString();
                                HdnCity.Value = Ds.Tables[1].Rows[0]["City"].ToString();
                                HdnIsActive.Value = Ds.Tables[1].Rows[0]["IsActive"].ToString();
                                Hdnkitid.Value = Ds.Tables[1].Rows[0]["kitid"].ToString();
                                Hdnnoofid.Value = Ds.Tables[1].Rows[0]["noofid"].ToString();
                                Hdnkitstatus.Value = Ds.Tables[1].Rows[0]["kitstatus"].ToString();
                                Hdncoupon.Value = Ds.Tables[1].Rows[0]["coupon"].ToString();
                                Hdnshoppoint.Value = Ds.Tables[1].Rows[0]["shoppoint"].ToString();
                                DivLoginCheck.Visible = false;
                                DivConfirm.Visible = true;

                                FUND_WALLET_CHECK(HdnFirstName.Value, Hdndoj.Value, HdnEmail.Value, HdnMobileNo.Value, HdnCity.Value, HdnIsActive.Value, Hdnkitid.Value, Hdnnoofid.Value, Hdnkitstatus.Value,
                                    Hdncoupon.Value, Hdnshoppoint.Value);
                            }
                            else
                            {
                                DivLoginCheck.Visible = true;
                                DivConfirm.Visible = false;
                            }
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Login SuccessFully.!');location.replace('MM_Voucher.aspx');", true);
                            return;
                        }
                        else
                        {
                            sql_User = "Insert into UserInfo(TrueStatus,FormNo,UserId,FirstName,DOJ,Email,MobileNo,City,IsActive,kitid,noofid,kitstatus,coupon,shoppoint,Apistatus,ewallet,Request,Response,ApiType)";
                            sql_User += "VALUES('" + Ds.Tables[0].Rows[0]["success"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','" + referCode + "',";
                            sql_User += "'','','','0','','','0','','','','','','0','https://sbgrewards.ai/api/sbglogin','" + apiResponse.Trim() + "','LOGIN')";
                            int x_User = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_User));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Not SuccessFully.!');location.replace('AddFund.aspx');", true);
                            return;
                        }

                    }
                }
                else
                {
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + response.StatusCode + "' WHERE ReqID = '" + sResult + "' AND ApiType = 'LOGIN'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + httpEx.Message + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'LOGIN'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
        }
        catch (Exception ex)
        {
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + ex.Message + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'LOGIN'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
        }
    }
    private void FUND_WALLET_CHECK(string FirstName, string doj, string Email, string MobileNo, string City, string IsActive, string kitid, string noofid, string kitstatus, string coupon, string shoppoint)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (var httpClient = new HttpClient())
            {
                var hostname = "sbgrewards.io";
                var referCode = TxtUserID.Text;
                var password = TxtPassword.Text;
                Session["UserPassowrd"] = password;
                var postData = "{\"refer_code\":\"" + referCode + "\",\"password\":\"" + password + "\"}";
                StringContent content = new StringContent(postData, Encoding.UTF8, "application/json");
                var byteArray = Encoding.UTF8.GetBytes(postData);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sbgrewards.ai/api/checkWallet");
                request.Headers.Host = hostname;
                request.Content = content;
                request.Content.Headers.ContentLength = byteArray.Length;
                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType) ";
                sql_req += "VALUES ('" + sResult + "', '" + Convert.ToInt32(Session["FormNo"]) + "', 'https://sbgrewards.ai/api/checkWallet', '" + postData + "', 'CHECKWALLET')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_req));
                var response = httpClient.SendAsync(request).Result;
                // Log after receiving the response
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + apiResponse.Trim() + "' WHERE ReqID = '" + sResult + "' AND ApiType = 'CHECKWALLET'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
                    if (x_res > 0)
                    {
                        DataSet Ds = new DataSet();
                        string sql_User = "";
                        Ds = ConvertJsonStringToDataSet(apiResponse);
                        if (Ds.Tables[0].Rows[0]["success"].ToString().ToUpper() == "TRUE")
                        {
                            sql_User = "Insert into UserInfo(TrueStatus,FormNo,UserId,FirstName,DOJ,Email,MobileNo,City,IsActive,kitid,noofid,kitstatus,coupon,shoppoint,Apistatus,ewallet,Request,Response,ApiType)";
                            sql_User += "VALUES('" + Ds.Tables[0].Rows[0]["success"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','" + Ds.Tables[1].Rows[0]["UserId"] + "',";
                            sql_User += "'" + FirstName + "','" + doj + "','" + Email + "','" + MobileNo + "','" + City + "',";
                            sql_User += "'" + IsActive + "','" + kitid + "','" + noofid + "','" + kitstatus + "','" + coupon + "','" + shoppoint + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["msg"] + "','" + Ds.Tables[1].Rows[0]["ewallet"] + "','https://sbgrewards.ai/api/checkWallet','" + apiResponse.Trim() + "','CHECKWALLET')";
                            int x_User = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_User));
                            // LblGiftWalletBala.Text = Ds.Tables[1].Rows[0]["ewallet"].ToString();
                            LblGiftWalletBala.Text = (Convert.ToDecimal(Ds.Tables[1].Rows[0]["ewallet"]) * 85).ToString();
                            HdnAmount.Value = (Convert.ToDecimal(Ds.Tables[1].Rows[0]["ewallet"]) * 85).ToString();
                            Session["ServiceWallet"] = LblGiftWalletBala.Text;
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Login SuccessFully.!');location.replace('MM_Voucher.aspx');", true);
                            return;
                        }
                        else
                        {
                            sql_User = "Insert into UserInfo(TrueStatus,FormNo,UserId,FirstName,DOJ,Email,MobileNo,City,IsActive,kitid,noofid,kitstatus,coupon,shoppoint,Apistatus,ewallet,Request,Response,ApiType)";
                            sql_User += "VALUES('" + Ds.Tables[0].Rows[0]["success"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','" + Ds.Tables[1].Rows[0]["UserId"] + "',";
                            sql_User += "'" + FirstName + "','" + doj + "','" + Email + "','" + MobileNo + "','" + City + "',";
                            sql_User += "'" + IsActive + "','" + kitid + "','" + noofid + "','" + kitstatus + "','" + coupon + "','" + shoppoint + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["msg"] + "','" + Ds.Tables[1].Rows[0]["ewallet"] + "','https://sbgrewards.ai/api/checkWallet','" + apiResponse.Trim() + "','CHECKWALLET')";
                            int x_User = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_User));
                            LblGiftWalletBala.Text = "0";
                            return;
                        }

                    }
                }
                else
                {
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + response.StatusCode + "' WHERE ReqID = '" + sResult + "' AND ApiType = 'CHECKWALLET'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + httpEx.Message + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'CHECKWALLET'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
        }
        catch (Exception ex)
        {
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + ex.Message + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'CHECKWALLET'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
        }
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
    private void FUND_DEDUCT_WALLET_CHECK(string FirstName, string doj, string Email, string MobileNo, string City, string IsActive, string kitid, string noofid, string kitstatus, string coupon, string shoppoint, string Amount)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (var httpClient = new HttpClient())
            {
                var hostname = "sbgrewards.io";
                var referCode = TxtUserID.Text;
                var password = Session["UserPassowrd"];
                var TxnData = GenerateRandomStringJoining(6);
                decimal amount = Convert.ToDecimal(TxtReqAmount.Text);
                decimal conversionRate = 85;
                decimal result = amount / conversionRate;
                var AmountApiSent = result;
                var actionApi = "dr";
                var remark = "Gift Wallet";
                var postData = "{\"refer_code\":\"" + referCode + "\",\"password\":\"" + password + "\",\"TxnData\":\"" + TxnData + "\",";
                postData += "\"amount\":\"" + AmountApiSent + "\",\"action\":\"" + actionApi + "\",\"remark\":\"" + remark + "\"}";
                StringContent content = new StringContent(postData, Encoding.UTF8, "application/json");
                var byteArray = Encoding.UTF8.GetBytes(postData);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sbgrewards.ai/api/deduct_wallet");
                request.Headers.Host = hostname;
                request.Content = content;
                request.Content.Headers.ContentLength = byteArray.Length;
                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType) ";
                sql_req += "VALUES ('" + sResult + "', '" + Convert.ToInt32(Session["FormNo"]) + "', 'https://sbgrewards.ai/api/deduct_wallet', '" + postData + "', 'DEDUCT_WALLET')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_req));
                var response = httpClient.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + apiResponse.Trim() + "' WHERE ReqID = '" + sResult + "' AND ApiType = 'DEDUCT_WALLET'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
                    if (x_res > 0)
                    {
                        DataSet Ds = new DataSet();
                        string sql_User = "";
                        Ds = ConvertJsonStringToDataSet(apiResponse);
                        if (Ds.Tables[0].Rows[0]["success"].ToString().ToUpper() == "TRUE")
                        {
                            sql_User = "Insert into UserInfo(TrueStatus,FormNo,UserId,FirstName,DOJ,Email,MobileNo,City,IsActive,kitid,noofid,";
                            sql_User += "kitstatus,coupon,shoppoint,Apistatus,ewallet,Request,Response,ApiType,voucherno,ReqAmount)";
                            sql_User += "VALUES('" + Ds.Tables[0].Rows[0]["success"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','" + Ds.Tables[1].Rows[0]["ReferCode"] + "',";
                            sql_User += "'" + FirstName + "','" + doj + "','" + Email + "','" + MobileNo + "','" + City + "',";
                            sql_User += "'" + IsActive + "','" + kitid + "','" + noofid + "','" + kitstatus + "','" + coupon + "','" + shoppoint + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["msg"] + "','" + Ds.Tables[1].Rows[0]["Amount"] + "',";
                            sql_User += "'https://sbgrewards.ai/api/deduct_wallet','" + apiResponse.Trim() + "','DEDUCT_WALLET','" + Ds.Tables[1].Rows[0]["voucherno"] + "','" + AmountApiSent + "')";
                            int x_User = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_User));
                            if (x_User > 0)
                            {
                                String Trn_Sql = "";
                                int TrnSql = 0;
                                string Remark = "";
                                string voucherNo = "";
                                string originalString = Ds.Tables[1].Rows[0]["ReferCode"].ToString();
                                string sql = "select IsNull(Max(VoucherNo+1),1) as VoucherNo from TrnVoucher";
                                DataTable dt = new DataTable();
                                dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql).Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    voucherNo = dt.Rows[0]["VoucherNo"].ToString();
                                }
                                string sql_CrnSess = " select Max(SEssid) as SessID from m_SessnMaster ";
                                DataTable dt_CrnSess = new DataTable();
                                dt_CrnSess = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_CrnSess).Tables[0];
                                if (dt_CrnSess.Rows.Count > 0)
                                {
                                    Session["CurrentSessnUpdate"] = dt_CrnSess.Rows[0]["SessID"].ToString();
                                }
                                if (Convert.ToDecimal(Ds.Tables[1].Rows[0]["Amount"]) > 0)
                                {
                                    Remark = "Gift Wallet Credited Against VoucherNo : " + Ds.Tables[1].Rows[0]["voucherno"] + "(" + Ds.Tables[1].Rows[0]["ReferCode"] + ") ";
                                    Trn_Sql = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId,FromType,Response)";
                                    Trn_Sql += "values('" + voucherNo + "',Getdate(),0,'" + Convert.ToInt32(Session["FormNo"]) + "', '" + Convert.ToDecimal(TxtReqAmount.Text) * 2 + "', ";
                                    Trn_Sql += "'" + Remark + "','Credit/" + Ds.Tables[1].Rows[0]["ReferCode"] + "','S',GetDate(),'C',Convert(Varchar,GetDate(),112),";
                                    Trn_Sql += "" + Session["CurrentSessnUpdate"] + ",'1','A','" + apiResponse + "')";

                                    Trn_Sql += "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values('1','" + FirstName + "','Deduct Wallet Api','Gift Wallet Credit Transfer',";
                                    Trn_Sql += "'" + Remark + "',Getdate(),'" + originalString.Replace("SBG", "") + "')";

                                    TrnSql = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, Trn_Sql));
                                    if (TrnSql > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Fund Added Successfully.!');location.replace('GiftWallet.aspx');", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Insufficient Balance In Wallet.!');location.replace('AddFund.aspx');", true);
                                    return;
                                }

                            }
                        }
                        else
                        {
                            sql_User = "Insert into UserInfo(TrueStatus,FormNo,UserId,FirstName,DOJ,Email,MobileNo,City,IsActive,kitid,noofid,";
                            sql_User += "kitstatus,coupon,shoppoint,Apistatus,ewallet,Request,Response,ApiType,voucherno,ReqAmount)";
                            sql_User += "VALUES('" + Ds.Tables[0].Rows[0]["success"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','',";
                            sql_User += "'" + FirstName + "','" + doj + "','" + Email + "','" + MobileNo + "','" + City + "',";
                            sql_User += "'" + IsActive + "','" + kitid + "','" + noofid + "','" + kitstatus + "','" + coupon + "','" + shoppoint + "',";
                            sql_User += "'" + Ds.Tables[1].Rows[0]["msg"] + "','0','https://sbgrewards.ai/api/deduct_wallet','" + apiResponse.Trim() + "','DEDUCT_WALLET','0','" + AmountApiSent + "')";
                            int x_User = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_User));
                            return;
                        }
                    }
                }
                else
                {
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + response.StatusCode + "' WHERE ReqID = '" + sResult + "' AND ApiType = 'DEDUCT_WALLET'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + httpEx.Message + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'DEDUCT_WALLET'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
        }
        catch (Exception ex)
        {
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + ex.Message + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'CHECKWALLET'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
        }
    }
    protected void BtnconfirmBtotton_Click(object sender, EventArgs e)
    {
        decimal LblGiftWallet;
        if (!decimal.TryParse(LblGiftWalletBala.Text, out LblGiftWallet) || LblGiftWallet == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Insufficient Balance In Wallet.!');location.replace('AddFund.aspx');", true);
            return;
        }
        decimal reqAmount;
        if (!decimal.TryParse(TxtReqAmount.Text, out reqAmount) || reqAmount == 0)
        {
            string scrName = "<script language='javascript'>alert('Enter a valid Request Amount!');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
            TxtReqAmount.Text = "0";
            return;
        }
        if (Convert.ToDecimal(TxtReqAmount.Text) > Convert.ToDecimal(Session["ServiceWallet"]))
        {
            string scrName = "<script language='javascript'>alert('Insufficient Balance In Wallet.!');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
            TxtReqAmount.Text = "0";
            return;
        }
        else
        {
            string msg = "OK";
            if (msg.ToUpper() == "OK")
            {
                int OTP_ = 0;
                Random Rs = new Random();
                OTP_ = Rs.Next(100001, 999999);
                // TxtOtp.Text = OTP_.ToString();

                if (Session["OTP_"] == null)
                {
                    if (SendMail(OTP_.ToString()) == true)
                    {
                        Session["OtpTime"] = DateTime.Now.AddMinutes(5);
                        Session["Retry"] = "1";
                        Session["OTP_"] = OTP_;
                        int i = 0;
                        string R = "";
                        R = "INSERT INTO AdminLogin(UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID,ReqType) " +
                            "VALUES ('0', '', '" + TxtOtp.Text + "', '0', '" + OTP_ + "', GETDATE(), '" + OTP_ + "', '" + HdnEmail.Value.ToString().Trim() + "','joshmart')";
                        i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, R));

                        if (i > 0)
                        {
                            LblUserEmail.Text = HdnEmail.Value.ToString();
                            divotp.Visible = true;
                            BtnconfirmBtotton.Visible = false;
                            BtnOtp.Visible = true;
                            ResendOtp.Visible = true;

                            string scrName = "<script language='javascript'>alert('OTP Send On Mail');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                            return;
                        }
                        else
                        {
                            string scrName = "<script language='javascript'>alert('Try Later');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                            return;
                        }
                    }
                    else
                    {
                        string scrName = "<script language='javascript'>alert('OTP Try Later');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                        return;
                    }
                }
                else
                {
                    divotp.Visible = true;
                    BtnconfirmBtotton.Visible = false;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = true;
                }
            }

            //FUND_DEDUCT_WALLET_CHECK(HdnFirstName.Value, Hdndoj.Value, HdnEmail.Value, HdnMobileNo.Value, HdnCity.Value, HdnIsActive.Value, Hdnkitid.Value, Hdnnoofid.Value,
            //    Hdnkitstatus.Value, Hdncoupon.Value, Hdnshoppoint.Value, HdnAmount.Value);
        }
    }
    public bool SendMail(string otp)
    {
        try
        {
            string strMsg = "";
            string emailAddress = HdnEmail.Value.ToString().Trim();
            var sendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            var sendTo = new System.Net.Mail.MailAddress(emailAddress);
            var myMessage = new System.Net.Mail.MailMessage(sendFrom, sendTo);

            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                     "<tr>" +
                     "<td>" +
                     "Your OTP for Add Fund is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                     "</table>";

            myMessage.Subject = "Thanks For Connecting!!!";
            myMessage.Body = strMsg;
            myMessage.IsBodyHtml = true;

            var smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(myMessage);
            TxtReqAmount.Enabled = false;
            int c = 0;
            return true;
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            return false;
        }
    }
    protected void BtnOtp_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            string emaail = "";
            DataTable Dt = new DataTable();
            string TransPassw = TxtOtp.Text.Trim();
            DataTable Dt1 = new DataTable();

            Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;

            if (Session["OTP_"].ToString() == TxtOtp.Text)
            {
                string str = "Select TOP 1 * from AdminLogin as a where EmailID='" + HdnEmail.Value.ToString().Trim() + "' ";
                str += "and emailotp='" + TxtOtp.Text.Trim() + "' AND ReqType = 'joshmart' ORDER BY AID DESC";
                Dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                if (Dt1.Rows.Count > 0)
                {
                    FUND_DEDUCT_WALLET_CHECK(HdnFirstName.Value, Hdndoj.Value, HdnEmail.Value, HdnMobileNo.Value, HdnCity.Value, HdnIsActive.Value, Hdnkitid.Value, Hdnnoofid.Value,
                        Hdnkitstatus.Value, Hdncoupon.Value, Hdnshoppoint.Value, HdnAmount.Value);
                }
            }
            else
            {
                TxtOtp.Text = "";

                if (Convert.ToInt32(Session["OtpCount"]) >= 3)
                {
                    Session["OtpCount"] = 0;
                    scrname = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\\n Please generate OTP again.');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('You have tried 3 times with invalid OTP.\\n Please generate OTP again.');", true);
                    ResendOtp.Visible = true;
                    BtnOtp.Visible = true;
                    BtnOtp.Enabled = false;
                    divotp.Visible = false;
                }
                else
                {
                    scrname = "<script language='javascript'>alert('Invalid OTP.');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Invalid OTP.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    protected void ResendOtp_Click(object sender, EventArgs e)
    {
        try
        {
            Session["OTP_"] = "";
            int OTP_ = 0;
            Random Rs = new Random();
            OTP_ = Rs.Next(100001, 999999);

            if (SendMail(OTP_.ToString()) == true)
            {
                string Emailid = HdnEmail.Value.ToString();
                string membername = "";
                string mobileno = "0";
                string Sms = "";
                int i = 0;
                string R = "";
                R = "INSERT INTO AdminLogin(UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID,ReqType) " +
                    "VALUES ('0', '" + membername + "', '" + TxtOtp.Text + "', '" + mobileno + "', '" + OTP_ + "', getdate(), '" + OTP_ + "', '" + HdnEmail.Value.ToString().Trim() + "','joshmart')";
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, R));
                if (i > 0)
                {
                    LblUserEmail.Text = HdnEmail.Value.ToString();
                    Session["OTP_"] = OTP_;
                    divotp.Visible = true;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = true;
                    BtnOtp.Enabled = true;
                    string scrname = "<script language='javascript'>alert('OTP Send On Mail');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
                else
                {
                    string scrname = "<script language='javascript'>alert('Try Later');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
            }
            else
            {
                string scrname = "<script language='javascript'>alert('OTP Try Later');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}