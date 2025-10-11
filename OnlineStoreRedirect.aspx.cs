using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OnlineStoreRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://store.joshmart.ai/");
            }
            else
            {
                try
                {
                    string formPostText = string.Empty;
                    string UserName = Encrypt(Session["IDNO"].ToString());
                    string Password = Encrypt(Session["MemPassw"].ToString());

                    formPostText = "<form method=\"POST\" action=\"https://store.joshmart.ai/Account/DirectLogin\" name=\"frm2Post\">" +
                                   " <input type=\"hidden\" name=\"LoginId\" value=\"" + UserName + "\" />" +
                                   " <input type=\"hidden\" name=\"Password\" value=\"" + Password + "\" /> " +
                                   " <input type=\"hidden\" name=\"Token\" value=\"aMogVqwfBLFntHkVxMGj7KoK5jha/7tVa+vM6TTn+YyuwOB2mI9XqVp8PYKWkspI\" />" +
                                   " <script type=\"text/javascript\">document.frm2Post.submit();</script></form>";

                    Response.Write(formPostText);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public string Encrypt(string plainText)
    {
        string completeUrl = "https://store.joshmart.ai/Account/Encrypt?plainText=" + plainText;
        string customerOTPmessage = string.Empty;
        string amount = string.Empty;

        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // TLS 1.2
        HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
        request1.ContentType = "application/json";
        request1.Method = "GET";

        using (HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse())
        {
            using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                string responseString = reader.ReadToEnd();
                return responseString;
            }
        }
    }
    public string Decrypt(string cipherText)
    {
        // AES decryption logic
        byte[] iv = new byte[16]; // 16-byte IV for AES
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes("Y$8tM9d#k4KqpV^rLw2zXN&yS7uPqU@3"); // 32-byte key for AES-256
            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}