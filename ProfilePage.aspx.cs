using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProfilePage : System.Web.UI.Page
{
    double _dblAvailLeg = 0;
    private DAL ObjDAL = new DAL();
    SqlCommand cmd = new SqlCommand();
    SqlDataReader dRead;
    string strQuery, strCaptcha;
    DataTable tmpTable = new DataTable();
    int minSpnsrNoLen, minScrtchLen;
    double Upln, dblSpons, dblTehsil, dblDistrict, dblIdNo;
    DateTime CurrDt;
    string[] montharray = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    int LastInsertID = 0;
    string scrname;
    DAL Obj;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
                if (!Page.IsPostBack)
                {
                    FillCountryName();
                    FillCountryMasterCode();
                    FillDetail();
                   
                }

            }

            else
            {
                Response.Redirect("Login.aspx", false);
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
    public bool IsValidIFSCCode(string ifscCode)
    {
        string regexPattern = "^[A-Z]{4}0[A-Z0-9]{6}$";
        Regex regex = new Regex(regexPattern);
        return regex.IsMatch(ifscCode);
    }
    public bool IsValidPANNumber(string panNumber)
    {
        string regexPattern = "^[A-Z]{5}[0-9]{4}[A-Z]{1}$";
        Regex regex = new Regex(regexPattern);
        return regex.IsMatch(panNumber);
    }
    public bool IsValidMobileNumber(string mobileNumber)
    {
        string regexPattern = "^[0-9]{10}$";
        Regex regex = new Regex(regexPattern);
        return regex.IsMatch(mobileNumber);
    }
    public bool IsValidAccountNumber(string accountNumber)
    {
        string regexPattern = "^[0-9]{8,14}$";
        Regex regex = new Regex(regexPattern);
        return regex.IsMatch(accountNumber);
    }
    private void FillDetail()
    {
        try
        {
            string idverified = "";
            string sql = "exec sp_MemDtl ' and mMst.Formno=''" + Session["Formno"] + "'''  ";
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtReferalId.Text = (dt.Rows[0]["RefIDNo"] == DBNull.Value) ? "" : dt.Rows[0]["RefIDNo"].ToString();
                TxtReferalNm.Text = dt.Rows[0]["RefName"].ToString();
                TxtUplinerid.Text = (dt.Rows[0]["UpLnIDNo"] == DBNull.Value) ? "" : dt.Rows[0]["UpLnIDNo"].ToString();
                TxtUplinerName.Text = dt.Rows[0]["UpLnName"].ToString();
                txtFrstNm.Text = dt.Rows[0]["MemName"].ToString();
                lblPosition.Text = (dt.Rows[0]["LegNo"].ToString() == "1") ? "Left" : "Right";
                txtFNm.Text = dt.Rows[0]["MemFname"].ToString();
                TxtDobDate.Text = Convert.ToDateTime(dt.Rows[0]["MemDob"]).ToString("dd-MMM-yyyy");
                txtPhNo.Text = dt.Rows[0]["PhN1"].ToString();
                ddlMobileNAme.Text = dt.Rows[0]["usercode"].ToString();
                txtMobileNo.Text = dt.Rows[0]["Mobl"].ToString();
                txtEMailId.Text = dt.Rows[0]["EMail"].ToString();
                ddlCountryName.SelectedValue = dt.Rows[0]["CountryId"].ToString();
                txtNominee.Text = dt.Rows[0]["NomineeName"].ToString();
                txtRelation.Text = dt.Rows[0]["Relation"].ToString();
                
                if (Convert.ToChar(dt.Rows[0]["ActiveStatus"].ToString()) == 'N')
                {
                    txtFrstNm.Text = dt.Rows[0]["MemName"].ToString();
                    txtFrstNm.Enabled = true;
                    if (ddlMobileNAme.Text != "")
                    {
                        ddlMobileNAme.Enabled = false;
                    }
                    else
                    {
                        ddlMobileNAme.Enabled = true;
                    }
                    txtMobileNo.Text = dt.Rows[0]["Mobl"].ToString();
                    txtMobileNo.Enabled = true;
                    txtEMailId.Text = dt.Rows[0]["EMail"].ToString();
                    txtEMailId.Enabled = true;
                    if (!string.IsNullOrEmpty(ddlCountryName.SelectedValue) && Convert.ToInt32(ddlCountryName.SelectedValue) > 0)
                    {
                        ddlCountryName.Enabled = false;
                    }
                    if (txtFNm.Text != "")
                    {
                        txtFNm.Enabled = false;
                    }
                    else
                    {
                        txtFNm.Enabled = true;
                    }

                    if (txtPhNo.Text.Length >= 10)
                    {
                        txtPhNo.Enabled = false;
                    }
                    else
                    {
                        txtPhNo.Enabled = true;
                    }
                    if (txtNominee.Text != "")
                    {
                        txtNominee.Enabled = false;
                    }
                    else
                    {
                        txtNominee.Enabled = true;
                    }
                    if (txtRelation.Text != "")
                    {
                        txtRelation.Enabled = false;
                    }
                    else
                    {
                        txtRelation.Enabled = true;
                    }
                }
                else {
                    txtFrstNm.Enabled = false;
                    if (!string.IsNullOrEmpty(ddlCountryName.SelectedValue) && Convert.ToInt32(ddlCountryName.SelectedValue) > 0)
                {
                    ddlCountryName.Enabled = false;
                }
                //if (IsValidMobileNumber(txtMobileNo.Text))
                //{
                //    txtMobileNo.Enabled = false;
                //}
                //else
                //{
                //    txtMobileNo.Enabled = true;
                //}

                if (txtFNm.Text != "")
                {
                    txtFNm.Enabled = false;
                }
                else
                {
                    txtFNm.Enabled = true;
                }

                if (txtPhNo.Text.Length >= 10)
                {
                    txtPhNo.Enabled = false;
                }
                else
                {
                    txtPhNo.Enabled = true;
                }

                if (txtEMailId.Text != "")
                {
                    txtEMailId.Enabled = false;
                }
                else
                {
                    txtEMailId.Enabled = true;
                }

                if (txtNominee.Text != "")
                {
                    txtNominee.Enabled = false;
                }
                else
                {
                    txtNominee.Enabled = true;
                }
                if (txtMobileNo.Text != "0")
                {
                    txtMobileNo.Enabled = false;
                }
                else
                {
                    txtMobileNo.Enabled = true;
                }
                if (ddlMobileNAme.Text != "")
                {
                    ddlMobileNAme.Enabled = false;
                }
                else
                {
                    ddlMobileNAme.Enabled = true;
                }
                if (txtRelation.Text != "")
                {
                    txtRelation.Enabled = false;
                }
                else
                {
                    txtRelation.Enabled = true;
                }
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private string ConvertDateToString(int Month)
    {
        try
        {
            switch (Month)
            {
                case 1:
                    return "JAN";
                case 2:
                    return "FEB";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    return "";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = $"{path}:  {DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ")}{Environment.NewLine}";
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return "";
        }
    }
    private void FindSession()
    {
        try
        {
            string sql = "Select Top 1 SessID, ToDate, FrmDate from M_SessnMaster order by SessID desc";
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Session["SessID"] = dt.Rows[0]["SessID"];
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = $"{path}:  {DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ")}{Environment.NewLine}";
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void UpdateDb()
    {
        try
        {
            string strQry = "";
            DateTime strDOB;
            string Remark = "";
            string MembName = "";
            string Password = "";
            string TransactionPassword = "";

            try
            {
                DataTable Dt1 = new DataTable();
                strDOB = DateTime.Parse(TxtDobDate.Text);

                string str = "select * from M_MemberMaster where Formno='" + Session["Formno"] + "'";
                Dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];

                if (Dt1.Rows.Count > 0)
                {
                    MembName = Dt1.Rows[0]["MemFirstName"].ToString() + " " + Dt1.Rows[0]["MemLastName"].ToString();
                    Password = Dt1.Rows[0]["Passw"].ToString();
                    TransactionPassword = Dt1.Rows[0]["EPassw"].ToString();

                    if (ClearInject(Dt1.Rows[0]["MemfirstName"].ToString()) != ClearInject(txtFrstNm.Text))
                        Remark += " Member Name From " + ClearInject(Dt1.Rows[0]["MemfirstName"].ToString()) + " to " + ClearInject(txtFrstNm.Text) + ",";

                    if (DateTime.Parse(Dt1.Rows[0]["MemDob"].ToString()) != strDOB)
                        Remark += "Dob From " + ClearInject(Dt1.Rows[0]["MemDob"].ToString()) + " to " + ClearInject(strDOB.ToString("dd-MMM-yyyy")) + ",";

                    if (ClearInject(Dt1.Rows[0]["PhN1"].ToString()) != ClearInject(txtPhNo.Text))
                        Remark += " PhoneNo From " + ClearInject(Dt1.Rows[0]["PhN1"].ToString()) + " to " + ClearInject(txtPhNo.Text) + ",";
                    if (Convert.ToInt32(Dt1.Rows[0]["CountryId"]) != Convert.ToInt32(ddlCountryName.SelectedValue))
                    {
                        Remark = Remark + " Country from " + ClearInject(Convert.ToInt32(Dt1.Rows[0]["CountryId"]) + " to " + Convert.ToInt32(ddlCountryName.SelectedValue)) + ",";
                    }
                    if (Convert.ToString(Dt1.Rows[0]["Usercode"].ToString()) != ddlMobileNAme.Text.ToString())
                    {
                        Remark = Remark + " usercode from " +ClearInject(Convert.ToString(Dt1.Rows[0]["Usercode"].ToString()) +" to " + ddlMobileNAme.Text.ToString()) + ", ";
                    }
                    if (ClearInject(Dt1.Rows[0]["Mobl"].ToString()) != ClearInject(txtMobileNo.Text))
                        Remark += " MobileNo From " + ClearInject(Dt1.Rows[0]["Mobl"].ToString()) + " to " + ClearInject(txtMobileNo.Text) + ","; 

                    if (ClearInject(Dt1.Rows[0]["Email"].ToString()) != ClearInject(txtEMailId.Text))
                        Remark += " Email From " + ClearInject(Dt1.Rows[0]["Email"].ToString()) + " to " + ClearInject(txtEMailId.Text) + ",";

                    if (ClearInject(Dt1.Rows[0]["NomineeName"].ToString()) != ClearInject(txtNominee.Text))
                        Remark += " NomineeName From " + ClearInject(Dt1.Rows[0]["NomineeName"].ToString()) + " to " + ClearInject(txtNominee.Text) + ",";

                    if (ClearInject(Dt1.Rows[0]["Relation"].ToString()) != ClearInject(txtRelation.Text))
                        Remark += " Relation From " + ClearInject(Dt1.Rows[0]["Relation"].ToString()) + " to " + ClearInject(txtRelation.Text) + ",";
                }

                strQry = " exec Sp_UpdateMemberProfile  '" + Session["FormNo"] + "','" + ClearInject(txtFrstNm.Text.ToUpper()) + "','" + ClearInject(txtFNm.Text.ToUpper()) + "',";
                strQry += " '" + strDOB.ToString("dd-MMM-yyyy") + "','" + ClearInject(txtPhNo.Text) + "','" + ClearInject(txtMobileNo.Text) + "','" + ClearInject(txtEMailId.Text) + "',";
                strQry += " '" + ClearInject(txtNominee.Text.ToUpper()) + "','" + ClearInject(txtRelation.Text.ToUpper()) + "',";
                strQry += "'Update Profile - " + Context.Request.UserHostAddress.ToString() + "','" + Remark + "'," + ddlCountryName.SelectedValue + ",'" + ddlMobileNAme.Text + "';";
                strQry += " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)";
                strQry += "Values(" + (Session["formno"]) + ",'" + Session["MemName"] + "','Profile','Profile Update',";
                strQry += "'" + Remark + "',Getdate()," + (Session["formno"]) + ");";

                int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strQry));
                string message = "";
                if (i > 0)
                    message = "Profile Successfully Updated.!";
                else
                    message = "Try Again Later.!";

                string url = "ProfilePage.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                CmdSave.Visible = true;
                return;
            }
            catch (Exception e)
            {
                scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');" + "</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname, true);
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "").Trim();
            return StrObj;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = $"{path}:  {DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff")} {Environment.NewLine}";
            Obj.WriteToFile($"{text}{ex.Message}");
            Response.Write("Try later.");
            return ""; // Or handle the exception as per your application's requirements
        }
    }
    protected void CmdSave_Click(object sender, EventArgs e)
    {
        UpdateDb();
    }
    private void FillCountryName()
    {
        try
        {
            DataTable Dt = new DataTable();
            strQuery = " SELECT CId,CountryName FROM M_CountryMaster WHERE ACTIVESTATUS='Y' ORDER BY CountryNAme";
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQuery).Tables[0];
            ddlCountryName.DataSource = Dt;
            ddlCountryName.DataValueField = "CId";
            ddlCountryName.DataTextField = "CountryName";
            ddlCountryName.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
    private void FillCountryMasterCode()
    {
        try
        {
            //DataTable dt = new DataTable();
            DataTable dt = new DataTable();
            strQuery = "SELECT StdCode FROM M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + ddlCountryName.SelectedValue + "'";
            dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQuery).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddlMobileNAme.Text = dt.Rows[0]["StdCode"].ToString();
            }
        }
        // ddlMobileNAme.DataSource = dt
        // ddlMobileNAme.DataValueField = "StdCode"
        // ddlMobileNAme.DataBind()

        catch (Exception ex)
        {
        }
    }
}
