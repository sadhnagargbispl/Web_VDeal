using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net;
using System.Text;


/// <summary>
/// Summary description for DAL
/// </summary>
public class DAL
{
    public SqlConnection objSQlConnection;

    public SqlConnection objSQlConnection1;

    public SqlDataAdapter sda = new SqlDataAdapter();
    public DataTable dt = new DataTable();
    public HttpRequest request;
    public string AppUrl = "";
    public SqlConnectionStringBuilder Connection = new SqlConnectionStringBuilder();
    public SqlConnectionStringBuilder Connection1 = new SqlConnectionStringBuilder();
    private SqlCommand sqlCmd = new SqlCommand();
    public string activeCondition = "RowStatus='Y'";
    public string tblUserGrpMaster = "M_UserGroupMaster";
    public string tblUserMaster = "M_UserMaster";
    public string tblStateMaster = "M_StateDivMaster";
    public string tblDistrictMaster = "M_DistrictMaster";
    public string tblCityStateMaster = "M_CityStateMaster";
    public string tblBankMaster = "M_BankMaster";
    public string tblNewsMaster = "M_NewsSeminarMaster";
    public string tblNewsTypeMaster = "M_NewsTypeMaster";
    public string tblCountryMaster = "M_CountryMaster";
    public string tblKitMaster = "M_KitMaster";
    public string tblKitMaster2 = "M_KitMaster2";
    public string tblAchieverMaster = "M_AchieverMaster";
    public string tblMeetingMaster = "M_MeetingMaster";
    public string tblUserPermision = "M_UserPermissionMaster";
    public string tblMenuMaster = "M_WebMenuMaster";
    public string tblSearchCriteria = "M_SearchCriteriaMaster";
    public string tblMemberMaster = "M_MemberMaster";
    public string tblKitProductMaster = "M_KitProductMaster";
    public string tblCTypeMaster = "M_ComplaintTypeMaster";
    private string _ConnectionString;
    public string IsoStart = " Alter Database joshmart Set Allow_SnapShot_isolation on; Set Transaction Isolation level read uncommitted;Set nocount on; Begin Tran  ";
    public string IsoEnd = " Commit Tran ";
    public string dBName = "joshmart";

    public DAL()
    {
        // Setting Connection String
        // Connection.ConnectionString = "Server=164.132.18.172;UID=ubgshp;PWD=S#09B!81ee$;Database=BigShopee;Pooling=False;Connect Timeout=1500000000"
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        objSQlConnection = new SqlConnection(Connection.ConnectionString);

        Connection1.ConnectionString = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
        objSQlConnection1 = new SqlConnection(Connection1.ConnectionString);
    }

    public int SaveData(string qry)
    {
        int a = 0;
        try
        {


            // objSQlConnection.Open()
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            sqlCmd = new SqlCommand(qry, objSQlConnection);
            a = sqlCmd.ExecuteNonQuery();
            // objSQlConnection.Close()
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();

        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return a;
    }
    public int SaveData1(string qry)
    {
        int a = 0;
        try
        {
            // objSQlConnection.Open()
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            sqlCmd = new SqlCommand(qry, objSQlConnection);
            sqlCmd.CommandTimeout = 0;
            a = sqlCmd.ExecuteNonQuery();
            // objSQlConnection.Close()
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();
            return a;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return a;
    }

    public DataTable GetData(string qry)
    {
        DataTable tempDt = new DataTable();
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            sda = new SqlDataAdapter(qry, objSQlConnection);
            dt = new DataTable();
            sda.Fill(dt);
            tempDt = dt;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return tempDt;
    }

    public DataTable GetDataWithIso(string qry)
    {
        DataTable tempDt = new DataTable();
        try
        {
            if (objSQlConnection1.State == ConnectionState.Closed)
                objSQlConnection1.Open();

            qry = IsoStart + qry + IsoEnd;

            sda = new SqlDataAdapter(qry, objSQlConnection1);
            dt = new DataTable();
            sda.Fill(dt);
            tempDt = dt;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection1 == null))
            {
                if (objSQlConnection1.State == ConnectionState.Open)
                    objSQlConnection1.Close();
            }
        }
        return tempDt;
    }


    public int UpdateData(string qry, string ParaName = "", string ParaValue = "")
    {
        // objSQlConnection.Open()
        int j = 0;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            string[] strParaName, strParaValue;
            int i = 0;

            if (string.IsNullOrEmpty(ParaName) == true & string.IsNullOrEmpty(ParaValue) == true)
                sqlCmd = new SqlCommand(qry, objSQlConnection);
            else
            {
                strParaName = ParaName.Split(';');
                strParaValue = ParaValue.Split(';');
                sqlCmd = new SqlCommand(qry, objSQlConnection);
                for (i = 0; i <= strParaName.Count() - 1; i++)
                    sqlCmd.Parameters.AddWithValue(strParaName[i], strParaValue[i]);
            }
            int a = sqlCmd.ExecuteNonQuery();
            // objSQlConnection.Close()
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();
            j = a;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return j;
    }

    public int ExecuteProcedure(string procname, string ParaName = "", string ParaValue = "")
    {
        int j = 0;
        try
        {
            SqlCommand cmd = objSQlConnection.CreateCommand();
            string[] strParaName, strParaValue;
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            // objSQlConnection.Open()
            cmd.CommandType = CommandType.StoredProcedure;
            if (string.IsNullOrEmpty(ParaName) == false & string.IsNullOrEmpty(ParaValue) == false)
            {

                strParaName = ParaName.Split(';');
                strParaValue = ParaValue.Split(';');
                for (var i = 0; i <= strParaName.Count() - 1; i++)
                    cmd.Parameters.AddWithValue(strParaName[i], strParaValue[i]);
            }
            cmd.CommandText = procname;
            int a = cmd.ExecuteNonQuery();
            // objSQlConnection.Close()
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();
            j = a;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return j;
    }

    public DataTable GenerateTreeProc(string strqry)
    {

        DataTable tempDt = new DataTable();
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            SqlCommand comm = new SqlCommand(strqry, objSQlConnection);
            comm.CommandTimeout = 100000000;
            SqlDataAdapter sda = new SqlDataAdapter(comm);
            dt = new DataTable();
            sda.Fill(dt);
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();
            tempDt = dt;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return tempDt;
    }

    public DataSet ExecProcDataSet(string strqry)
    {
        DataSet dsGetData = new DataSet();
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            SqlCommand comm = new SqlCommand(strqry, objSQlConnection);
            comm.CommandTimeout = 100000000;
            SqlDataAdapter sda = new SqlDataAdapter(comm);
            // dt = New DataTable
            sda.Fill(dsGetData);
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return dsGetData;
    }

    public int UpdateMultipleFields(string tblName, string ParaName = "", string ParaValue = "", string whereCond = "")
    {

        int j = 0;
        try
        {
            string SubPart = "";
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();
            string[] strParaName, strParaValue;
            int i = 0;

            if (string.IsNullOrEmpty(ParaName) == true & string.IsNullOrEmpty(ParaValue) == true)
            {
            }
            else
            {
                strParaName = ParaName.Split(';');
                strParaValue = ParaValue.Split(';');
                for (i = 0; i <= strParaName.Count() - 1; i++)
                    SubPart = SubPart + strParaName[i] + "='" + strParaValue[i] + "',";
            }
            SubPart = SubPart.Remove(SubPart.Length - 1, 1);

            string qry = " update " + tblName + " set " + SubPart + whereCond;
            sqlCmd = new SqlCommand(qry, objSQlConnection);
            int a = sqlCmd.ExecuteNonQuery();
            // Dim a As Integer = sqlCmd.ExecuteNonQuery()
            // objSQlConnection.Close()
            if (objSQlConnection.State == ConnectionState.Open)
                objSQlConnection.Close();
            j = a;
        }
        catch (Exception ex)
        {
            if (!(objSQlConnection == null))
            {
                if (objSQlConnection.State == ConnectionState.Open)
                    objSQlConnection.Close();
            }
        }
        return j;
    }
    public string ClearInject(string StrObj)
    {
        try
        {
            //StrObj = Strings.Replace(Strings.Replace(Strings.Replace(StrObj, ";", ""), "'", ""), "=", "");
            StrObj = StrObj.Trim().Replace(";", "").Replace("'", "").Replace("=", "");
        }
        catch (Exception ex)
        {
        }
        return StrObj;
    }

    public void FillCombo(string qry, ref DropDownList CmbNm, string DisFld, string ValFld)
    {
        try
        {
            dt = new DataTable();
            dt = GetData(qry);
            {
                var withBlock = CmbNm;
                withBlock.DataSource = dt;
                withBlock.DataTextField = DisFld;
                withBlock.DataValueField = ValFld;
                withBlock.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void WriteToFile(string text)
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath("~/images/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
                writer.WriteLine("--------------------------------------------------------");
                writer.Close();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public bool SendSMS(string sms, string mobl, string senderid)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                string baseUrl = "https://www.fast2sms.com/dev/bulkV2";

                // Prepare the parameters
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("authorization", senderid);
                parameters.Add("message", sms);
                parameters.Add("language", "english");
                parameters.Add("route", "v3");
                parameters.Add("numbers", mobl);

                // Encode and send the request
                byte[] responseBytes = client.UploadValues(baseUrl, "POST", parameters);
                string responseString = Encoding.UTF8.GetString(responseBytes);

                // Check the response for success or failure
                if (responseString.Contains("success"))
                {
                    return true;
                }
                else
                {
                    // Log the error or handle it as needed
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return false;
        }
    }
}