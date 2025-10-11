using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FundRequestReport : System.Web.UI.Page
{
    string strquery;
    DataTable dt;
    DAL Obj = new DAL();
    string IsoStart;
    string IsoEnd;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Status"] == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Page.IsPostBack)
            {
                PaymentDetails();
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void RptDirects_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            RptDirects.PageIndex = e.NewPageIndex;
            PaymentDetails();
        }
        catch (Exception ex)
        {
            // Handle page index change exception if necessary
        }
    }
    private void PaymentDetails()
    {
        try
        {
            DataTable dtData = new DataTable();
            string Condition = "";
            strquery = " select ReqNo,Replace(Convert(Varchar,a.RecTimeStamp,106),' ','-')+ ' '+  " + " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,";
            strquery += "PayMode,Chqno,Replace(Convert(Varchar,ChqDate,106),' ','-') as ChequeDate," + " b.BankName,a.Branchname,";
            strquery += "Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" + " end as status,";
            strquery += "Amount,a.Remarks,Case when ScannedFile='' then '' when scannedfile like'http%' then ScannedFile " + " else 'images/UploadImage/'+ ScannedFile end as ScannedFile " + " ,";
            strquery += "Case when ScannedFile='' then 'False' else 'True'";
            strquery += " end as ScannedFileStatus ";
            strquery += " from WalletReq " + " as a,M_BankMaster as b where a.BankId=b.BankCode and b.RowStatus='Y' and a.Formno='" + Session["Formno"] + "' order by ReqNo desc ";
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strquery).Tables[0];
            if (dtData.Rows.Count > 0)
            {
                Session["ReceivedPin"] = dtData;
                RptDirects.DataSource = dtData;
                RptDirects.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
}
