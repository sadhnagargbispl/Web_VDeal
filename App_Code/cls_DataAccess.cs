using System;
using System.Data;
using System.Data.SqlClient;

public class cls_DataAccess
{
    public SqlConnection cnnObject;
    public string _SerucityCode = "";
    private SqlCommand cmd;
    private SqlTransaction tran;
    private string _ConnectionString;

    public event EventHandler ConnectionOpen;

    public cls_DataAccess(string strConnectionString)
    {
        _ConnectionString = strConnectionString;
    }

    public string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj;
    }

    public SqlConnection OpenConnection()
    {
        try
        {
            if (cnnObject == null)
            {
                cnnObject = new SqlConnection(_ConnectionString);
            }

            if (cnnObject.State == ConnectionState.Closed ||
                cnnObject.State == ConnectionState.Broken)
            {
                cnnObject.Open();
                ConnectionOpen?.Invoke(this, EventArgs.Empty);
            }

            return cnnObject;
        }
        catch (Exception e)
        {
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
            return null;
        }
    }

    public void CloseConnection()
    {
        try
        {
            if (cnnObject.State == ConnectionState.Open)
            {
                cnnObject.Close();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }

    public string ExecuteScaller_old(string strQuery)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        try
        {
            cmd = new SqlCommand
            {
                Connection = cnnObject,
                CommandType = CommandType.Text,
                CommandText = strQuery
            };

            return cmd.ExecuteScalar().ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }

    public string ExistOrNot(string strQuery)
    {
        string _returnValue = "";

        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        SqlDataAdapter da = new SqlDataAdapter(strQuery, cnnObject);
        DataTable DTable = new DataTable();

        try
        {
            da.Fill(DTable);

            if (DTable.Rows.Count > 0)
            {
                _returnValue = DTable.Rows[0][0].ToString();
            }
        }
        catch (Exception)
        {
            // Handle the exception
        }
        finally
        {
            da.Dispose();
            DTable = null;
        }

        return _returnValue;
    }

    public DateTime GetServerDate()
    {
        string SqlD;
        DataSet SqlDs = new DataSet();

        // SqlD = "Select Convert(char (13),getdate(),113) +    convert(varchar(10),getdate(),108)   as Dts"
        SqlD = "Select Cast(Convert(Varchar,Getdate(),106) as DateTime) as Dts";
        // SqlD = "Select GetDate() as Dts"

        Fill_DataSET_Tables(SqlD, ref SqlDs, "MyDate");
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        if (SqlDs.Tables["MyDate"].Rows.Count > 0)
        {
            return Convert.ToDateTime(SqlDs.Tables["MyDate"].Rows[0]["Dts"]);
        }
        else
        {
            return DateTime.Now.Date;
        }
    }
    public int Fire_Query_For_Procedure(string Query)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        int affectedRows = 0;

        try
        {
            tran = cnnObject.BeginTransaction();
            cmd = new SqlCommand(Query, cnnObject);
            cmd.CommandTimeout = 0;
            cmd.Transaction = tran;
            cmd.ExecuteNonQuery();
            tran.Commit();
            affectedRows = 1;
            return affectedRows;
        }
        catch (Exception e)
        {
            tran.Rollback();
            //MessageBox.Show(e.Message);
            //MyLogError.WriteFile(e.Message);
            throw e;
        }
        finally
        {
            cmd = null;
            //  tran = null;
        }
    }
    public int Fire_Query(string Query)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        int affectedRows = 0;

        try
        {
            tran = cnnObject.BeginTransaction();
            cmd = new SqlCommand(Query, cnnObject);
            cmd.CommandTimeout = 0;
            cmd.Transaction = tran;
            affectedRows += cmd.ExecuteNonQuery();
            tran.Commit();
            return affectedRows;
        }
        catch (Exception e)
        {
            tran.Rollback();
            //MessageBox.Show(e.Message);
            throw e;
        }
        finally
        {
            cmd = null;
            //  tran = null;
        }
    }
    public DataTable Fill_Data_Tables(string strQuery, ref DataTable DTable)
    {
        if (cnnObject == null)
        {
            OpenConnection();
        }

        SqlDataAdapter da = new SqlDataAdapter(strQuery, cnnObject);
        DTable = new DataTable();
        try
        {
            // da.SelectCommand.Transaction = tran;
            da.Fill(DTable);
            // tran.Commit();
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
            return null;
        }
        finally
        {
            da.Dispose();
        }
        return DTable;
    }
    public DataTable Fill_Data_Tables_new(ref SqlTransaction tran, string strQuery, ref DataTable DTable)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        SqlDataAdapter da = new SqlDataAdapter(strQuery, cnnObject);
        DTable = new DataTable();

        try
        {
            da.Fill(DTable);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message + " Error in Filling Data " + strQuery.ToString);
            return null;
        }
        finally
        {
            da.Dispose();
        }

        return DTable;
    }
    public void Fill_DataSET_Tables(string strQuery, ref DataSet DS, string strTableName)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        SqlDataAdapter da = new SqlDataAdapter(strQuery, cnnObject);

        try
        {
            da.Fill(DS, strTableName);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message + " Error in Filling Data " + strQuery.ToString);
            return;
        }
        finally
        {
            da.Dispose();
        }
    }
    public string returnRandom(int iLen)
    {
        string random = "";

        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        SqlCommand cmm;
        SqlDataReader dRead;

        try
        {
            cmm = new SqlCommand("select Left(NewId()," + iLen + ") as RandomNum", cnnObject);
            dRead = cmm.ExecuteReader();

            if (!dRead.Read())
            {
                dRead.Close();
            }
            else
            {
                random = dRead["RandomNum"].ToString();
                return random;
            }
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message + " Error in Filling Data " + strQuery.ToString);
            return "";
        }
        finally
        {
            
        }

        return random;
    }
}
