using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MovieBookingRedirect : System.Web.UI.Page
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
                string idno = Dt.Rows[0]["Idno"].ToString().Trim();
                string passw = Dt.Rows[0]["Passw"].ToString();
                string name = Dt.Rows[0]["Name"].ToString().Trim();
                string email = Dt.Rows[0]["Email"].ToString().Trim();
                string mobl = Dt.Rows[0]["Mobl"].ToString().Trim();
                string doj = Convert.ToDateTime(Dt.Rows[0]["doj"]).ToString("dd-MMM-yyyy");
                string userInfo;
                userInfo = Dt.Rows[0]["Idno"].ToString().Trim() + ";" + Dt.Rows[0]["Passw"].ToString() + ";" + Dt.Rows[0]["Name"].ToString().Trim() + ";" + Dt.Rows[0]["Email"].ToString().Trim() + ";" + Dt.Rows[0]["Mobl"].ToString().Trim() + ";" + Convert.ToDateTime(Dt.Rows[0]["doj"]).ToString("dd-MMM-yyyy");
                string encryptedUserInfo = EncryptData(userInfo);
                string url = "https://movie.vdeal.in/controller.aspx?user_info=" + encryptedUserInfo + "&log_key=AB7B4CD2-40E6-45CD-BF14-7F29A38F5FA8";
                Response.Redirect(url);
            }
            else
            {
                Response.Redirect("https://movie.vdeal.in/");
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