using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MM_Voucher : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    ModuleFunction objModuleFun = new ModuleFunction();
    string CategoryID_;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["CategoryID"]))
        {
            CategoryID_ = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["CategoryID"]));
            FillKit(CategoryID_);
            FUN_SP_GETLIST(CategoryID_);
        }
        
    }
    private void FillKit(string kitid)
    {
        try
        {
            DataSet ds = new DataSet();
            string sql = "select * from RedeemCategories where id = '" + kitid + "' ";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblPackageName.Text = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                LblDis.Text = ds.Tables[0].Rows[0]["Description"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FUN_SP_GETLIST(string kitid)
    {
        try
        {
            DataSet Ds = new DataSet();
            string Sql = "Exec Sp_GetAllPageckeDetail '" + kitid + "'";
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