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
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net.Mail;

public partial class WalletTransfer : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
                this.BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnProceedToPay));
                this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));

                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomString(6);
                    TxtMemberName.ReadOnly = true;
                    Session["OtpCount"] = 0;
                    Session["OtpTime"] = null;
                    Session["Retry"] = null;
                    Session["OTP_"] = null;
                    GetBalance();
                    // txtMemberId.Text = Session["IdNo"].ToString();
                    //GetName();
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
    public string GenerateRandomString(int iLength)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        return sResult;
    }
    protected void GetBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = " Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','S')";
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
    protected void CheckAmount()
    {
        DataTable Dt;
        string str = "Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','S')";
        Dt = new DataTable();
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
        if (Dt.Rows.Count > 0)
        {
            Session["MainBalance"] = Convert.ToInt32(Dt.Rows[0]["Balance"]);
            LblAmount.Text = Convert.ToInt32(Dt.Rows[0]["Balance"]).ToString();
            if (Convert.ToInt32(Session["MainBalance"]) < Convert.ToInt32(txtAmount.Text))
            {
                LblAmount.Text = "Insufficient Balance";
                LblAmount.ForeColor = System.Drawing.Color.Red;
                LblAmount.Visible = true;
                BtnProceedToPay.Enabled = false;
            }
            else
            {
                LblAmount.Visible = false;
                BtnProceedToPay.Enabled = true;
            }
        }
    }
    public string GetName()
    {
        try
        {
            string strquery = "";
            strquery = " select * from M_membermaster where idno='" + txtMemberId.Text.Trim() + "' ";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtMemberName.Text = ds.Tables[0].Rows[0]["Memfirstname"].ToString();
                HdnFormno.Value = ds.Tables[0].Rows[0]["Formno"].ToString();
                return "OK";
            }
            else
            {
                string scrName = "<SCRIPT language='javascript'>alert('Invalid ID Does Not Exist');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                TxtMemberName.Text = "";
                HdnFormno.Value = "0";
                txtMemberId.Text = "";
                return "";
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
    }
    protected void otp_save_function()
    {
        try
        {
            string scrname = "";

            if (txtMemberId.Text.Trim() == "")
            {
                scrname = "<script language='javascript'>alert('Enter Member ID');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            if (txtAmount.Text.Trim() == "")
            {
                scrname = "<script language='javascript'>alert('Please Enter Amount.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }



            if (Convert.ToInt32(txtAmount.Text) < 0)
            {
                scrname = "<script language='javascript'>alert('Sorry ! You cannot fund request with negative value.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            if (GetName() == "OK")
            {
                string scrName = "";

                int OTP_ = 0;
                Random Rs = new Random();
                OTP_ = Rs.Next(100001, 999999);
                //TxtOtp.Text = OTP_.ToString();
                if (Session["OTP_"] == null)
                {
                    if (SendMail(OTP_))
                    {
                        Session["OtpTime"] = DateTime.Now.AddMinutes(5);
                        Session["Retry"] = "1";
                        Session["OTP_"] = OTP_;

                        int i = 0;
                        string R = "";
                        R = "INSERT AdminLogin(UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID) " +
                            "VALUES ('0', '', '" + TxtOtp.Text + "', '0', '" + OTP_ + "', getdate(), '" + OTP_ + "', '" + Session["EMail"].ToString().Trim() + "')";
                        i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, R));

                        if (i > 0)
                        {
                            divotp.Visible = true;
                            BtnProceedToPay.Visible = false;
                            BtnOtp.Visible = true;
                            ResendOtp.Visible = false;
                            scrName = "<script language='javascript'>alert('OTP Send On Mail');</script>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                            return;
                        }
                        else
                        {
                            scrName = "<script language='javascript'>alert('Try Later');</script>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                            return;
                        }
                    }
                    else
                    {
                        scrName = "<script language='javascript'>alert('OTP Try Later');</script>";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                        return;
                    }
                }
                else
                {
                    divotp.Visible = true;
                    BtnProceedToPay.Visible = false;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void AmountTransfer()
    {
        string StrSql = "Insert into TrnFundUniqe (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
        int updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrSql));
        if (updateeffect > 0)
        {
            string query = "";
            string scrName = "";
            string VouherNo2 = "";
            string VouherNo3 = "";

            CheckAmount();

            if (Convert.ToInt32(txtAmount.Text) < 0)
            {
                scrName = "<SCRIPT language='javascript'>alert('Invalid Amount!! ');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Upgraded", scrName, false);
                return;
            }

            if (Convert.ToInt32(Session["MainBalance"]) >= Convert.ToInt32(txtAmount.Text))
            {
                string remark1 = "Receive from IdNo " + Session["IDNo"] + "";
                string remark = "Transfer Gift Wallet To Gift Wallet To IdNo " + txtMemberId.Text + "";
                string Remarks = "Gift Wallet To Gift Wallet Transfer Of " + txtAmount.Text + " Rs. from " + Session["IDNo"];
                query = "Exec Sp_WaslletTransferAmountsave '" + Session["Formno"] + "','" + Convert.ToInt32(txtAmount.Text).ToString() + "','" + remark + "',";
                query += "'" + remark1 + "','" + VouherNo2 + "/" + txtMemberId.Text + "','" + VouherNo3 + "/" + txtMemberId.Text + "','" + HdnFormno.Value + "','" + Remarks + "'";
                CheckAmount();
                if (Convert.ToInt32(Session["MainBalance"]) >= Convert.ToInt32(txtAmount.Text))
                {
                    int TrnSql;
                    TrnSql = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, query));
                    if (TrnSql > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Amount Transfer Successfully!!');location.replace('GiftWallet.aspx');", true);
                        Session["CkyPinTransfer1"] = null;
                        GetBalance();
                        txtMemberId.Text = "";
                        TxtMemberName.Text = "";
                        txtAmount.Text = "";
                        LblAmount.Text = "";
                        BtnProceedToPay.Visible = true;
                    }
                }
            }
            else
            {
                scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance!! ');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Upgraded", scrName, false);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Something Went Wrong.!');location.replace('GiftWallet.aspx');", true);
        }
    }
    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please Wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        otp_save_function();
    }
    protected void TxtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            try
            {
                if (Convert.ToInt32(txtAmount.Text) == 0)
                {
                    string scrName = "<script language='javascript'>alert('Sorry ! Minimum transfer amount should be greater than 0.00.');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Upgraded", scrName, false);
                    txtAmount.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    protected void TxtUserID_TextChanged(object sender, EventArgs e)
    {
        GetName();
    }
    public bool SendMail(int otp)
    {
        try
        {
            string strMsg = "";
            string emailAddress = Session["EMail"].ToString().Trim();
            MailAddress sendFrom = new MailAddress(Session["CompMail"].ToString());
            MailAddress sendTo = new MailAddress(emailAddress);
            MailMessage myMessage = new MailMessage(sendFrom, sendTo);

            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                     "<tr>" +
                     "<td>" +
                     "Your OTP for Wallet Transfer is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                     "</table>";

            myMessage.Subject = "Thanks For Connecting!!!";
            myMessage.Body = strMsg;
            myMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString())
            {
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString())
            };
            smtp.Send(myMessage);
            txtMemberId.ReadOnly = true;
            txtAmount.ReadOnly = true;
            int c = 0;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnOtp_Click(object sender, EventArgs e)
    {
        string scrname = "";
        try
        {
            DataTable dt = new DataTable();
            Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;
            if (Convert.ToInt32(Session["OTP_"]) == Convert.ToInt32(TxtOtp.Text))
            {
                string str = "Select TOP 1 * from AdminLogin as a where EmailID='" + Session["EMail"].ToString().Trim() + "' and emailotp='" + Convert.ToInt32(TxtOtp.Text) + "' ORDER BY AID DESC ";
                dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    AmountTransfer();
                }
            }
            else
            {
                TxtOtp.Text = "";

                if (Convert.ToInt32(Session["OtpCount"]) >= 3)
                {
                    Session["OtpCount"] = 0;
                    scrname = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\n Please generate OTP again.');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('You have tried 3 times with invalid OTP.\n Please generate OTP again.');", true);
                    ResendOtp.Visible = true;
                    BtnOtp.Visible = false;
                    divotp.Visible = false;
                }
                else
                {
                    scrname = "<script language='javascript'>alert('Invalid OTP.');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Invalid OTP.');", true);
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
            int OTP_ = 0;
            Random rs = new Random();
            OTP_ = rs.Next(100001, 999999);

            if (SendMail(OTP_))
            {
                Session["OtpTime"] = DateTime.Now.AddMinutes(5);
                Session["Retry"] = "1";
                Session["OTP_"] = OTP_;
                string emailId = Session["Email"].ToString();
                string memberName = "";
                string mobileNo = "0";
                int i = 0;
                string strSql = "";

                strSql = "INSERT INTO AdminLogin(UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID) " +
                         "VALUES ('0', '" + memberName + "', '" + TxtOtp.Text + "', '" + mobileNo + "', '" + OTP_ + "', GETDATE(), '" + OTP_ + "', '" + Session["EMail"].ToString().Trim() + "')";

                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strSql));

                if (i > 0)
                {
                    divotp.Visible = true;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = false;
                    string scrName = "<script language='javascript'>alert('OTP Sent On Mail');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                    return;
                }
                else
                {
                    string scrName = "<script language='javascript'>alert('Try Later');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                    return;
                }
            }
            else
            {
                string scrName = "<script language='javascript'>alert('OTP Try Later');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrName, false);
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}