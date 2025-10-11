<%@ Application Language="C#" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["Connect"] = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        Application["Connect1"] = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        getData();
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    public void getData()
    {
        DAL objdal = new DAL();
        try
        {
            DataTable dtCompany = new DataTable();

            if (Application["dtCompany"] == null)
            {
                DataSet ds = new DataSet();
                string strQ = " select * from M_CompanyMaster";
                ds = SqlHelper.ExecuteDataset(Application["Connect1"].ToString(), CommandType.Text, strQ);
                dtCompany = ds.Tables[0];
                Application["dtCompany"] = dtCompany;
            }
            else
                dtCompany = (DataTable)Application["dtCompany"];
            if (dtCompany.Rows.Count > 0)
            {
                Session["CompName"] = dtCompany.Rows[0]["CompName"].ToString();
                Session["CompAdd"] = dtCompany.Rows[0]["CompAdd"].ToString();
                Session["CompWeb"] = string.IsNullOrEmpty(dtCompany.Rows[0]["WebSite"].ToString()) ? "index.asp" : dtCompany.Rows[0]["WebSite"].ToString();
                Session["Title"] = dtCompany.Rows[0]["CompTitle"].ToString();
                Session["CompMail"] = dtCompany.Rows[0]["CompMail"].ToString();
                Session["CompMobile"] = dtCompany.Rows[0]["MobileNo"].ToString();
                Session["ClientId"] = dtCompany.Rows[0]["smsSenderId"].ToString();
                Session["SmsId"] = dtCompany.Rows[0]["smsUserNm"].ToString();
                Session["SmsPass"] = dtCompany.Rows[0]["smPass"].ToString();
                Session["MailPass"] = dtCompany.Rows[0]["mailPass"].ToString();
                Session["MailHost"] = dtCompany.Rows[0]["mailHost"].ToString();
                Session["AdminWeb"] = dtCompany.Rows[0]["AdminWeb"].ToString();
                Session["CompCST"] = dtCompany.Rows[0]["CompCSTNo"].ToString();
                Session["CompState"] = dtCompany.Rows[0]["CompState"].ToString();
                Session["CompDate"] = Convert.ToDateTime(dtCompany.Rows[0]["RecTimeStamp"]).ToString("dd-MMM-yyyy");
                Session["Spons"] = "KL223344";
                Session["CompWeb1"] = dtCompany.Rows[0]["WebSite"].ToString();
                Session["CompMovieWeb"] = "";
                Session["SmsAPI"] = "";
                Session["CompShortUrl"] = dtCompany.Rows[0]["UrlShort"].ToString();
            }
            else
            {
                Session["CompName"] = "";
                Session["CompAdd"] = "";
                Session["CompWeb"] = "";
                Session["Title"] = "Welcome";
            }

            DataTable dtConfig = new DataTable();
            if (Application["dtConfig"] == null)
            {
                DataSet ds = new DataSet();
                string strQ = "";
                strQ = " select * from M_ConfigMaster ";
                ds = SqlHelper.ExecuteDataset(Application["Connect1"].ToString(), CommandType.Text, strQ);
                dtConfig = ds.Tables[0];
                Application["dtConfig"] = dtConfig;
            }
            else
                dtConfig = (DataTable)Application["dtConfig"];
            if (dtConfig.Rows.Count > 0)
            {
                Session["IsGetExtreme"] = dtConfig.Rows[0]["IsGetExtreme"].ToString();
                Session["IsTopUp"] = dtConfig.Rows[0]["IsTopUp"].ToString();
                Session["IsSendSMS"] = dtConfig.Rows[0]["IsSendSMS"].ToString();
                Session["IdNoPrefix"] = dtConfig.Rows[0]["IdNoPrefix"].ToString();
                Session["IsFreeJoin"] = dtConfig.Rows[0]["IsFreeJoin"].ToString();
                Session["IsStartJoin"] = dtConfig.Rows[0]["IsStartJoin"].ToString();
                Session["JoinStartFrm"] = dtConfig.Rows[0]["JoinStartFrm"].ToString();
                Session["IsSubPlan"] = dtConfig.Rows[0]["IsSubPlan"].ToString();
                Session["Logout"] = "Default.aspx";

            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
                Session["Logout"] = "https://BunnyFx tech.com/";

            }
            DataTable dtMsession = new DataTable();
            if (Application["dtMsession"] == null)
            {

                DataSet ds = new DataSet();
                string strQ = "";
                strQ = " select Max(SEssid) as SessID from D_Monthlypaydetail  ";
                ds = SqlHelper.ExecuteDataset(Application["Connect1"].ToString(), CommandType.Text, strQ);
                dtMsession = ds.Tables[0];
                Application["dtMsession"] = dtMsession;
            }
            else
                dtMsession = (DataTable)Application["dtMsession"];
            if (dtMsession.Rows.Count > 0)
                Session["MaxSessn"] = dtMsession.Rows[0]["SessID"];
            else
                Session["MaxSessn"] = "";
            DataTable dtsession = new DataTable();
            if (Application["dtsession"] == null)
            {
                DataSet ds = new DataSet();
                string strQ = "";
                strQ = " select Max(SEssid) as SessID from m_SessnMaster  ";
                ds = SqlHelper.ExecuteDataset(Application["Connect1"].ToString(), CommandType.Text, strQ);
                dtsession = ds.Tables[0];
                Application["dtsession"] = dtsession;
            }
            else
                dtsession = (DataTable)Application["dtsession"];
            if (dtsession.Rows.Count > 0)
                Session["CurrentSessn"] = dtsession.Rows[0]["SessID"];
            else
                Session["CurrentSessn"] = "";
        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }

</script>
