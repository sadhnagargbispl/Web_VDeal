using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Irony;
using System.Configuration;
using System.Web.UI;
using System.Web;

using System.Web.UI.WebControls;
using System.Net.Http;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.ServiceModel.Activities;
using System.Web.UI.HtmlControls;

partial class Registartion : System.Web.UI.Page
{
    private double _dblAvailLeg = 0;
    private clsGeneral dbGeneral = new clsGeneral();
    private cls_DataAccess dbConnect;
    private DAL ObjDAL;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    public string DsnName, UserName, Passw, role, token, refreshToken, name;
    private string strQuery, strCaptcha;
    //private System.Data.DataTable tmpTable = new System.Data.DataTable();
    //private AccClass.MyAccClass.NewClass QryCls = new AccClass.MyAccClass.NewClass();
    private int minSpnsrNoLen, minScrtchLen;
    private string Authorization;
    private double Upln, dblSpons, dblState, dblBank, dblIdNo;
    private string dblDistrict, dblTehsil, IfSC;
    private string dblPlan;
    private DateTime CurrDt;
    private string scrname;
    private string LastInsertID = "";
    private string InVoiceNo;
    private int SupplierId;
    private string BillNo;
    private string TaxType;
    private string BillDate;
    private int SBillNo;
    private string SoldBy = "WR";
    private string FType;
    private string IsoStart;
    private string IsoEnd;
    private SqlConnection cnn;
    private DataSet Ds = new DataSet();
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    private DataTable dt = new DataTable();
    System.Data.DataTable tmpTable = new System.Data.DataTable();
    public DateTime Now { get; private set; }

    protected void Page_Load(object sender, System.EventArgs e)
    {
        cnn = new SqlConnection(constr);
        dbConnect = new cls_DataAccess((string)Application["Connect"]);
        try
        {
            var str1 = "exec('Create table Trnjoining ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," + "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnjoining] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')";
            int i = 0;
            i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str1);
        }
        catch (Exception ex)
        {
        }

        // Me.BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnProceedToPay))

        try
        {
            if (!Page.IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomStringactive(6);
                // checkeKit(Request("kitid"))
                FindSession();
                FillStateMaster();
                FillCountryMasterNAme();
                int LegNo = 1;
                // txtRefralId.Text = Session("Idno")

                if (LegNo == 1)
                    RbtnLegNo.SelectedIndex = 0;
                else
                    RbtnLegNo.SelectedIndex = 1;
                RbtnLegNo.Enabled = false;
                Session["iLeg"] = LegNo;
            }
            if (Request["ref"] != null)
            {
                string req = Request["ref"].Replace(" ", "+");
                string str = Crypto.Decrypt(req);
                string[] rfAr = str.Split('/');
                if (rfAr.Length >= 1)
                {
                    if (rfAr[0] != "" & rfAr[1] == "1")
                    {
                        
                            TxtSponsorid.Text = (rfAr[0]);
                            RbtnLegNo.SelectedIndex = 0;
                            RbtnLegNo.Enabled = false;
                        TxtSponsorid.ReadOnly = true;
                        //goto refLink;
                        FillReferral(ref cnn);
                    }
                    else if (rfAr[0] != "" & rfAr[1] == "0")
                    {
                        
                            TxtSponsorid.Text = (rfAr[0]);
                            RbtnLegNo.SelectedIndex = 0;
                            RbtnLegNo.Enabled = false;
                        TxtSponsorid.ReadOnly = true;
                        //goto refLink;
                        FillReferral(ref cnn);
                    }
                    else if (rfAr[0] != "" & rfAr[1] == "2")
                    {
                       TxtSponsorid.Text = (rfAr[0]);
                            RbtnLegNo.SelectedIndex = 1;
                            RbtnLegNo.Enabled = false;
                        TxtSponsorid.ReadOnly = true;
                        //goto refLink;
                        FillReferral(ref cnn);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        // sb.Append("this.value = 'Please Wait...';")
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    public string GenerateRandomStringactive(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i <= iLength - 1; i++)
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        return sResult;
    }
    private void FillStateMaster()
    {
        try
        {
            strQuery = "SELECT STATECODE,STATENAME as State FROM M_StateDivMaster WHERE ACTIVESTATUS='Y' ORDER BY STATENAME";
            // dbConnect.OpenConnection()
            //dbConnect.Fill_Data_Tables(strQuery, tmpTable);
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQuery);
            tmpTable = ds.Tables[0];
            {
                var withBlock = ddlStatename;
                withBlock.DataSource = tmpTable;
                withBlock.DataValueField = "STATECODE";
                withBlock.DataTextField = "State";
                withBlock.DataBind();
                withBlock.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void TxtSponsorid_TextChanged(object sender, System.EventArgs e)
    {
        FillReferral(ref cnn);
    }
    private void FillReferral(ref SqlConnection Cnn)
    {
        try
        {
            errMsg.Text = "";
            //TxtSponsorid.Text = Replace(Replace(Replace(Trim(TxtSponsorid.Text), ";", ""), "'", ""), "=", "");
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = "Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where IDNo='" + TxtSponsorid.Text.Trim() + "' and IsBlock='N' ";
            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
            dt = Ds.Tables[0];
            if ((dt.Rows.Count > 0))
            {
                // TxtSponsorName.Text = dt.Rows(0)("MemName")
                lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
                HDnRefFormno.Value = dt.Rows[0]["formno"].ToString();
                //BtnProceedToPay.Enabled = false;
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist.!');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                TxtSponsorid.Text = "";
                lblRefralNm.Text = "";
                return;
            }
        }

        catch (Exception ex)
        {
        }
    }
    private object Trim(string text)
    {
        throw new NotImplementedException();
    }
    private string Replace(object value, string v1, string v2)
    {
        throw new NotImplementedException();
    }
    protected void txtemail_TextChanged(object sender, System.EventArgs e)
    {
        if (txtemail.Text != "")
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = " select Count(Email) as Email from M_Membermaster where Email='" + txtemail.Text.Trim() + "'";
            DsEmail = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];
            if (DtEmail.Rows[0]["Email"].ToString() != "0")
            //if (DtEmail.Rows.Count >= 1)
            {
                BtnProceedToPay.Enabled = true;
                chkterms.Checked = false;
                scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this Emailid.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                txtemail.Text = "";
                return;
            }
        }
    }
    protected void txtmobl_TextChanged(object sender, System.EventArgs e)
    {
        if (txtmobl.Text != "")
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = " select Count(mobl) as Mobile from M_Membermaster where mobl='" + txtmobl.Text.Trim() + "' ";
            DsEmail = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];
            if (Convert.ToInt32(DtEmail.Rows[0]["Mobile"]) > 0)
            //if (DtEmail.Rows.Count >= 1)
            {
                BtnProceedToPay.Enabled = true;
                chkterms.Checked = false;
                scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this MobileNo.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                txtmobl.Text = "";
                return;
            }
        }
    }
    public void SaveIntoDB()
    {
        try
        {
            int updateeffect;
            string StrSql2 = "Insert into Trnjoining (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrSql2));
            if (updateeffect > 0)
            {
                string strQry = "";
                string strDOB = "", strDOM = "", strDOJ, s;
                int iLeg;
                char cGender, cMarried;
                cGender = 'M';
                cMarried = 'N';
                string Aadharno = "";
                string HostIp = Context.Request.UserHostAddress.ToString();
                int DistrictCode, CityCode, VillageCode;
                BtnProceedToPay.Enabled = false;
                try
                {
                    if (Validt_SpnsrDtl("") == "OK")
                    {
                        iLeg = Convert.ToInt32(Session["iLeg"]);
                        string s1 = "";
                     
                        string q = "";
                        int i = 0;
                        DataTable Dt = new DataTable();
                        int BankCode = 0;
                        int AreaCode = 0;
                        AreaCode = 0;
                        string RegestType = "";
                        RegestType = "IN";
                        int PostalAreaCode = 0;
                        //strDOJ = Format(dbConnect.Get_ServerDate(), "dd-MMM-yyyy");

                        if (dblDistrict == null)
                            dblDistrict = "";
                        DistrictCode = 0;
                        CityCode = 0;
                        VillageCode = 0;
                        dblPlan = "0";
                        InVoiceNo = "0";
                        if (Convert.ToInt32(Session["SessID"]) == 0)
                            FindSession();
                        string Name = "";
                        string fathername = "";

                        fathername = ClearInject(txtfather.Text).ToString().ToUpper();
                        Name = ClearInject(Txtname.Text).ToString().ToUpper();
                        strQry = " insert into m_memberMaster (SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,";
                        strQry += "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,NomineeName,Address1,Address2,Post,";
                        strQry += "Tehsil,City,District,StateCode,CountryId,PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,";
                        strQry += "BankID,MICRCode,BranchName,EMail,BV,UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,";
                        strQry += " PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,IFSCode,Acno,AreaName,AreaCode,Fld2,AadharNo3,RegType,RegNo,usercode)";

                        strQry += "Values(" + Session["SessID"] + ",0,0,0,1,0,0," + iLeg + ",0,";
                        strQry += "" + Session["Refral"] + ",'" + ClearInject(Name).ToString().ToUpper() + "','','','" + ClearInject(fathername).ToString().ToUpper() + "',";
                        strQry += "getdate(),'" + cGender + "','','','" + ClearInject(TxtAddress.Text).ToString() + "','','";
                        strQry += "" + "','" + dblTehsil + "','" + txtTehsil.Text + "','" + txtDistrict.Text + "'," + ddlStatename.SelectedValue + "," + ddlCountryNAme.SelectedValue + ",";
                        strQry += "'" + txtPinCode.Text + "','0','CHOOSE ACCOUNT TYPE','" + txtmobl.Text + "',getdate(),'" + ClearInject(TxtPasswd.Text) + "',";
                        strQry += "Getdate(),'','','" + dblBank + "','','','" + ClearInject(txtemail.Text) + "'," + Session["Bv"] + ",0,'" + ClearInject(TxtPasswd.Text) + "',";
                        strQry += "'" + ClearInject(TxtPasswd.Text) + "','N','" + InVoiceNo + "','" + Session["RP"] + "','" + HostIp + "','0','0','','0','','', '','N','','','',";
                        strQry += "'" + VillageCode + "','','','" + RegestType + "','','" + ddlMobileNAme.Text + "')";
                        int isOk = 0;
                        isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strQry));
                        LastInsertID = "0";
                        if (isOk > 0)
                        {
                            string membername = "";
                            string Email = "";
                            string Password = "";
                            string str = "";
                            str = "EXEC Sp_GetProfile ";
                            DataTable Dt1 = new DataTable();
                            Dt1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
                            string Lastformno = "";
                            if (Dt1.Rows.Count > 0)
                            {
                                membername = Dt1.Rows[0]["MemfirstName"] + " " + Dt1.Rows[0]["MemLastName"];
                                Email = Dt1.Rows[0]["Email"].ToString();
                                LastInsertID = Dt1.Rows[0]["IDNO"].ToString();
                                Lastformno = Dt1.Rows[0]["Formno"].ToString();
                                Password = Dt1.Rows[0]["Passw"].ToString();
                                Session["Idno"] = Dt1.Rows[0]["IDNO"].ToString();
                                Session["Passw"] = Dt1.Rows[0]["Passw"].ToString();
                                Session["Kit"] = Dt1.Rows[0]["IsBill"].ToString();
                                //FUND_LOGIN_CHECK(Dt1.Rows[0]["IDNO"].ToString(), Dt1.Rows[0]["Passw"].ToString());
                            }
                            else
                                LastInsertID = "10001";
                            BtnProceedToPay.Enabled = true;
                            SendToMemberMail(LastInsertID, Email, membername, Password);
                            Session["LASTID"] = LastInsertID;
                            Session["Join"] = "YES";
                            Response.Redirect("thankyou.aspx", false);
                        }
                        else
                        {
                            BtnProceedToPay.Enabled = true;
                            chkterms.Checked = false;
                            scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" + "</SCRIPT>";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Try Again Later.');", true);
                        }
                    }
                }
                catch (Exception e)
                {
                    BtnProceedToPay.Enabled = true;
                    chkterms.Checked = false;
                    scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + e.Message + "');", true);
                    //string path = HttpContext.Current.Request.Url.AbsoluteUri;
                    //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
                    //ObjDAL.WriteToFile(text + e.Message);
                    Response.Write("Try later.");
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This id already register.!');location.replace('Registartion.aspx');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
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
    
    private string Val(string v)
    {
        throw new NotImplementedException();
    }
    private string ClearInject(Func<string> toUpper)
    {
        throw new NotImplementedException();
    }
    //private string Format(DateTime dateTime, string v)
    //{
    //    throw new NotImplementedException();
    //}
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password)
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
            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; color:black;\"> " + "<tr>" + "<td>" + "<span style=\"font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span>" + "Congratulations! you have successfully registered with  " + Session["CompName"].ToString() + " .<br />" + "Your username and password are given below : <br />" + "<strong>User ID: " + IdNo + "</strong><br />" + "<strong>Password: " + Password + "</strong><br />" + "You may login to the Member Center at: <a href=\"" + Session["CompWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompWeb"].ToString() + "</a><br />" + "<span style=\"color: #0099FF; font-weight: bold;\">Regards,</span><br />" + "<a href=\"" + Session["CompWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"].ToString() + "</a><br />" + "<br />" + "<br />" + "</td>" + "</tr>" + "</table>";

            MyMessage.Subject = "Welcome and Congratulations!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.Port = 587;
            // smtp.EnableSsl = False
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(MyMessage);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public string GenerateRandomString(ref int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i <= iLength - 1; i++)
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        return sResult;
    }
    public string Validt_SpnsrDtl(string chkby)
    {
        string valid = "";
        try
        {
            TxtSponsorid.Text = TxtSponsorid.Text.Replace("'", "").Replace("=", "").Replace(";", "");
            if (((TxtSponsorid.Text).Trim() != ""))
            {

                try
                {

                    DataTable dt = new DataTable();
                    string strSql = "Select FormNo,MemFirstName + ' ' + MemLastName as MemName,ActiveStatus from M_MemberMaster where Idno='" + TxtSponsorid.Text + "'";
                    dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql).Tables[0];

                    if ((dt.Rows.Count == 0))
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist.');" + "</SCRIPT>";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                        valid = "";
                        vsblCtrl(false, true);
                        return valid;
                    }
                    else
                    {
                        Session["Kitid"] = 1;
                        Session["Bv"] = 0;
                        Session["JoinStatus"] = "N";
                        Session["RP"] = 0;
                        valid = "OK";
                        Session["Refral"] = dt.Rows[0]["FormNo"];
                        lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
                        return valid;
                    }

                }

                catch (Exception ex)
                {
                    Response.Write("Please check sponsor ID.");
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Check Sponsor ID.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                TxtSponsorid.Focus();
                valid = "";
                return valid;
            }
            RbtnLegNo.Enabled = false;
            TxtSponsorid.Enabled = false;

            // txtPIN.Enabled = False
            // txtScratch.Enabled = False
            // cmdNext.Visible = False
            //return valid;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return valid;
    }
    
    protected void vsblCtrl(bool IsVsbl, bool IsOnlyDv)
    {
        try
        {
            if ((!IsOnlyDv))
                TxtSponsorid.Enabled = !IsVsbl;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj.Trim();
    }
    private void FindSession()
    {
        try
        {
            Session["SessID"] = 1;
            //break;
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select Max(SessId) as SessId from M_SessnMaster  " + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = Ds.Tables[0];
            if ((dt.Rows.Count > 0))
                Session["SessID"] = dt.Rows[0]["SessID"];
            else
            {
                errMsg.Text = "Session Not Exist. Please Enter New Session.";
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        if (chkterms.Checked == false)
        {
            scrname = "<SCRIPT language='javascript'>alert('Please select Terms and Condtions');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }

        else
        {
             if (Txtname.Text == "")
            {
                chkterms.Checked = false;
                string scrname = "<SCRIPT language='javascript'>alert('Please Enter Name.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            else if (txtfather.Text == "")
            {
                chkterms.Checked = false;
                string scrname = "<SCRIPT language='javascript'>alert('Please Enter Father Name.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            else if (txtmobl.Text == "")
            {
                chkterms.Checked = false;
                string scrname = "<SCRIPT language='javascript'>alert('Please Enter Mobile No.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            else if (txtemail.Text == "")
            {
                chkterms.Checked = false;
                string scrname = "<SCRIPT language='javascript'>alert('Please Enter Email Id.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            else if (TxtPasswd.Text == "")
            {
                chkterms.Checked = false;
                string scrname = "<SCRIPT language='javascript'>alert('Please Enter Password.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            errMsg.Text = "";
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select FormNo,MemFirstName + ' ' + MemLastName as MemName  ";
            strSql += "from M_MemberMaster where IDNo='" + TxtSponsorid.Text + "' and IsBlock='N' " + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
            dt = Ds.Tables[0];
            if ((dt.Rows.Count > 0))
            {
                lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
                HDnRefFormno.Value = dt.Rows[0]["formno"].ToString();
                BtnProceedToPay.Enabled = false;
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist.!');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                TxtSponsorid.Text = "";
                lblRefralNm.Text = "";
                return;
            }
            SaveIntoDB();
        }
    }
    private void FillCountryMaster(int CID)
   { 
     try
    {
        strQuery = "SELECT CId,StdCode FROM M_CountryMaster WHERE ACTIVESTATUS='Y' And  CId = '" + ddlCountryNAme.SelectedValue + "' ORDER BY StdCode";
            // dbConnect.OpenConnection()
            //dbConnect.Fill_Data_Tables(strQuery, tmpTable);
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQuery);
            tmpTable = ds.Tables[0];
            if (tmpTable.Rows.Count > 0)
            {
                //        var withBlock = ddlCountry;
                //withBlock.DataSource = tmpTable;
                //        withBlock.DataValueField = "CId";
                //        withBlock.DataTextField = "StdCode";
                //        withBlock.DataBind();
                //            withBlock.SelectedIndex = 0;
                //withBlock.SelectedIndex = 198;

                ddlMobileNAme.Text = tmpTable.Rows[0]["StdCode"].ToString();
            }
    }
    catch (Exception ex)
    {
    }
}
    private void FillCountryMasterNAme()
    {
        try
        {
            strQuery = "SELECT CId,CountryNAme FROM M_CountryMaster WHERE ACTIVESTATUS='Y' ORDER BY CountryNAme";
            // dbConnect.OpenConnection()
            //dbConnect.Fill_Data_Tables(strQuery, tmpTable);
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQuery);
            tmpTable = ds.Tables[0];
            {
                var withBlock = ddlCountryNAme;
                withBlock.DataSource = tmpTable;
                withBlock.DataValueField = "CId";
                withBlock.DataTextField = "CountryNAme";
                withBlock.DataBind();
                withBlock.SelectedValue = "229";
            }
            FillCountryMaster(Convert.ToInt32(ddlCountryNAme.SelectedValue));
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlCountryNAme_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillCountryMaster(Convert.ToInt32(ddlCountryNAme.SelectedValue));
        }
        catch (Exception ex)
        {
        }
    }
}
