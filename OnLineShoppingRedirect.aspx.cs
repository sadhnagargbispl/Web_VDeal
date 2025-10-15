using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OnLineShoppingRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://brands.vdeal.in/");
            }
            else
            {
                string formPostText = "";
                formPostText = "<form method=\"POST\" action=\"https://brands.vdeal.in/members/index.php\" name=\"frm2Post\">";
                formPostText += "<input type=\"hidden\" name=\"token\" value=\"460673b7c80c4d29c61dfea0d82c401a\" />";
                formPostText += "<input type=\"hidden\" name=\"mod\" value=\"interLogin\" />";
                formPostText += "<input type=\"hidden\" name=\"userid\" value=\"" + Session["IDNO"].ToString() + "\" />";
                formPostText += "<input type=\"hidden\" name=\"password\" value=\"" + Session["MemPassw"].ToString() + "\" /> ";
                formPostText += "<script type=\"text/javascript\">document.frm2Post.submit();</script>";
                formPostText += "</form>";
                Response.Write(formPostText);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during this process
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