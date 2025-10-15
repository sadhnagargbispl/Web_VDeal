using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FoodBookingRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string Str = "SELECT *, (MemFirstname + ' ' + MemLastname) AS Name FROM M_memberMaster WHERE Formno = '" + Convert.ToInt32(Session["formno"]) + "'";
            DataTable Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                string userInfo;
                userInfo = Dt.Rows[0]["Idno"].ToString().Trim() + ";" + Dt.Rows[0]["Passw"].ToString();
                string encryptedUserInfo = EncryptData(userInfo);
                string url = "https://food.vdeal.in/controller.aspx?user_info=" + encryptedUserInfo + "&log_key=749518F8-6DDA-4AAE-9A02-A0EE820E636F";
                Response.Redirect(url);
            }
            else
            {
                Response.Redirect("https://food.vdeal.in/");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private string EncryptData(string data)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[data.Length];
        encode = Encoding.UTF8.GetBytes(data);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }

}