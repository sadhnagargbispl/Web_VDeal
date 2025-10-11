using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class ApiTest : System.Web.UI.Page
{
    DataSet dsLogin = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        string jsonResponse = @"{
            ""code"": 10000,
            ""msg"": ""success"",
            ""data"": {
                ""address"": ""6MYHqNrdarQNPKaToMjpW8rPwyeoS9EhAtmY7NiGesGY"",
                ""memo"": """"
            }
        }";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        var Data = jss.Deserialize<Object>(jsonResponse);
        dsLogin = ConvertJsonStringToDataSet(jsonResponse);
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        DataSet ds = new DataSet();
        XmlDocument xd = new XmlDocument();
        // Add root node to the JSON string for proper XML structure
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        // Deserialize JSON string to XML
        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
        // Read the XML into the DataSet
        ds.ReadXml(new XmlNodeReader(xd));

        return ds;
    }
    //private async Task GenerateAddressAsync1()
    //{
    //    string sResult = "";
    //    string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //    int randomNumber = new Random().Next(0, 999);
    //    string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
    //    sResult = formattedDatetime;
    //    string appId = "a4tcpLeAUW9Zaogy";
    //    string appSecret = "165d35b8912071d978c8c14911207131";
    //    string url = "https://ccpayment.com/ccpayment/v2/getAppOrderInfo";
    //    var content = new
    //    {
    //        orderId = Session["idno"]
    //    };

    //    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    //    string body = JsonConvert.SerializeObject(content);

    //    string signText = appId + timestamp;
    //    if (!string.IsNullOrEmpty(body) && body != "{}")
    //    {
    //        signText += body;
    //    }
    //    else
    //    {
    //        body = "";
    //    }

    //    string serverSign;
    //    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
    //    {
    //        serverSign = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(signText)))
    //                                  .Replace("-", "")
    //                                  .ToLower();
    //    }

    //    using (var httpClient = new HttpClient())
    //    {
    //        // httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=utf-8");
    //        httpClient.DefaultRequestHeaders.Add("Appid", appId);
    //        httpClient.DefaultRequestHeaders.Add("Sign", serverSign);
    //        httpClient.DefaultRequestHeaders.Add("Timestamp", timestamp.ToString());

    //        var response = await httpClient.PostAsync(
    //            url,
    //            new StringContent(body, Encoding.UTF8, "application/json")
    //        );

    //        string responseString = await response.Content.ReadAsStringAsync();
    //        var result = JsonConvert.DeserializeObject(responseString);
    //        Response.Write("API Response: " + responseString);

    //        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    //    }
    //}
    //private async Task GenerateAddressAsync()
    //{
    //    string sResult = "";
    //    string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //    int randomNumber = new Random().Next(0, 999);
    //    string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
    //    sResult = formattedDatetime;
    //    string appId = "a4tcpLeAUW9Zaogy";
    //    string appSecret = "165d35b8912071d978c8c14911207131";
    //    string url = "https://ccpayment.com/ccpayment/v2/getOrCreateUserDepositAddress";

    //    // Prepare content with chain set to SOLANA
    //    var content = new
    //    {
    //        userId = Session["idno"],
    //        chain = "SOL"
    //    };
    //    string body = JsonConvert.SerializeObject(content);
    //    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    //    string signText = appId + timestamp + (body.Length != 2 ? body : "");

    //    string serverSign;
    //    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
    //    {
    //        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signText));
    //        serverSign = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    //    }

    //    // Log request details
    //    Console.WriteLine("Request Body: " + body);
    //    Console.WriteLine("Sign Text: " + signText);
    //    Console.WriteLine("Generated Signature: " + serverSign);

    //    using (var client = new HttpClient())
    //    {
    //        client.DefaultRequestHeaders.Add("Appid", appId);
    //        client.DefaultRequestHeaders.Add("Sign", serverSign);
    //        client.DefaultRequestHeaders.Add("Timestamp", timestamp.ToString());

    //        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
    //        HttpResponseMessage response = await client.PostAsync(url, httpContent);

    //        string responseString = await response.Content.ReadAsStringAsync();
    //        try
    //        {
    //            // Deserialize the JSON response into the ApiResponse object
    //            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);

    //            // Check if the response code indicates success
    //            if (apiResponse != null && apiResponse.Code == 10000 && apiResponse.Data != null)
    //            {
    //                // Fetch the address
    //                string address = apiResponse.Data.Address;
    //                SpnAddress.InnerText = address;
    //                hdnPaymentId.Text = address;
    //                // Generate the QR code
    //                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
    //                {
    //                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
    //                    using (QRCode qrCode = new QRCode(qrCodeData))
    //                    {
    //                        using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
    //                        {
    //                            using (MemoryStream memoryStream = new MemoryStream())
    //                            {
    //                                qrCodeImage.Save(memoryStream, ImageFormat.Png);
    //                                byte[] qrCodeBytes = memoryStream.ToArray();
    //                                QRCodeBase64 = "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes);
    //                                //qrCodeImage.Src = QRCodeBase64;
    //                            }
    //                        }
    //                    }
    //                }
    //                // Use the address variable as needed
    //                Response.Write("Fetched Address: " + address);
    //            }
    //            else
    //            {
    //                Response.Write("Error: Unexpected response format or code.");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Error: Unable to parse the response. " + ex.Message);
    //        }
    //        Response.Write("API Response: " + responseString);

    //        if (!response.IsSuccessStatusCode)
    //        {
    //            throw new Exception($"API Error: {response.StatusCode}, Response: {responseString}");
    //        }
    //    }
    //}
    //public void GenerateQRCode(string address)
    //{
    //    try
    //    {
    //        // Generate QR code
    //        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
    //        {
    //            QRCodeData qrCodeData = qrGenerator.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
    //            using (QRCode qrCode = new QRCode(qrCodeData))
    //            {
    //                // Create bitmap for the QR code
    //                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
    //                {
    //                    // Save QR code as PNG in memory stream
    //                    using (MemoryStream memoryStream = new MemoryStream())
    //                    {
    //                        qrCodeImage.Save(memoryStream, ImageFormat.Png);
    //                        //qrCodeImage.src = ImageFormat.Png;
    //                        // Return QR code as an image response
    //                        HttpContext.Current.Response.Clear();
    //                        HttpContext.Current.Response.ContentType = "image/png";
    //                        HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
    //                        HttpContext.Current.Response.End();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle errors
    //        HttpContext.Current.Response.Write($"Error generating QR code: {ex.Message}");
    //    }
    //}
    //private static string GenerateHMACSHA256Signature(string text, string secret)
    //{
    //    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
    //    {
    //        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
    //        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    //    }
    //}
}