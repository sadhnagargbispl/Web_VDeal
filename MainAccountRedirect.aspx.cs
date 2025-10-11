using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MainAccountRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private static readonly TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
    private static readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
    private static readonly string key = "sg75b79-nj48dh02";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://cpanel.vdeal.in/");
            }
            else
            {
                string LgnID = Encrypt("uid=" + Session["IDNo"] + "&pwd=" + Session["MemPassw"]);
                DateTime currentDate = DateTime.Now;
                string result = DateTime.Now.Day.ToString() +
                DateTime.Now.Hour.ToString() +
                DateTime.Now.Year.ToString() +
                (DateTime.Now.Month - 1).ToString();
                // string TmID = currentDate.Day.ToString("00") + currentDate.Hour.ToString("00") + currentDate.Year.ToString() + (currentDate.Month - 1).ToString();
                string url = "https://cpanel.vdeal.in/Default.aspx?lgnT=" + LgnID + "&ID=" + result;
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public static byte[] MD5Hash(string value)
    {
        return MD5.ComputeHash(Encoding.ASCII.GetBytes(value));
    }
    public static string Encrypt(string stringToEncrypt)
    {
        DES.Key = MD5Hash(key);
        DES.Mode = CipherMode.ECB;
        byte[] buffer = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
        return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
    }
    public static string Decrypt(string encryptedString)
    {
        try
        {
            DES.Key = MD5Hash(key);
            DES.Mode = CipherMode.ECB;
            byte[] buffer = Convert.FromBase64String(encryptedString);
            return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }
        catch (Exception ex)
        {
            return DBNull.Value.ToString();
        }
    }

}