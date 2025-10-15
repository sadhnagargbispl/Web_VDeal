using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HolidayPortalRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://holiday.vdeal.in/");
            }
            else
            {
                string Token = "A75AFBA0292C4AE59DFD7D56F96C8B06";
                string info12 = "UserName=" + Convert.ToString(Session["IDNO"]) + "&Password=" + Convert.ToString(Session["MemPassw"]) + "&Action=Login" + "&Token=" + Token;
                string refes = Base64Encode(info12);
                string Url = "https://holiday.vdeal.in/Account/DirectLogin?URL=" + refes;
                Response.Redirect(Url);
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
