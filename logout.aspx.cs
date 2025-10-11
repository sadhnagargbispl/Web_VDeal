using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        string nextpage = Session["Logout"] as string;
        nextpage = "index.aspx";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.SetAllowResponseInBrowserHistory(false);
        Response.Cache.SetNoStore();
        Session.Abandon();
        Session.Clear();
        Response.Cookies.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Session["Status"] = "";
        Session["FormNo"] = "";
        Session["Idno"] = "";
        Session["MemName"] = "";
        Session["RefIncome"] = "";
        Session["KitId"] = "";
        Session["Uid"] = "";
        Session["CkyPinTransfer"] = "";
        Response.Redirect(nextpage);

    }

}