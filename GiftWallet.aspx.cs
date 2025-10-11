using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;

public partial class GiftWallet : System.Web.UI.Page
{
    private cls_DataAccess dbConnect;
    private clsGeneral dbGeneral = new clsGeneral();
    string scrname;
    DAL Obj = new DAL();
    DataSet Ds;
    DataTable Dt = new DataTable();
    string IsoStart;
    string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    FillWallet();
                }
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            Response.Redirect("logout.aspx");
        }
    }

    private void FillWallet()
    {
        try
        {
            DataSet Ds = SqlHelper.ExecuteDataset(constr, "sp_GetWallettype");
            Rbtnwallet.DataSource = Ds.Tables[0];
            Rbtnwallet.DataValueField = "Actype";
            Rbtnwallet.DataTextField = "Walletname";
            Rbtnwallet.DataBind();
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
            string query = IsoStart + "Select * From dbo.ufnGetBalance('" + Session["FormNo"] + "','" + Rbtnwallet.SelectedValue + "')" + IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                MCredit.InnerText = Dt.Rows[0]["Credit"].ToString();
                MDebit.InnerText = Dt.Rows[0]["Debit"].ToString();
                MBal.InnerText = Dt.Rows[0]["Balance"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    private void Fill_Grid()
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + " Exec Sp_GetAllWalletDetail '" + Session["Formno"] + "','" + Rbtnwallet.SelectedValue + "'" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = Ds.Tables[0];
            RptDirects.DataSource = dt;
            RptDirects.DataBind();
        }
        catch (Exception ex)
        {
        }
    }

    protected void RptDirects_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            RptDirects.PageIndex = e.NewPageIndex;
            Fill_Grid();
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        FillBalance();
        Fill_Grid();
    }
}
