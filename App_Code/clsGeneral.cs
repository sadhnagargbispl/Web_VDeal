//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

///// <summary>
///// Summary description for clsGeneral
///// </summary>
//public class clsGeneral
//{
//    public clsGeneral()
//    {
//        //
//        // TODO: Add constructor logic here
//        //
//    }
//}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using Irony;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web;

public class clsGeneral
{
    public SqlConnection objSQlConnection;
    public System.Data.SqlClient.SqlConnectionStringBuilder Connection = new System.Data.SqlClient.SqlConnectionStringBuilder();


    //public void Fill_Date_box(ref DropDownList cday, ref DropDownList cMonth, ref DropDownList cYear, int YearStart = 1950, int yearEnd = 2010)
    //{
    //    for (Int16 i = 1; i <= 31; i++)
    //        cday.Items.Add(i.ToString().PadLeft(2, "0"));

    //    for (Int16 i = 1; i <= 12; i++)
    //        cMonth.Items.Add(Strings.Left(DateTime.MonthName(i), 3).Trim().ToUpper());

    //    for (int i = YearStart; i <= yearEnd; i++)
    //        cYear.Items.Add(i);
    //}

    public void FillCmb(ref DropDownList Cmb, ref DataTable strTbl, ref string strValFld, ref string strTxtFld)
    {
        {
            var withBlock = Cmb;
            withBlock.DataSource = strTbl;
            withBlock.DataValueField = strValFld;
            withBlock.DataTextField = strTxtFld;
            withBlock.DataBind();
        }
    }


    private int RandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    } // RandomNumber 

    private string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        int i;
        for (i = 0; i <= size - 1; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    } // RandomString 

    public string GenerateRandomCode()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(1, true));
        builder.Append(RandomNumber(1, 9));
        builder.Append(RandomString(1, true));
        builder.Append(RandomNumber(1, 9));
        builder.Append(RandomString(1, true));
        string myRandomStr;
        myRandomStr = builder.ToString();
        return myRandomStr;
    } // GenerateRandomCode 
    public string myMsgBx(string sMessage)
    {
        string msg;
        msg = "<script language='javascript'>";
        msg += "alert('" + sMessage + "');";
        msg += "</script>";
        return msg;
    }
    public string ClrAllCtrl()
    {
        string msg;
        msg = "<script language='javascript'>";
        msg += " rstCtrl(); ";
        msg += "</script>";
        return msg;
    }
    public void WriteToFile(string text, SqlHelper sqlHelper)
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath("~/images/ErrorLog.txt");
            // Using writer As New StreamWriter(path, True)
            // writer.WriteLine(text)
            // writer.WriteLine("--------------------------------------------------------")
            // writer.Close()
            // End Using
            string str = " insert Into TrnErrorLog(Pth,txt) Values('" + path + "','" + text + "--CompID = " + HttpContext.Current.Session["CompID"] + "')";
            int i = 0;
            //i = sqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, str);
            i = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, str);

        }
        catch (Exception ex)
        {
        }
    }

    public string GetConnectionByComp()
    {
        DataSet ds = new DataSet();
        string msg;
        string CompID = (string)HttpContext.Current.Session["CompID"]; // 'ConfigurationManager.AppSettings("CompanyID")
        string str = " Exec Proc_GetConnection1 '" + CompID + "' ";
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings["sconstr"].ConnectionString;

        objSQlConnection = new System.Data.SqlClient.SqlConnection(Connection.ConnectionString);
        ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str);
        msg = ds.Tables[0].Rows[0]["ConnectionString"].ToString();
        HttpContext.Current.Session["MlmDatabase" + CompID] = msg;
        int i = 0;
        try
        {
            string str1 = "IF object_id('TrnTemp') IS NULL Create Table TrnTemp ( ID int identity(1,1),Formno int not null, transNo Numeric (18,0) Primary Key, Remark Nvarchar(100) , Rectimestamp Datetime Default(Getdate()))";

            i = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, str1);
            
        }
        catch (Exception ex)
        {
        }


        try
        {
            string str2 = "IF object_id('TrnErrorLog') IS NULL Create table TrnErrorLog ( Id int Identity(1,1), Pth nvarchar(500), txt nvarchar(4000), Rectimestamp Datetime Default Getdate())";
            i = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, str2);
        }
        catch (Exception ex)
        {
        }

        return msg;
    }


    public string GetInvDataBaseByComp()
    {
        DataSet ds = new DataSet();
        string msg;
        string CompID = (string)HttpContext.Current.Session["CompID"]; // 'ConfigurationManager.AppSettings("CompanyID")
        string str = " Exec Proc_GetConnection1 '" + CompID + "' ";
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings["sconstr"].ConnectionString;
        objSQlConnection = new System.Data.SqlClient.SqlConnection(Connection.ConnectionString);
        ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str);
        msg = ds.Tables[1].Rows[0]["DatabaseName"].ToString();
        HttpContext.Current.Session["InvDatabase" + CompID] = msg;
        return msg;
    }


    public bool RegTrans(string TransID, string CompID, string Formno = "0")
    {
        bool @bool = false;
        try
        {
            int i = 0;
            string str = " insert Into TrnTemp(transNo,Formno) Values('" + TransID + "', '" + Formno + "')";
            i = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, str);
            if ((i == 1))
                @bool = true;
            else
                @bool = false;
        }
        catch (Exception ex)
        {
        }
        return @bool;
    }
}
