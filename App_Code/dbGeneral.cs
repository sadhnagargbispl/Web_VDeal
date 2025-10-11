using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for dbGeneral
/// </summary>
public class dbGeneral
{
    public dbGeneral()
    {

    }
    public void Fill_Date_box(DropDownList cday, DropDownList cMonth, DropDownList cYear, int yearStart = 1950, int yearEnd = 2010)
    {
        for (int i = 1; i <= 31; i++)
        {
            cday.Items.Add(i.ToString().PadLeft(2, '0'));
        }

        for (int i = 1; i <= 12; i++)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i);
            cMonth.Items.Add(monthName.Trim().Substring(0, 3).ToUpper());
        }

        for (int i = yearStart; i <= yearEnd; i++)
        {
            cYear.Items.Add(i.ToString());
        }
    }
    public void FillCmb(ref DropDownList Cmb, ref DataTable strTbl, string strValFld, string strTxtFld)
    {
        Cmb.DataSource = strTbl;
        Cmb.DataValueField = strValFld;
        Cmb.DataTextField = strTxtFld;
        Cmb.DataBind();
    }
    private Random random = new Random();
    private int RandomNumber(int min, int max)
    {
        return random.Next(min, max);
    }
    private string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
        {
            return builder.ToString().ToLower();
        }
        return builder.ToString();
    }
    public string GenerateRandomCode()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(1, true));
        builder.Append(RandomNumber(1, 9));
        builder.Append(RandomString(1, true));
        builder.Append(RandomNumber(1, 9));
        builder.Append(RandomString(1, true));
        return builder.ToString();
    }
    public string MyMsgBx(string sMessage)
    {
        string msg = "<script language='javascript'>";
        msg += "alert('" + sMessage + "');";
        msg += "</script>";
        return msg;
    }
    public string ClrAllCtrl()
    {
        string msg = "<script language='javascript'>";
        msg += "rstCtrl();";
        msg += "</script>";
        return msg;
    }
}