using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        FUN_SP_GETLIST();
    }
    private void FUN_SP_GETLIST()
    {
        try
        {
            DataSet Ds = new DataSet();
            string Sql = "Exec Sp_GetRedeemCategories";
            Ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, Sql);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                RepFoodMovie.DataSource = Ds.Tables[0];
                RepFoodMovie.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}