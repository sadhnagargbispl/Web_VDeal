using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Application["WebStatus"] != null)
            {
                if (Application["WebStatus"].ToString() == "N")
                {
                    Session.Abandon();
                    Response.Redirect("default.aspx", false);
                }
            }

            if (!Page.IsPostBack)
            {
                DataTable dt = new DataTable();
                if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                {
                    string sql = "select profilepic, * from M_MemberMaster where formno=" + Session["FormNo"];
                    dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        LblIDno.Text = dt.Rows[0]["idno"].ToString();
                        LblName.Text = dt.Rows[0]["memfirstname"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }

    }
}
