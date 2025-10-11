using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyPurchase : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    FillData();
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
    public void FillData()
    {
        string str = "";
        try
        {
            str = "select BillNo as [Order No.],Kitamount as [Order Amount], a.BV, kitname as [Pakage Name], replace(convert(varchar, billdate, 106), ' ', '-') As [Activation Date] ";
            str += "from mM_kitmaster as A, Repurchincome as C where a.kitid = c.kitid AND c.formno = '" + Session["formno"].ToString() + "' order by rid desc";
            DataTable Dt_ = new DataTable();
            Dt_ = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
            if (Dt_.Rows.Count > 0)
            {
                gv.DataSource = Dt_;
                gv.DataBind();
                Session["DirectData1"] = Dt_;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}