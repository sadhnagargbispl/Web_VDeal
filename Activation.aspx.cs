using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Policy;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Xml;
using System.Activities.Expressions;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Net.NetworkInformation;
using System.Globalization;
using AjaxControlToolkit;
using DocumentFormat.OpenXml.Office2010.Excel;
using Irony.Parsing;
using System.ServiceModel.Activities;
using System.ComponentModel.Design;
//using Org.BouncyCastle.Utilities.Collections;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;


public partial class Activation : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    ModuleFunction objModuleFun = new ModuleFunction();
    string kitid_;
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            this.BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnProceedToPay));
            //this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
            //this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));
            if (!string.IsNullOrEmpty(Request["kitid"]))
            {
                kitid_ = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["kitid"]));
            }
            if (!IsPostBack)
            {
                Session["OtpCount"] = 0;
                Session["OtpTime"] = null;
                Session["Retry"] = null;
                Session["OTP_"] = null;
                HdnCheckTrnns.Value = GenerateRandomStringactive(6);
                FillKit(kitid_);
                string kitidString = kitid_;
                int kitid;
                if (int.TryParse(kitidString, out kitid))
                {
                    FillDis(kitid);
                }
            }

            //}
        }
        else
        {
            Response.Redirect("Login.aspx", false);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
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
    private void FillKit(string kitid)
    {
        try
        {
            DataSet ds = new DataSet();
            string sql = "Exec Sp_GetKitAmount '" + kitid + "' ";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblPakageAmount.Text = ds.Tables[0].Rows[0]["Kitamount"].ToString();
                lbljoinamount.Text = ds.Tables[0].Rows[0]["Joinamount"].ToString();
                LblPackageName.Text = ds.Tables[0].Rows[0]["Kitname"].ToString();
                LblUserName.Text = Session["MemName"].ToString();
                LblUserID.Text = Session["IDNo"].ToString();
                Img.Src = ds.Tables[0].Rows[0]["KitImg"].ToString();
                //TextBox1.Text = ds.Tables[0].Rows[0]["Kitamount"].ToString();
                TextBox1.Text = ds.Tables[0].Rows[0]["Joinamount"].ToString();
                Session["PayAmount"] = ds.Tables[0].Rows[0]["Joinamount"].ToString();
                if (DDLPaymode.SelectedValue == "1")
                {
                    TxtAmount.Text = "0";
                    GetBalance();
                    Check_Amount_Condition();

                }
                else
                {
                    TxtAmount.Text = Session["PayAmount"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void FillDis(int kitid)
    {
        try
        {
            DataSet ds = new DataSet();
            string sql = "Exec Sp_GetKitDisDetails '" + kitid + "' ";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Dis"].ToString() != "" || ds.Tables[0].Rows[0]["Dis"].ToString() == "")
                {
                    LblFoodDis.Text = ds.Tables[0].Rows[0]["Dis"].ToString();
                    LblFoodUse.Text = ds.Tables[0].Rows[0]["Uses"].ToString();
                    //lblhowtouse.Text = ds.Tables[0].Rows[0]["Howtouse"].ToString();
                    LblFoodTerms.Text = ds.Tables[0].Rows[0]["Trmscon"].ToString();
                    DivMDescription_Food.Visible = true;
                }
                else
                {
                    DivMDescription_Food.Visible = false;
                }
            }
            else
            {
                DivMDescription_Food.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GetBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = " Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','P')";
            dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                LblGiftWalletBala.Text = Convert.ToString(dt.Rows[0]["Balance"]);
            }
            else
            {
                LblGiftWalletBala.Text = "0";
            }
            Session["ServiceWallet"] = LblGiftWalletBala.Text;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string GenerateRandomStringactive(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        StringBuilder sResult = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sResult.Append(allowChrs[rdm.Next(0, allowChrs.Length)]);
        }

        return sResult.ToString();
    }
    protected void TxtAmount_TextChanged(object sender, EventArgs e)
    {
        //Check_Amount_Condtion();
    }
    private bool Check_Amount_Condition()
    {
        // Example values
        bool result = false;
        decimal giftWalletAmount = Convert.ToDecimal(Session["ServiceWallet"]);
        decimal pointWalletAmount = Convert.ToDecimal(Session["PointWallet"]);
        decimal requestAmount = Convert.ToDecimal(TextBox1.Text);
        //decimal maxGiftWalletUsage = requestAmount * 0.70m; // Updated: Maximum 70% of the request amount
        decimal maxGiftWalletUsage = requestAmount;
        decimal amountFromGiftWallet = Math.Min(maxGiftWalletUsage, giftWalletAmount);
        decimal remainingAmount = requestAmount - amountFromGiftWallet;
        decimal amountFromPointWallet = Math.Min(remainingAmount, pointWalletAmount);
        //bool isRequestFulfilled = (amountFromGiftWallet + amountFromPointWallet) >= requestAmount;
        bool isRequestFulfilled = (amountFromGiftWallet) >= requestAmount;
        // Update the text boxes with new messages
        TxtAmount.Text = Convert.ToDecimal(amountFromGiftWallet).ToString();
        //TxtPointWallet.Text = Convert.ToDecimal(amountFromPointWallet).ToString();
        if (isRequestFulfilled)
        {
            BtnProceedToPay.Visible = true;
            result = true;
        }
        else
        {
            string scrName = "<SCRIPT language='javascript'>alert('Your wallet does not have the balance required to meet the requested amount.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Point Wallet Error", scrName, false);
            BtnProceedToPay.Visible = false;
            result = false;
        }
        return result;
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
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        if (DDLPaymode.SelectedValue == "1")
        {
            Check_Amount_Condition();
        }
    }
    public bool SendMail(string otp)
    {
        try
        {
            string strMsg = "";
            string emailAddress = Session["EMail"].ToString().Trim();
            var sendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            var sendTo = new System.Net.Mail.MailAddress(emailAddress);
            var myMessage = new System.Net.Mail.MailMessage(sendFrom, sendTo);

            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                     "<tr>" +
                     "<td>" +
                     "Your OTP for ID Activation is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
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
            int c = 0;
            return true;
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            return false;
        }
    }
    private bool SendOTP(string mobileNo, string otp, string amount)
    {
        try
        {
            //string sms = "OTP for ₹" + amount + " transaction: " + otp + ". Keep it safe. Team SollyWood";
            string sms = "OTP for Rs. " + amount + " transaction: " + otp + ". Keep it safe. Team SollyWood";
            string baseurl = "";

            using (WebClient client = new WebClient())
            {
                Session["SmsId"] = "sollywoodcmd@gmail.com";
                Session["SmsPass"] = HttpUtility.UrlEncode("Ahmedabad1#");
                Session["ClientId"] = "SOLYWD";

                baseurl = "http://64.227.180.129/ApiSmsHttp?UserId=" + Session["SmsId"] +
                          "&pwd=" + Session["SmsPass"] +
                          "&Message=" + HttpUtility.UrlEncode(sms) +
                          "&Contacts=" + mobileNo +
                          "&SenderId=" + Session["ClientId"] +
                          "&ServiceName=SMSTRANS&MessageType=1";

                using (Stream data = client.OpenRead(baseurl))
                using (StreamReader reader = new StreamReader(data))
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("\"status\":\"success\""))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    protected void IdActivation()
    {
        try
        {
            string scrName = "";
            var updateEffect = 0;
            var strSql2 = "INSERT INTO Trnfundwithdrawcpanel (Transid, Rectimestamp) VALUES ('" + HdnCheckTrnns.Value + "', GETDATE())";
            updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strSql2));
            if (updateEffect > 0)
            {
                Check_Amount_Condition();
                if (Convert.ToDecimal(Session["ServiceWallet"]) >= Convert.ToDecimal(TxtAmount.Text))
                {
                    var billNo = GenerateRandomStringactive(6);
                    string sql = "";

                    sql = " EXEC Sp_ActivateMemberswastikNew '" + LblUserID.Text.Trim() + "','" + Convert.ToInt32(kitid_) + "','" + Convert.ToInt32(Session["Formno"]) + "',";
                    sql += "'" + billNo + "','" + TxtAmount.Text + "'";
                    try
                    {
                        var i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql));
                        if (i > 0)
                        {
                            GetBalance();
                            var scrName1 = $"<SCRIPT language='javascript'>alert('ID : {LblUserID.Text.Trim().ToUpper()}. Name : {LblUserName.Text}. Package Name : {LblPackageName.Text.Trim()}. Activated successfully!!');location.replace('index.aspx');</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName1, false);
                        }
                    }

                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    var scrName1 = "<SCRIPT language='javascript'>alert('Insufficient Balance In Cash Wallet.!');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName1, false);
                }
            }
            else
            {
                //Response.Redirect("MM_Voucher.aspx");
                Response.Redirect("Activation.aspx");
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred: {ex.Message}";
            var scrName = $"<SCRIPT language='javascript'>alert('{errorMessage}');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
        }
    }
    private bool IsValidID(string MemberID, string PinNo, out string Msg)
    {
        Msg = string.Empty;

        // Clean input
        MemberID = MemberID.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

        // Get new kit info
        string q = "SELECT KitId, KitName, MACAdrs, TopUpSeq, KitAmount, categoryid " +
                   "FROM MM_KitMaster " +
                   "WHERE CAST(KitAmount AS Numeric) = @PinNo  " +
                   "AND Allowtopup = 'Y' AND RowStatus = 'Y' AND activeStatus = 'Y' order by kitid";

        DataTable dtKit = SqlHelper.ExecuteDataset(
            ConfigurationManager.ConnectionStrings["constr1"].ConnectionString,
            CommandType.Text,
            q,
            new SqlParameter("@PinNo", Convert.ToDecimal(PinNo))
        ).Tables[0];

        if (dtKit.Rows.Count == 0)
        {
            Msg = "Package not found.";
            return false;
        }



        //// Check if this TopUpSeq already exists for this member
        //string qrCheckTopUp = "SELECT COUNT(*) FROM M_MemberMaster AS m " +
        //                      "INNER JOIN MM_KitMaster AS k ON m.KitId = k.KitId " +
        //                      "WHERE m.activestatus = 'Y' AND  m.Formno = @MemberID AND k.TopupSeq> @TopUpSeq";

        //int existingCount = Convert.ToInt32(SqlHelper.ExecuteScalar(
        //    ConfigurationManager.ConnectionStrings["constr1"].ConnectionString,
        //    CommandType.Text,
        //    qrCheckTopUp,
        //    new SqlParameter("@MemberID", MemberID),
        //    new SqlParameter("@TopUpSeq", newTopUpSeq)
        //));
        string Sql = @"
    Select 
        a.Formno,
        a.Idno,
        a.MemFirstName + ' ' + a.MemLastName as MemName,
        ISNULL(c.Idno, '') as SponsorId,
        ISNULL((c.MemFirstName + ' ' + c.MemLastName), ' ') as SponsorName,
        a.IsTopup,
        a.KitId,
        b.MACAdrs,
        b.TopUpSeq,
        a.LegNo,
        b.KitName,
        a.BV,
        b.BV as KBv,
        CASE 
            WHEN a.ActiveStatus = 'Y' 
            THEN REPLACE(CONVERT(VARCHAR, a.UpgradeDate, 106), ' ', '-') 
            ELSE '' 
        END as UpgradeDate,
        a.ActiveStatus,
        a.FLD1,
        a.Planid,
        a.IsBlock,
        a.Fld4
    from 
        MM_KitMaster as b,
        M_MemberMaster as a 
        Left Join M_MemberMaster as c on a.RefFormno = c.Formno
    where 
        a.KitId = b.KitId 
        and b.RowStatus = 'Y'  
     and a.formno = @MemberID
        and a.IsBlock = 'N'";
        DataTable dtKit1 = SqlHelper.ExecuteDataset(
            ConfigurationManager.ConnectionStrings["constr1"].ConnectionString,
            CommandType.Text,
Sql,
            new SqlParameter("@MemberID", MemberID)
        ).Tables[0];
        if (dtKit1.Rows.Count > 0)
        {
            int newTopUpSeq = Convert.ToInt32(dtKit1.Rows[0]["TopUpSeq"]);
            string query = "";
            query = "Select top 1 KitId, KitName, KitAmount From MM_KitMaster " +
                    "where ActiveStatus = 'Y' and Rowstatus = 'Y' " +
                    "and KitAmount > 0 and TopupSeq > " + newTopUpSeq +
                    " Order By KitId";
            DataTable dtKit11 = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, query).Tables[0];
            if (dtKit11.Rows.Count > 0)
            {
                int Kitid = Convert.ToInt32(dtKit11.Rows[0]["KitId"]);
                //string query1 = "";
                //query1 = "Select top 1 KitId, KitName, KitAmount From MM_KitMaster " +
                //   "where ActiveStatus = 'Y' and Rowstatus = 'Y' " +
                //   "and KitAmount > 0 and TopupSeq > " + newTopUpSeq +
                //   " Order By KitId";
                //DataTable dtKit111 = SqlHelper.ExecuteDataset(
                //ConfigurationManager.ConnectionStrings["constr1"].ConnectionString,
                //CommandType.Text, query1).Tables[0];
                if (newTopUpSeq != 0)
                {
                    if (Kitid == Convert.ToInt32(kitid_))
                    {
                        Msg = "OK";
                        return true;
                    }
                    else
                    {
                        Msg = "You have already taken this Package. Please choose a higher Package to upgrade.";
                        return false;
                    }
                }
                else
                {
                    Msg = "OK";
                    return true;
                }



            }
            else
            {
                Msg = "You have already taken this Package. Please choose a higher Package to upgrade.";
                return false;
            }

        }
        Msg = "OK";
        return true;
    }

    //private bool IsValidID(string MemberID, string PinNo, out string Msg)
    //{
    //    Msg = string.Empty;
    //    bool BoolResult = false;
    //    int NewKitTopupseq = 0;
    //    int categoryid = 0;
    //    string NewKitMacAdrs = string.Empty;

    //    // Clean input
    //    MemberID = MemberID.Trim().Replace(";", "").Replace("'", "").Replace("=", "");
    //    // Check package
    //    string q = "SELECT a.KitName, a.Allowtopup, a.MACAdrs, a.TopUpSeq, a.KitAmount, a.KitId, a.RP,categoryid " +
    //               "FROM MM_KitMaster AS a " +
    //               "WHERE CAST(a.KitAmount AS Numeric) = '" + Convert.ToDecimal(PinNo) + "' AND CAST(a.kitid AS Numeric) = '" + Convert.ToInt32(kitid_) + "' " +
    //               "AND a.Allowtopup = 'Y' AND a.RowStatus = 'Y' AND a.activeStatus = 'Y' " +
    //               "ORDER BY a.KitId DESC";

    //    DataTable dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, q).Tables[0];

    //    if (dt.Rows.Count > 0)
    //    {
    //        NewKitTopupseq = Convert.ToInt32(dt.Rows[0]["TopUpSeq"]);
    //        NewKitMacAdrs = dt.Rows[0]["MACAdrs"].ToString();
    //        categoryid = Convert.ToInt32(dt.Rows[0]["categoryid"]);
    //    }
    //    else
    //    {
    //        Msg = "Package not found.";
    //        return false;
    //    }

    //    // Get member details
    //    string qr1 = string.Empty;
    //        qr1 = "SELECT a.Formno, a.MemFirstName + ' ' + a.MemLastName AS MemName, " +
    //              "ISNULL(c.Idno, ' ') AS SponsorId, " +
    //              "ISNULL(c.MemFirstName + ' ' + c.MemLastName, ' ') AS SponsorName, " +
    //              "a.IsTopup, a.KitId, b.KitName, b.MACAdrs, b.TopUpSeq, a.LegNo, '' as Is_FranKit,categoryid " +
    //              "FROM MM_KitMaster AS b, M_MemberMaster AS a " +
    //              "LEFT JOIN M_MemberMaster AS c ON a.RefFormno = c.Formno " +
    //              "WHERE a.KitId = b.KitId AND b.RowStatus = 'Y' AND a.formno = '" + MemberID + "'";

    //    dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, qr1).Tables[0];

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows[0]["Is_FranKit"].ToString() == "Y")
    //        {
    //            BoolResult = true;
    //            Msg = "OK";
    //        }
    //        else
    //        {
    //            BoolResult = true;
    //            if (categoryid > Convert.ToInt32(dt.Rows[0]["categoryid"]))
    //            {
    //                Msg = "OK";
    //            }
    //            else
    //            {
    //                Msg = "Member Could Not Be Upgraded By This Package.";
    //            }
    //            //if (NewKitTopupseq >= Convert.ToInt32(dt.Rows[0]["TopUpSeq"]))
    //            //{
    //            //    Msg = "OK";
    //            //}
    //            //else
    //            //{
    //            //    Msg = "Member Could Not Be Upgraded By This Package.";
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        // Note: in original code this could throw an exception if dt.Rows.Count == 0
    //        // So adding safe check
    //        if (dt.Rows.Count == 0 || dt.Rows[0]["IsTopup"].ToString() == "N")
    //        {
    //            Msg = "OK";
    //        }
    //        else
    //        {
    //            Msg = "Member already activated. Please enter another Member ID.";
    //        }
    //    }

    //    return Msg == "OK";
    //}
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        if (TxtTransPass.Text == "")
        {
            ShowAlert("Please Transaction Password.!");
            return;
        }


        if (!string.IsNullOrEmpty(TxtAmount.Text))
        {
            if (DDLPaymode.SelectedValue == "1")
            {
                if (!Check_Amount_Condition())
                {
                    TxtAmount.Text = "";
                    return;
                }
            }
            string str = " Exec Sp_CheckTransctionPassword '" + Convert.ToInt32(Session["Formno"]) + "','" + TxtTransPass.Text.Trim() + "'";
            DataTable dts = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, str).Tables[0];
            if (dts.Rows.Count > 0)
            {
                IdActivation();
                //if (IsValidID(Session["formno"].ToString(), TxtAmount.Text, out string msg))
                //{
                //    if (msg.ToUpper() == "OK")
                //    {

                //    }
                //}
                //else
                //{
                //    string scrName = "<SCRIPT language='javascript'>alert('" + msg + "');</SCRIPT>";
                //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                //    return;
                //}
            }
            else
            {
                string scrName = "<SCRIPT language='javascript'>alert('Invalid Transaction Password ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
            }
        }

    }

    private void ShowAlert(string message)
    {
        string script = "<script language='javascript'>alert('" + message + "');</script>";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert", script, false);
    }

    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
    protected void DDLPaymode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLPaymode.SelectedValue == "1")
        {
            GetBalance();
            Check_Amount_Condition();
        }
        else
        {
            BtnProceedToPay.Visible = true;
            TxtAmount.Text = Session["PayAmount"].ToString();
        }
    }

}

