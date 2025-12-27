using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PetrocardRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://gv.vdeal.in/index.php");
            }
            else
            {
                string formPostText = "";
                formPostText = "<form method=\"POST\" action=\"https://gv.vdeal.in/members/index.php\" name=\"frm2Post\">";
                formPostText += "<input type=\"hidden\" name=\"token\" value=\"aef26b5beff2beb16d88f6530578d518\" />";
                formPostText += "<input type=\"hidden\" name=\"mod\" value=\"product_detail_electro\" />";
                formPostText += "<input type=\"hidden\" name=\"userid\" value=\"" + Session["IDNO"].ToString() + "\" />";
                formPostText += "<input type=\"hidden\" name=\"password\" value=\"" + Session["MemPassw"].ToString() + "\" /> ";
                formPostText += "<script type=\"text/javascript\">document.frm2Post.submit();</script>";
                formPostText += "</form>";
                Response.Write(formPostText);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string Base64Encode(string plainText)
    {
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    private string Base64Decode(string base64EncodedData)
    {
        byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

}
