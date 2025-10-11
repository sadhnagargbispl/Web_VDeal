using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GiftVoucherRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://gv.joshmart.ai/index.php");
            }
            else
            {
                //string Token = "XAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJjb25zdW1lcklkIjo2ODYsImV4cCI6MTcwODM0MDc3OCwidG9rZW4iOiI2M";
                //string info12 = "UserName=" + Convert.ToString(Session["IDNO"]) + "&Password=" + Convert.ToString(Session["MemPassw"]) + "&Action=Login" + "&Token=" + Token;
                //string refes = Base64Encode(info12);
                //string data = Base64Decode(refes);
                //Response.Redirect("http://gv.joshmart.ai/Login/DirectLogin?URL=" + refes);

                string formPostText = "";
                formPostText = "<form method=\"POST\" action=\"https://gv.joshmart.ai/members/index.php\" name=\"frm2Post\">";
                formPostText += "<input type=\"hidden\" name=\"token\" value=\"03789e777ce4968dd7ad963d4af7d5a3\" />";
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
