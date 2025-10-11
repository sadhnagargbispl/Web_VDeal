using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PointWallet : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!IsPostBack)
                {
                    FillBalance();
                    FillDetail();
                }       
            }
            else
            {
                Response.Redirect("Logout.aspx");
                Response.End();
            }      
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private void FillBalance()
    {
        try
        {
            string query = "Select * From dbo.ufnGetBalance('" + Session["FormNo"] + "','M')";
            DataTable Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, query).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                LblDeposit.Text = Dt.Rows[0]["Credit"].ToString();
                LblUsed.Text = Dt.Rows[0]["Debit"].ToString();
                LblBalance.Text = Dt.Rows[0]["Balance"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillDetail()
    {
        try
        {
            string query = " EXEC SP_GetPointWalletDetail '" + Session["Formno"] + "' ";
            DataTable Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, query).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                Session["MainFund"] = Dt;
                RptDirects.DataSource = Dt;
                RptDirects.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RptDirects.PageIndex = e.NewPageIndex; // Assuming 'e' is an event argument passed to this method
            FillDetail();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}