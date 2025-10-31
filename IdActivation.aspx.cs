using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Text;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
//using DocumentFormat.OpenXml.Wordprocessing;
public partial class IdActivation : System.Web.UI.Page
{
    string scrName;
    DAL objDal = new DAL();
    DataTable Dt = new DataTable();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                cmdSave1.Attributes.Add("onclick", DisableTheButton(this.Page, this.cmdSave1));
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringActive(6);
                    FundWalletGetBalance();
                    fillkit();
                    //txtMemberId.Text = Session["idno"].ToString();
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
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomStringActive(int iLength)
    {
        try
        {
            Random rdm = new Random();
            char[] allowChrs = "123456789".ToCharArray();
            string sResult = "";

            for (int i = 0; i < iLength; i++)
            {
                sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
            }
            return sResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string GetName()
    {
        try
        {
            string str = string.Empty;
            DataTable dt = new DataTable();

            str = " Exec Sp_GetMemberNameINR '" + txtMemberId.Text + "'";
            dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, str).Tables[0];

            if (dt.Rows.Count == 0)
            {
                txtMemberId.Text = string.Empty;
                TxtMemberName.Text = string.Empty;
                HdnMemberMacAdrs.Value = string.Empty;
                HdnMemberTopupseq.Value = string.Empty;

                string scrName = "<SCRIPT language='javascript'>alert('Invalid ID Does Not Exist');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
            }
            else
            {
                if (dt.Rows[0]["Isblock"].ToString() == "Y")
                {
                    txtMemberId.Text = string.Empty;
                    TxtMemberName.Text = string.Empty;
                    HdnMemberMacAdrs.Value = string.Empty;
                    HdnMemberTopupseq.Value = string.Empty;

                    string scrName = "<SCRIPT language='javascript'>alert('This Id is blocked. Please Contact Admin.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                    return string.Empty;
                }

                else
                {
                    TxtMemberName.Text = dt.Rows[0]["memname"].ToString();
                    HdnMemberMacAdrs.Value = dt.Rows[0]["MacAdrs"].ToString();
                    HdnMemberTopupseq.Value = dt.Rows[0]["Topupseq"].ToString();
                    MemberStatus.Value = dt.Rows[0]["ActiveStatus"].ToString();
                    hdnFormno.Value = dt.Rows[0]["Formno"].ToString();
                    hdnemail.Value = dt.Rows[0]["Email"].ToString();
                    LblMobile.Text = string.Empty;
                    return "OK";
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

        return string.Empty;
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void cmdSave1_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtAmount.Text == "")
            {
                scrName = "<script language='javascript'>alert('Please Fill Request Amount !');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                txtAmount.Text = "";
                return;
            }
            if (Convert.ToDouble(txtAmount.Text) == 0)
            {
                string scrName = "<SCRIPT language='javascript'>alert('Invalid Amount.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                return;
            }

            if (!string.IsNullOrEmpty(txtAmount.Text))
            {
                if (!CheckAmount())
                {
                    txtAmount.Text = "0";
                    return;
                }
            }
            string str = " Exec Sp_CheckTransctionPassword '" + Convert.ToInt32(Session["Formno"]) + "','" + TxtTransPass.Text.Trim() + "'";
            DataTable dts = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, str).Tables[0];
            if (dts.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(txtMemberId.Text))
                {
                    string scrName = "<SCRIPT language='javascript'>alert('Enter Id No.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                    return;
                }
                else if (Convert.ToDouble(txtAmount.Text) == 0)
                {
                    string scrName = "<SCRIPT language='javascript'>alert('Invalid Amount.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                    return;
                }
                else if (Checkid() == string.Empty)
                {
                    if (DDLPaymode.SelectedValue == "1")
                    {
                        IdAActivation();
                    }
                }
            }
            else
            {
                string scrName = "<SCRIPT language='javascript'>alert('Invalid Wallet Password ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
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
    protected void IdAActivation()
    {
        try
        {
            string query = "";
            string kitid = "";
            string strSql = "Insert into Trnfundwithdrawcpanel (Transid, Rectimestamp) values(" + HdnCheckTrnns.Value + ", GETDATE())";
            int updateEffect = objDal.SaveData(strSql);

            if (updateEffect > 0)
            {
                if (GetName() == "OK")
                {
                    CheckAmount();
                    if (Convert.ToDecimal(Session["ServiceWallet"]) >= Convert.ToDecimal(txtAmount.Text))
                    {
                        DataTable dt_ = new DataTable();
                        string sql = "";
                        var billNo = GenerateRandomStringactive(6);
                        sql = "EXEC Sp_PaymentActivateNewINR '" + txtMemberId.Text.Trim() + "','" + CmbKit.SelectedValue + "', '','" + txtAmount.Text + "','USDT',";
                        sql += "'" + billNo + "','" + Session["Formno"] + "'";
                        DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                        if (dt.Rows[0]["Result"].ToString().ToUpper() == "SUCCESS")
                        {
                            Clear();
                            FundWalletGetBalance();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Id Activation/Upgrade Successfully!!');location.replace('IdActivation.aspx');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Id Activation/Upgrade Not Successfully!!');", true);
                        }
                    }
                    else
                    {
                        var scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance In Fund Wallet.!');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
                    }
                }
            }
            else
            {
                Response.Redirect("IdActivation.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void Clear()
    {
        try
        {
            txtMemberId.Text = "";
            TxtMemberName.Text = "";
            txtAmount.Text = "";
            // TxtRemark.Text = ""; // Uncomment if this field is used
            LblAmount.Text = "";
            LblAmount.Visible = false;
            LblError.Visible = false;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private string Checkid()
    {
        try
        {
            string str = string.Empty;

            if (GetName() == "OK")
            {
                str = "";
            }

            return str;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

        return string.Empty;
    }
    protected void fillkit(string condition = "")
    {
        try
        {
            string query = "";
            DataTable dt = new DataTable();
            query = "Select KitId,KitName,KitAmount From M_KitMaster where ActiveStatus='Y' and Rowstatus='Y' and Kitamount>0 AND kitid not in (2,3,4) " + condition + " Order By kitId";
            dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, query).Tables[0];

            Session["KitTable"] = dt;
            CmbKit.DataSource = dt;
            CmbKit.DataTextField = "KitName";
            CmbKit.DataValueField = "KitId";
            CmbKit.DataBind();

            Session["MKit"] = dt;

            if (dt.Rows.Count > 0)
            {
                txtAmount.Text = dt.Rows[0]["KitAmount"].ToString();
                Session["KitId"] = dt.Rows[0]["KitId"].ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }
    //protected void FillKit()
    //{
    //    try
    //    {
    //        string query = "";
    //        DataTable dt;
    //        string condition = "";
    //        dt = new DataTable();
    //        query = " Exec sp_GetKitDetailsINR ";
    //        dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
    //        if (dt.Rows.Count > 0)
    //        {
    //            Session["KitTable"] = dt;
    //            CmbKit.DataSource = dt;
    //            CmbKit.DataTextField = "KitName";
    //            CmbKit.DataValueField = "KitId";
    //            CmbKit.DataBind();
    //            txtAmount.Text = dt.Rows[0]["Kitamount"].ToString();
    //        }
    //        else
    //        {
    //            txtAmount.Text = "0";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string path = HttpContext.Current.Request.Url.AbsoluteUri;
    //        string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
    //        objDal.WriteToFile(text + ex.Message);
    //        Response.Write("Try later.");
    //    }
    //}
    protected bool CheckAmount()
    {
        try
        {
            DataTable dt;
            string str = " Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','R')";

            dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, str).Tables[0];

            if (dt.Rows.Count > 0)
            {
                Session["ServiceWallet"] = Convert.ToDouble(dt.Rows[0]["Balance"]);
                LblAmount.Text = Convert.ToDouble(dt.Rows[0]["Balance"]).ToString();

                if (Convert.ToDouble(Session["ServiceWallet"]) < Convert.ToDouble(txtAmount.Text))
                {
                    LblAmount.Text = "Insufficient Balance";
                    LblAmount.ForeColor = System.Drawing.Color.Red;
                    LblAmount.Visible = true;
                    cmdSave1.Enabled = false;
                    return false;
                }
                else
                {
                    // LblAmount.Text = "";
                    cmdSave1.Enabled = true;
                    LblAmount.Visible = false;
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

        return false;
    }
    protected void FundWalletGetBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = " Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','R')";
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                AvailableBal.InnerText = Convert.ToString(dt.Rows[0]["Balance"]);
            }
            else
            {
                AvailableBal.InnerText = "0";
            }
            Session["ServiceWallet"] = AvailableBal.InnerText;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }

    }
    private bool Check_IdNo()
    {
        try
        {
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)Session["MKit"];
            //DataRow[] Dr = dt1.Select("KitID='" + CmbKit.SelectedValue + "'");

            string Sql = "";
            Sql = "Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId," +
       " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,a.LegNo,B.KitName,a.BV,b.bv as KBv " +
       ",Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else '' end as UpgradeDate,a.ActiveStatus,a.FLD1,a.Planid,a.isblock " +
       " , a.Fld4 from M_KitMaster as b,M_MemberMaster as a Left Join M_MemberMaster as c on a.RefFormno=c.Formno  " +
       " where a.KitId=b.KitId and  (b.RowStatus='Y')  and a.IDNo='" + txtMemberId.Text + "' and a.IsBlock='N'";
            DataTable Dt_ = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, Sql).Tables[0];
            if (Dt_.Rows.Count > 0)
            {
                kitid.Text = Dt_.Rows[0]["KitId"].ToString();
                LblError.Text = "";
                TxtMemberName.Text = Dt_.Rows[0]["MemName"].ToString();
                lblFormno.Text = Dt_.Rows[0]["Formno"].ToString();
                LblCondition.Text = "and TopupSeq>'" + Dt_.Rows[0]["TopupSeq"].ToString() + "'";
                return true;
            }
            else
            {
                kitid.Text = "";
                LblError.Text = "";
                TxtMemberName.Text = "";
                lblFormno.Text = "";
                LblCondition.Text = "";
                return false;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            // ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return false;
        }
    }
    protected void txtMemberId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Check_IdNo() == true)
            {
                fillkit(LblCondition.Text.Trim());
                if (CmbKit.SelectedValue == "")
                {
                    txtAmount.Text = "0";
                    cmdSave1.Enabled = false;
                }
                else
                {
                    cmdSave1.Enabled = true;
                }
            }
            else
            {
                cmdSave1.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void CmbKit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt = new DataTable();
        Dt = (DataTable)Session["KitTable"];
        DataRow[] Dr = Dt.Select("KitID='" + CmbKit.SelectedValue + "'");
        if (Dr.Length > 0)
        {
            txtAmount.Text = Dr[0]["KitAmount"].ToString();
        }

    }
}
