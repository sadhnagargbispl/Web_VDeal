using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgotNew : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Add onclick attribute to the Submit button using DisableTheButton function
            this.Submit.Attributes.Add("onclick", DisableTheButton(this.Page, this.Submit));

            // Check if the page is not being loaded due to a postback
            if (!Page.IsPostBack)
            {
                try
                {
                    // SQL command to create a stored procedure
                    string str = "Create Proc Sp_MemberForgotPassw ( @IDNo  Nvarchar(50))As Begin Select (a.MemFirstName+' '+a.MemLastname) as MemName,a.Idno,a.Passw,a.EPassw,a.mobl,b.smsUsernm,b.smsSenderID,b.SmPass from m_membermaster as a,m_companymaster as b where IDNo = @IDNo End";

                    // Execute the SQL command using SqlHelper
                    int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);
                }
                catch (Exception ex)
                {
                    // Handle exceptions that occur during SQL command execution
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
    protected void Submit_Click(object sender, EventArgs e)
    {
        DataTable Dt;
        string scrname;

        if (txtIDNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('ID No. can not be left blank');</SCRIPT>";
            ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
            return;
        }

        if (TxtMobileNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Email id. can not be left blank');</SCRIPT>";
            ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
            return;
        }

        if (txtIDNo.Text == "" || TxtMobileNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Fill Detail');</SCRIPT>";
            ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
            return;
        }

        string IDNo = txtIDNo.Text.Replace("'", "").Replace(";", "").Replace("=", "").Replace("-", "").Trim();

        if (!string.IsNullOrEmpty(IDNo))
        {
            string MemberPass = "";
            string MemberTransPassw = "";
            string str = "Exec Sp_MemberForgotPassw '" + IDNo + "'";
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                string Username = Dt.Rows[0]["Idno"].ToString();
                string Password = Dt.Rows[0]["Passw"].ToString();
                string TranPassw = Dt.Rows[0]["EPassw"].ToString();
                string Email = Dt.Rows[0]["EMail"].ToString();
                string MemfristName = Dt.Rows[0]["MemName"].ToString();

                //if (Session["CompID"].ToString() == "1070")
               //{
                    Session["website1"] = Dt.Rows[0]["website"].ToString();
                //}

                Session["SmsId"] = Dt.Rows[0]["smsUsernm"].ToString();
                Session["SmsPass"] = Dt.Rows[0]["SmPass"].ToString();
                Session["ClientId"] = Dt.Rows[0]["smsSenderID"].ToString();

                if (TxtMobileNo.Text == Email)
                {
                    // Encode special characters in passwords
                    MemberPass = Password.Replace("%", "%25").Replace("&", "%26").Replace("#", "%23").Replace("'", "%22")
                        .Replace(",", "%2C").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21")
                        .Replace("/", "%2F").Replace("@", "%40");

                    MemberTransPassw = TranPassw.Replace("%", "%25").Replace("&", "%26").Replace("#", "%23").Replace("'", "%22")
                        .Replace(",", "%2C").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21")
                        .Replace("/", "%2F").Replace("@", "%40");

                    // Send email to member
                    SendToMemberMail(txtIDNo.Text, Email, MemfristName, Password, TranPassw);

                    scrname = "<SCRIPT language='javascript'>alert('Your Password has been sent on your E mail Id !');location.replace('Forgotnew.aspx');</SCRIPT>";
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);

                    txtIDNo.Text = "";
                    TxtMobileNo.Text = "";
                    return;
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Invalid Email id.');</SCRIPT>";
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                    return;
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Invalid ID No.');</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                return;
            }
        }

    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password, string EPassword)
    {
        try
        {
            DataTable dt;
            string sql = "";
            string userEmail = "";

            string StrMsg = "";
            System.Net.Mail.MailAddress SendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);

            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\"> " +
                     "<tr>" +
                     "<td>" +
                     "<span style=\"color: #0099CC; font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span><br />" +
                     "Your Forgot Login password is <strong>" + Password + "</strong> and Transaction password is <strong>" + EPassword + "</strong> of IDNO <strong>" + IdNo + "</strong>.<br/> For login go to our site : <a href=\"" + Session["CompWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompWeb"].ToString() + "</a><br/>Thank you!<br> Regards : <br/><a href=\"" + Session["CompWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"].ToString() + "</a><br />" +
                     "<br />" +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                     "</table>";

            MyMessage.Subject = "Forgot Password";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;
            var smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(MyMessage);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
