using System;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Newtonsoft.Json;
using QRCoder;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
public partial class TestApi : System.Web.UI.Page
{
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    public string QRCodeBase64 { get; set; }
    private static string API_KEY = "67215889-9886-4900-8177-38b276e107b6";

    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Write(GenerateAddressAsync());
        }
    }
    static string makeAPICall()
    {
        var URL = new UriBuilder("https://sandbox-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["start"] = "1";
        queryString["limit"] = "5000";
        queryString["convert"] = "USD";

        URL.Query = queryString.ToString();

        var client = new WebClient();
        client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
        client.Headers.Add("Accepts", "application/json");
        return client.DownloadString(URL.ToString());

    }
    
    private async Task GenerateAddressAsync()
    {
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        string appId = "a4tcpLeAUW9Zaogy";
        string appSecret = "165d35b8912071d978c8c14911207131";
        string url = "https://ccpayment.com/ccpayment/v2/getOrCreateUserDepositAddress";
        hdnPayID.Value = Session["idno"].ToString();
        // Prepare content with chain set to SOLANA
        var content = new
        {
            userId = Session["idno"],
            chain = "MMIT"
        };
        string body = JsonConvert.SerializeObject(content);
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string signText = appId + timestamp + (body.Length != 2 ? body : "");

        string serverSign;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
        {
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signText));
            serverSign = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        // Log request details
        Console.WriteLine("Request Body: " + body);
        Console.WriteLine("Sign Text: " + signText);
        Console.WriteLine("Generated Signature: " + serverSign);

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Appid", appId);
            client.DefaultRequestHeaders.Add("Sign", serverSign);
            client.DefaultRequestHeaders.Add("Timestamp", timestamp.ToString());

            var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, httpContent);

            string responseString = await response.Content.ReadAsStringAsync();
            try
            {
                // Deserialize the JSON response into the ApiResponse object
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);

                // Check if the response code indicates success
                if (apiResponse != null && apiResponse.Code == 10000 && apiResponse.Data != null)
                {
                    // Fetch the address
                    string address = apiResponse.Data.Address;
                    SpnAddress.InnerText = address;
                    hdnPaymentId.Text = address;
                    // Generate the QR code
                    QRCodeGeneratorHelper1 qrHelper = new QRCodeGeneratorHelper1();
                    // string address = "http://example.com";
                    qrCodeImage.Src = qrHelper.GenerateQRCodeDataURL(SpnAddress.InnerText);
                    // Use the address variable as needed
                    Response.Write("Fetched Address: " + address);
                }
                else
                {
                    Response.Write("Error: Unexpected response format or code.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to parse the response. " + ex.Message);
            }
            Response.Write("API Response: " + responseString);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode}, Response: {responseString}");
            }
        }
    }
    public void GenerateQRCode(string address)
    {
        try
        {
            // Generate QR code
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    // Create bitmap for the QR code
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        // Save QR code as PNG in memory stream
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            qrCodeImage.Save(memoryStream, ImageFormat.Png);
                            //qrCodeImage.src = ImageFormat.Png;
                            // Return QR code as an image response
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.ContentType = "image/png";
                            HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
                            HttpContext.Current.Response.End();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors
            HttpContext.Current.Response.Write($"Error generating QR code: {ex.Message}");
        }
    }
    private static string GenerateHMACSHA256Signature(string text, string secret)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
public class ApiResponse
{
    public int Code { get; set; }
    public string Msg { get; set; }
    public Data Data { get; set; }
}

public class Data
{
    public string Address { get; set; }
    public string Memo { get; set; }
}
public class QRCodeGeneratorHelper1
{
    public string GenerateQRCodeDataURL(string address)
    {
        // Validate the Solana address
        if (string.IsNullOrEmpty(address) || !IsValidSolanaAddress(address))
        {
            throw new ArgumentException("Invalid Solana address", nameof(address));
        }

        try
        {
            // Build the Solana-specific URI
            var uriBuilder = new StringBuilder($"solana:{address}");
            var parameters = new List<string>();

            //if (amount.HasValue)
            //    parameters.Add($"amount={amount.Value}");

            //if (!string.IsNullOrEmpty(label))
            //    parameters.Add($"label={Uri.EscapeDataString(label)}");

            //if (!string.IsNullOrEmpty(message))
            //    parameters.Add($"message={Uri.EscapeDataString(message)}");

            if (parameters.Count > 0)
                uriBuilder.Append($"?{string.Join("&", parameters)}");

            string qrCodeContent = uriBuilder.ToString();

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrCodeData))
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, ImageFormat.Png);
                string base64Image = Convert.ToBase64String(memoryStream.ToArray());
                return $"data:image/png;base64,{base64Image}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating QR code: {ex.Message}");
            return string.Empty;
        }
    }

    // Helper method to validate Solana addresses
    private bool IsValidSolanaAddress(string address)
    {
        var solanaAddressPattern = "^[A-HJ-NP-Za-km-z1-9]{32,44}$";
        return Regex.IsMatch(address, solanaAddressPattern);
    }

    //public string GenerateQRCodeDataURL(string address)
    //{
    //    string qrCodeDataURL = string.Empty;

    //    try
    //    {
    //        // Construct the URI for the QR code
    //        string qrCodeContent = $"solana:{address}";
    //        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
    //        {
    //            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.Q);

    //            using (QRCode qrCode = new QRCode(qrCodeData))
    //            {
    //                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
    //                {
    //                    using (MemoryStream memoryStream = new MemoryStream())
    //                    {
    //                        qrCodeImage.Save(memoryStream, ImageFormat.Png);
    //                        byte[] qrCodeBytes = memoryStream.ToArray();
    //                        string base64Image = Convert.ToBase64String(qrCodeBytes);
    //                        qrCodeDataURL = "data:image/png;base64," + base64Image;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error generating QR code: {ex.Message}");
    //    }

    //    return qrCodeDataURL;
    //}
}