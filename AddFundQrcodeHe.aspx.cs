using AjaxControlToolkit;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using QRCoder;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;
public partial class AddFundQrcodeHe : System.Web.UI.Page
{
    private cls_DataAccess dbConnect;
    private int TransferId;
    private DataTable tmpTable = new DataTable();
    private DataTable dt1;
    private DataTable dt2;
    private string strQuery = "";
    private string scrname;
    private clsGeneral objGen = new clsGeneral();
    private string coinRate = "";
    private DAL Obj = new DAL();
    private string IsoStart;
    private string IsoEnd;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    public string QRCodeBase64 { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Obj = new DAL();
            this.BtnDepositClickBEP.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnDepositClickBEP));
            this.btnRetry.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnRetry));
            this.BtnconfirmBtotton.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnconfirmBtotton));
            this.BtnSaveDB.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSaveDB));
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringActive(6);
                    Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel();
                    FillPaymode();
                    CheckVisible();
                }
                if (RbtStatus.SelectedValue == "B")
                {
                    divByCrpto.Visible = false;
                    divByBank.Visible = true;
                }
                else
                {
                    divByCrpto.Visible = true;
                    divByBank.Visible = false;
                    GenerateQrCodeSummitSOl(Session["Formno"].ToString());
                }
            }

            else
            {
                Response.Redirect("Logout.aspx");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

    }
    public string GenerateRandomStringActive(int iLength)
    {
        try
        {
            Random rdm = new Random();
            char[] allowChrs = "123456789".ToCharArray();
            string sResult = "";

            for (int i = 0; i < iLength; i++)
            {
                sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
            }
            return sResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillPaymode()
    {
        try
        {
            strQuery = "SELECT * FROM M_PayModeMaster WHERE ActiveStatus='Y'   order by Pid";

            tmpTable = new DataTable();
            tmpTable = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            {
                var withBlock = DdlPaymode;
                withBlock.DataSource = tmpTable;
                withBlock.DataValueField = "PID";
                withBlock.DataTextField = "Paymode";
                withBlock.DataBind();
            }
            Session["PaymodeDetail"] = tmpTable;
        }
        catch (Exception ex)
        {

        }
    }
    protected void DdlPaymode_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        try
        {
            CheckVisible();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
        }
    }
    public string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj;
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = System.Convert.ToInt32((image.Width * scaleFactor));
            var newHeight = System.Convert.ToInt32((image.Height * scaleFactor));
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
    protected void CheckVisible()
    {
        DataTable dt;
        string condition = "";
        dt = (DataTable)Session["PaymodeDetail"];
        DataRow[] Dr = dt.Select("PID='" + DdlPaymode.SelectedValue + "'");
        if (Dr.Length > 0)
        {
            if (Dr[0]["IsTransNo"].ToString() == "Y")
                divDDno.Visible = true;
            else
            {
                divDDno.Visible = false;
                TxtDDNo.Text = "";
            }
            if (Dr[0]["AllBank"].ToString() == " ")
                condition = "";
            else if (Dr[0]["AllBank"].ToString() == "N")
                condition = "and MacAdrs='C' and BranChName<>'N'";
            else
                condition = "and MacAdrs='C'";
            if (Dr[0]["Isimage"].ToString() == "N")
                divImage.Visible = false;
            else
                divImage.Visible = true;

            LblDDNo.Text = Dr[0]["TransNoLbl"].ToString(); LblDDDate.Text = Dr[0]["TransDateLbl"].ToString();
        }
    }
    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    private void Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel()
    {
        try
        {
            string sql = "";
            DataTable dt_API_MAster = new DataTable();
            sql = Obj.IsoStart + " Exec Sp_GetCryptoAPIFor_FundWithdraw_Cpanel " + Obj.IsoEnd;
            dt_API_MAster = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["CreateQrCode"] = dt_API_MAster.Rows[0]["APIURL"];
            Session["BalanceCheckQrCode"] = dt_API_MAster.Rows[1]["APIURL"];
            Session["TokenCheckQrCode"] = dt_API_MAster.Rows[2]["APIURL"];
            Session["MMRATE"] = dt_API_MAster.Rows[3]["APIURL"];
            Session["CHECKTOKENADMIN"] = dt_API_MAster.Rows[4]["APIURL"];
            Session["BEPCREATE"] = dt_API_MAster.Rows[5]["APIURL"];
            Session["BEPBALANCE"] = dt_API_MAster.Rows[6]["APIURL"];
            Session["BEPTOKEN"] = dt_API_MAster.Rows[7]["APIURL"];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string GenerateQrCode(string Formno, double TokenAMount)
    {
        string completeUrl = "";
        try
        {
            int updateeffect = 0;
            string sql = "select orderid from ApiQrCodeReqResponse where orderid='" + Session["Orderid"] + "' ";
            sql += "AND ApiStatus='success' AND ApiType='Token Payout'";
            DataTable dt = new DataTable();
            dt = Obj.GetData(sql);

            if (dt.Rows.Count == 0)
            {
                DataSet dsLogin = new DataSet();
                DataTable dt_QrCode = new DataTable();
                string Str_QrcodeGet = "Exec Sp_GetFormnoqrCode '" + Formno + "'";
                dt_QrCode = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str_QrcodeGet).Tables[0];
                string responseString = string.Empty;

                if (dt_QrCode.Rows.Count == 0)
                {
                    completeUrl = Session["CreateQrCode"].ToString();
                    completeUrl += "?amount=" + Convert.ToDecimal(TokenAMount) + "";
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
                    request1.ContentType = "application/json";
                    request1.Method = "GET";
                    HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse();
                    StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();

                    string Str_RR = "Insert Into M_FormnoqrCode(Formno, QrCode, ActiveStatus,ForType) Values('" + Formno + "','" + responseString + "','Y','C')";
                    int RR = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_RR));
                }
                else
                {
                    responseString = dt_QrCode.Rows[0]["QrCode"].ToString();
                }

                string json = responseString;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var Data = jss.Deserialize<Object>(responseString);
                dsLogin = ConvertJsonStringToDataSet(responseString);
                SpnAddress.InnerText = dsLogin.Tables[0].Rows[0]["address"].ToString();
                privatekey.Value = dsLogin.Tables[0].Rows[0]["key"].ToString();
                //ImgQrCode.Src = dsLogin.Tables[0].Rows[0]["qrCodeDataURL"].ToString();
                fromidno.Value = Formno.ToString();
                HdnFormno.Value = Formno.ToString();
                OrderId.Value = dsLogin.Tables[0].Rows[0]["key"].ToString();
                Session["Orderid"] = OrderId.Value;

                string strs = "";
                strs += "insert into ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, ";
                strs += "Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData) ";
                strs += "Values('" + fromidno.Value + "','" + OrderId.Value + "','" + SpnAddress.InnerText + "',";
                strs += "'" + privatekey.Value + "','" + completeUrl + "','" + responseString + "','Pending', getdate(), 'QrCode Generate', '', '0', '');";

                string Query = "Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                DAL objdal = new DAL();
                int i = objdal.SaveData(Query);

                if (i > 0)
                {
                    DivQr.Visible = true;
                    DivQrCodeAmount.Visible = false;
                    hdnPaymentId.Value = SpnAddress.InnerText;
                }
                else
                {
                    string message = "Try Again.!";
                    string url = "AddFundPayment.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += message;
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                }
            }
            else
            {
                string message = "Try Again.!";
                string url = "AddFundPayment.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
            }
        }
        catch (Exception ex)
        {
            string errorQry = "";
            errorQry = "insert Into TrnLogData(ErrorText, LogDate, Url, WalletAddress)";
            errorQry += "values('" + ex.Message + "', getdate(), '" + completeUrl + "', '" + SpnAddress.InnerText + "')";
            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
        }
        return completeUrl;
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
    protected void btnRetry_Click(object sender, EventArgs e)
    {
        try
        {
            double tokenAmountS = Convert.ToDouble(TxtTokenAmount.Text);
            Fund_Balance_Check(hdnPaymentId.Value, HdnFormno.Value, OrderId.Value, privatekey.Value, tokenAmountS);
        }
        catch (Exception ex)
        {

        }
    }
    public string Fund_Balance_Check(string walletAddress, string formNoV, string orderIV, string privateKeyV, double TokenAMount)
    {
        string URL = "";
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        string postData = "";
        string balance = "";
        string str = "";
        string statusApi = "";
        int resultRR = 0;
        DataSet dsLogin = new DataSet();
        DataTable dsLoginToAddress = new DataTable();
        string responseString = string.Empty;
        string toWalletAddress = "";

        try
        {
            decimal value = 0;
            string code = "";
            DataSet ds = new DataSet();
            DataSet data = new DataSet();

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                URL = Session["BalanceCheckQrCode"].ToString();
                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;
                postData = "{\"walletAddress\": \"" + walletAddress.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType)";
                sqlReq += " VALUES('" + sResult.Trim() + "','" + formNoV.Trim() + "','" + URL.Trim() + "','" + postData.Trim() + "','BALANCECHECK')";
                int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                str = tReader.ReadToEnd();
                string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + str.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND ApiType = 'BALANCECHECK' ";
                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));
                string json = str;
                string strs = "";
                string strCheck = "";
                string tokenName = "";
                string tokenSymbol = "";
                decimal FinalAmount;
                DataTable dtCheck = new DataTable();
                try
                {
                    data = ConvertJsonStringToDataSet(str);
                    DataView dv = new DataView(data.Tables[1]);
                    dv.RowFilter = "To = '" + walletAddress.Trim() + "'";
                    dsLoginToAddress = dv.ToTable();
                    if (Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) > 0)
                    {
                        //decimal allowedDifference = 5m;
                        //bool result = IsAmountLessThanOrEqualToBalance(Convert.ToDecimal(TokenAMount), allowedDifference);
                        ////bool result = IsAmountLessThanOrEqualToBalance(Convert.ToDecimal(TokenAMount), Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]), allowedDifference);
                        //if (result)
                        //{
                        if (dsLoginToAddress.Rows.Count > 0)
                        {
                            foreach (DataRow row in dsLoginToAddress.Rows)
                            {
                                toWalletAddress = row["to"].ToString();
                                string fromWalletAddress = row["from"].ToString();
                                double originalValue = Convert.ToDouble(row["value"]);
                                FinalAmount = (decimal)(originalValue / Math.Pow(10, 18));
                                decimal accurateTotalAmount = decimal.Parse(FinalAmount.ToString(), NumberStyles.Float);
                                string hash = row["hash"].ToString();
                                string contractAddress = row["contractAddress"].ToString().Trim();
                                tokenName = row["tokenName"].ToString();
                                tokenSymbol = row["tokenSymbol"].ToString();
                                if (tokenName.ToUpper() == "MANGO MAN INTELLIGENT")
                                {
                                    if (tokenSymbol.ToUpper() == "MMIT")
                                    {
                                        strCheck = " EXEC Sp_CheckTxnHAsh__ '" + hash.ToString() + "'";
                                        dtCheck = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strCheck).Tables[0];
                                        if (dtCheck.Rows.Count == 0)
                                        {
                                            decimal allowedDifference = 5m;
                                            bool result = IsAmountLessThanOrEqualToBalance(Convert.ToDecimal(TokenAMount), decimal.Parse(FinalAmount.ToString(), NumberStyles.Float), allowedDifference);
                                            if (result)
                                            {
                                                string txnInsert = "INSERT INTO TrnvoucherTxnHash(Formno, From_walletAddress, To_walletAddress, Amount, Txnhash, To_PrivateKey,ReqFrom)";
                                                txnInsert += " VALUES('" + formNoV + "','" + fromWalletAddress + "','" + toWalletAddress + "',";
                                                txnInsert += "'" + decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) + "','" + hash + "','" + privateKeyV + "','MMIT')";
                                                int xI = 0;
                                                try
                                                {
                                                    xI = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, txnInsert);
                                                }
                                                catch (SqlException Ex)
                                                {
                                                    if (Ex.Number == 2627) // Unique constraint violation error number
                                                    {
                                                        xI = 0;
                                                    }
                                                }

                                                if (xI > 0)
                                                {
                                                    strCheck = " EXEC Sp_CheckTxnHAsh '" + hash.ToString() + "'";
                                                    dtCheck = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strCheck).Tables[0];
                                                    if (dtCheck.Rows.Count != 0)
                                                    {
                                                        if (contractAddress == "0x9767c8e438aa18f550208e6d1fdf5f43541cc2c8")
                                                        {
                                                            strs = string.Empty;
                                                            strs = "INSERT INTO ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData, TypeB, FromID, ToID) ";
                                                            strs += "VALUES('" + formNoV + "','" + orderIV + "','" + walletAddress + "','" + privateKeyV + "',";
                                                            strs += "'" + URL + "','" + str + "','" + statusApi + "',GETDATE(),'Re-Transcation','" + hash + "',";
                                                            strs += "'" + decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) + "','','QrCode','" + toWalletAddress + "','" + fromWalletAddress + "');";
                                                            strs += " EXEC sp_FundAddUpdate_FundUpdatejoshmart '" + formNoV + "','" + decimal.Parse(TxtReqAmount.Text.ToString(), NumberStyles.Float) + "',";
                                                            strs += "'" + orderIV + "','" + walletAddress + "','" + hash + "','" + fromWalletAddress + "','" + toWalletAddress + "';";
                                                            string queryStr = "";
                                                            queryStr = " BEGIN TRY BEGIN TRANSACTION " + strs + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                                                            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, queryStr);
                                                            if (x > 0)
                                                            {
                                                                resultRR = 1;
                                                                string sqlResToken = "";
                                                                string tokenResponse = Fund_Token_Send(toWalletAddress, privateKeyV, decimal.Parse(FinalAmount.ToString(), NumberStyles.Float), HdnFormno.Value);
                                                                if (tokenResponse.ToUpper().Trim() == "SUCCESSFUL")
                                                                {
                                                                    sqlResToken = "UPDATE TrnvoucherTxnHash SET Is_Pay = 'A', Update_DATe = GETDATE() WHERE To_walletAddress = '" + toWalletAddress.Trim() + "' AND Is_Pay = 'P'";
                                                                    int xResToken = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sqlResToken);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strs = string.Empty;
                                                            strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                                            strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                                            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strs = string.Empty;
                                                        strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                                        strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                                        int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                decimal remaining = Convert.ToDecimal(TokenAMount) - Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]);
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Have Deposited " + Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) + " Please Deposit Remaining Token " + remaining + "');", true);
                                            }
                                        }
                                        else
                                        {
                                            strs = string.Empty;
                                            strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                            strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                        }
                                    }
                                }
                            }
                        }
                        //}
                        //else
                        //{
                        //    decimal remaining = Convert.ToDecimal(TokenAMount) - Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]);
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Have Deposited " + Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) + " Please Deposit Remaining Token " + remaining + "');", true);
                        //}

                    }
                    if (resultRR > 0)
                    {
                        string message = "Payment Successfully Added in Wallet.!";
                        string urlStr = "pointWallet.aspx";
                        string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later');", true);
                    }
                }
                catch (Exception ex)
                {
                    string errorQry = "";
                    string errorMsg = "";

                    try
                    {
                        errorMsg = ex.Message;
                    }
                    catch (Exception eXX) { }
                    finally
                    {
                        if (errorMsg != "") { } else { errorMsg = ""; }
                    }

                    errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) ";
                    errorQry += "VALUES('" + errorMsg + "', GETDATE(),'" + URL + "','" + toWalletAddress.Trim() + "','" + str + "','" + formNoV + "')";
                    int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later.');", true);
                }
            }
            catch (Exception ex) { }
        }
        catch (Exception ex) { }

        return balance;
    }
    public string Fund_Token_Send(string senderAddress, string senderPrivateKey, decimal Balance, string Formno_V)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string URL = "";
        string postData = "";
        string str = string.Empty;
        decimal value = 0;
        string Code = "";
        DataSet ds = new DataSet();
        DataSet data = new DataSet();
        string StatusApi = "";
        string RetunnStatus = "";

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            URL = Session["CHECKTOKENADMIN"].ToString();
            WebRequest tRequest = WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.ContentLength = 0;
            postData = "{\"senderAddress\":\"" + senderAddress.Trim() + "\",\"senderPrivateKey\":\"" + senderPrivateKey.Trim() + "\",\"amount\":\"" + decimal.Parse(Balance.ToString(), NumberStyles.Float) + "\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata, ApiType)";
            sql_req += " Values('" + sResult.Trim() + "','" + Formno_V.Trim() + "','" + URL.Trim() + "','" + postData.Trim() + "','TOKENCHECK')";
            int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            str = tReader.ReadToEnd();
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + str.Trim() + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'TOKENCHECK' ";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
            string SqlUpdate = "";
            string json = str;
            data = ConvertJsonStringToDataSet(str);
            StatusApi = data.Tables[0].Rows[0]["transactionstatus"].ToString();
            string Query = "";
            string hash_ = "";
            if (StatusApi.ToUpper().Trim() == "SUCCESSFUL")
            {
                try
                {
                    hash_ = data.Tables[0].Rows[0]["transactionHash"].ToString();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (hash_ == "") { hash_ = ""; }
                }
            }
            string strs = "";
            strs += "insert into ApiQrCodeReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB) ";
            strs += "Values('" + Formno_V + "','" + senderPrivateKey + "','" + senderAddress + "','" + senderPrivateKey + "','" + URL + "','" + str + "','" + StatusApi + "',";
            strs += "getdate(),'Token Payout','" + hash_ + "','" + decimal.Parse(Balance.ToString(), NumberStyles.Float) + "','" + postData + "','QrCode');";

            Query = " Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, Query);

            if (x > 0)
            {
                RetunnStatus = StatusApi;
            }
        }
        catch (Exception ex)
        {
            string errorQry = "";
            string ErrorMsg = "";
            try
            {
                ErrorMsg = ex.Message;
            }
            catch (Exception eXX) { }
            finally
            {
                if (ErrorMsg == "") { ErrorMsg = ""; }
            }
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + ErrorMsg + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'TOKENCHECK' ";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
            errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno)values('" + ErrorMsg + "',getdate(),'" + URL + "','" + senderAddress.Trim() + "','" + postData + "','" + Formno_V + "')";
            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);

            if (x1 > 0)
            {
                RetunnStatus = "failed";
            }
        }

        return RetunnStatus;
    }
    public string MMIDUSDT()
    {
        string _Output = "";
        string completeUrl = "https://pro-api.coinmarketcap.com/v1/tools/price-conversion?symbol=SUMMIT&amount=1&convert=USD";
        string responseString = string.Empty;
        string amount = string.Empty;
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
            request1.ContentType = "application/json";
            request1.Method = "GET";
            request1.Headers.Add("X-CMC_PRO_API_KEY", "67215889-9886-4900-8177-38b276e107b6");
            HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse();
            using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var data = jss.Deserialize<dynamic>(responseString);
            amount = Convert.ToString(data["data"]["quote"]["USD"]["price"]);
            HdnAmountApi.Value = amount;
            string Str_RR = "insert into GasFeesCheckLatest(FormNo,StatusApi,ErrorMsg,Response,Url,RectimeStamp,Ratebalance) ";
            Str_RR += "Values('" + Session["formno"] + "','Y','" + responseString + "',";
            Str_RR += "'" + responseString + "','" + completeUrl + "',getdate(),'" + HdnAmountApi.Value + "')";
            int RR = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_RR));
            if (RR > 0)
            {
                amount = HdnAmountApi.Value;
            }
        }
        catch (Exception ex)
        {
            string errorQry = "";
            errorQry = "insert Into TrnLogData(ErrorText,LogDate,Txnhash,Url,WalletAddress,PostData,formno,SelectType,ResposeString) ";
            errorQry += "values('" + ex.Message + "',getdate(),'','" + completeUrl + "','" + SpnAddress.InnerText + "','" + responseString + "',";
            errorQry += "'" + responseString + "','" + HdnFormno.Value + "','C','" + responseString + "')";

            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);

            if (x1 > 0)
            {
                amount = "0";
            }
        }
            return amount;
        //string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJub25jZSI6Ijg5MjY0MDMzLWRmODQtNDkzNS1iMmQxLThmMDA3OWFhNjlkOSIsIm9yZ0lkIjoiNDAxMzY1IiwidXNlcklkIjoiNDEyNDI4IiwidHlwZUlkIjoiMGRiZjcyZjMtNWI5Ny00YWFkLWFlZTctMjg3NDIxMzdiZmI0IiwidHlwZSI6IlBST0pFQ1QiLCJpYXQiOjE3MjE5MDQyMzIsImV4cCI6NDg3NzY2NDIzMn0.VSd6PnHb1-azZwF-0ZrNcYcuaUK9RYRuMeXblWVCrv4";
        //DataSet dsLogin = new DataSet();
        //string completeUrl = Session["MMRATE"].ToString();
        //string responseString = string.Empty;
        //string amount = "";

        //try
        //{
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
        //    request1.ContentType = "application/json";
        //    request1.Method = "GET";
        //    request1.Headers.Add("X-API-Key", apiKey);

        //    HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse();
        //    StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
        //    responseString = reader.ReadToEnd();
        //    string json = responseString;

        //    JavaScriptSerializer jss = new JavaScriptSerializer();
        //    var data = jss.Deserialize<Object>(responseString);
        //    dsLogin = ConvertJsonStringToDataSet(responseString);

        //    HdnAmountApi.Value = dsLogin.Tables[0].Rows[0]["usdPriceFormatted"].ToString();

        //    string Str_RR = "insert into GasFeesCheckLatest(FormNo,StatusApi,ErrorMsg,Response,Url,RectimeStamp,Ratebalance) ";
        //    Str_RR += "Values('" + Session["formno"] + "','Y','" + responseString + "',";
        //    Str_RR += "'" + responseString + "','" + completeUrl + "',getdate(),'" + HdnAmountApi.Value + "')";

        //    int RR = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_RR));

        //    if (RR > 0)
        //    {
        //        amount = HdnAmountApi.Value;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    string errorQry = "";
        //    errorQry = "insert Into TrnLogData(ErrorText,LogDate,Txnhash,Url,WalletAddress,PostData,formno,SelectType,ResposeString) ";
        //    errorQry += "values('" + ex.Message + "',getdate(),'','" + completeUrl + "','" + SpnAddress.InnerText + "','" + responseString + "',";
        //    errorQry += "'" + responseString + "','" + HdnFormno.Value + "','C','" + responseString + "')";

        //    int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);

        //    if (x1 > 0)
        //    {
        //        amount = "0";
        //    }
        //}
    }
    protected void TxtReqAmount_TextChanged(object sender, EventArgs e)
    {
        HdnRate.Value = MMIDUSDT();
        double TotalRate = Convert.ToDouble(TxtReqAmount.Text);
        double Bonus = 0;
        double ResultAmount = 0;
        if (TotalRate <= 10000)
        {
            Bonus = TotalRate * 6 / 100;
            ResultAmount = TotalRate + Bonus;
        }
        else if (TotalRate >= 10001 && TotalRate <= 100000)
        {
            Bonus = TotalRate * 10 / 100;
            ResultAmount = TotalRate + Bonus;
        }
        else if (TotalRate >= 100001)
        {
            Bonus = TotalRate * 12 / 100;
            ResultAmount = TotalRate + Bonus;
        }
        TxtINRValue.Text = (Convert.ToDouble(ResultAmount) / 88).ToString();
        decimal inrValue;
        decimal rate;
        decimal tokenAmount = 0;
        //double inrValue = Convert.ToDouble(TxtINRValue.Text);
        //double rate = double.Parse(HdnRate.Value) / Math.Pow(10, 18);
        //double tokenAmount = inrValue / HdnRate.Value;
        //TxtTokenAmount.Text = "" + (tokenAmount / Math.Pow(10, 18));
        if (decimal.TryParse(TxtINRValue.Text, out inrValue) && decimal.TryParse(HdnRate.Value, out rate))
        {
            // Check for division by zero
            if (rate != 0)
            {
                tokenAmount = inrValue / rate;
                TxtTokenAmount.Text = tokenAmount.ToString();
            }
            else
            {
                // Handle division by zero if needed
                TxtTokenAmount.Text = "0";
            }
        }
        else
        {
            // Handle invalid input if needed
            TxtTokenAmount.Text = "0";
        }
        string StrSql = "INSERT INTO TrnTokenAmount(FormNo,Amount,TokenAmount,LiveRate)VALUES(" + Session["formno"] + ",'" + Convert.ToDouble(TxtReqAmount.Text) + "',";
        StrSql += "'" + Convert.ToDouble(TxtTokenAmount.Text) + "','" + HdnRate.Value + "')";
        int updateeffect = SqlHelper.ExecuteNonQuery(constr1, CommandType.Text, StrSql);
        if (updateeffect == 0)
        {
            scrname = "<SCRIPT language='javascript'>alert('Try Again Later.!');" + "</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }
    }
    protected void BtnconfirmBtotton_Click(object sender, EventArgs e)
    {
        //double tokenAmount;
        //if (double.TryParse(TxtTokenAmount.Text, out tokenAmount) && tokenAmount > 0)
        //{
        //    string formNumber = Session["Formno"].ToString();
        //    double tokenAmountS = double.Parse(TxtTokenAmount.Text) / Math.Pow(10, 18);
        //    TokenNoOF.InnerText = TxtTokenAmount.Text;
        GenerateQrCodeSummitSOl(Session["Formno"].ToString());
        //GenerateQrCode(formNumber, tokenAmountS);
        // }
    }
    public bool IsAmountLessThanOrEqualToBalance(decimal amount, decimal apiBalance, decimal allowedDifference)
    {
        decimal tabAmount = 0;
        decimal difference = 0;
        string s = "";
        DataTable dt;
        dt = new DataTable();

        s = "select top 1 * from TrnTokenAmount where formno = '" + Session["formno"] + "' order by id desc";
        dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];
        if (dt.Rows.Count > 0)
        {
            tabAmount = Convert.ToDecimal(dt.Rows[0]["TokenAmount"]);
            difference = Math.Abs(apiBalance - amount);
        }
        return difference <= allowedDifference;
    }
    protected void RbtStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbtStatus.SelectedValue == "B")
        {
            divByCrpto.Visible = false;
            divByBank.Visible = true;
        }
        else
        {
            divByCrpto.Visible = true;
            divByBank.Visible = false;
        }
    }
    protected void BtnSaveDB_Click(object sender, EventArgs e)
    {
        string FlNm, flnm1;
        Session["CkyPinRequest"] = null;
        if (Convert.ToInt32(TxtAmount.Text) <= 0)
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Amount.');" + "</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }
        if (TxtDDDate.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Transaction Date can not be blank');" + "</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }
        if (FlDoc.HasFile)
        {

        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('Select Choose File');" + "</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }
        bool flag;
        if ((DdlPaymode.SelectedValue) == "1")
            flag = true;
        else if (divDDno.Visible == true & TxtDDNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('" + LblDDNo.Text + " can not be blank');" + "</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }

        else if (CheckDDno() == true)
            flag = true;
        else
            flag = false;

        string strextension = "";
        try
        {
            if (flag == true)
            {
                SaveRequest();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected bool CheckDDno()
    {
        string s = "";
        DataTable dt;
        dt = new DataTable();
        if (Convert.ToInt32(DdlPaymode.SelectedValue) != 8)
        {
            if (divDDno.Visible == true & TxtDDNo.Text != "")
            {
                s = "select * from WalletReq where ChqNo = '" + (TxtDDNo.Text) + "' and IsApprove <> 'R'";
                dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    scrname = "<SCRIPT language='javascript'>alert('" + LblDDNo.Text + " already exist');" + "</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }
        else
            return true;
    }
    protected void SaveRequest()
    {
        try
        {
            string StrSql = "Insert into Trnreqwallet (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            int updateeffect = SqlHelper.ExecuteNonQuery(constr1, CommandType.Text, StrSql);
            if (updateeffect > 0)
            {
                string flnm1 = "";
                TxtDDNo.Text = ClearInject(TxtDDNo.Text);
                TxtDDDate.Text = ClearInject(TxtDDDate.Text);
                var query = "";
                string FlNm = "";

                DateTime ChqDate;
                string chqdates = "";
                try
                {
                    ChqDate = Convert.ToDateTime(TxtDDDate.Text);
                }
                catch (Exception ex)
                {
                    ChqDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff"));
                }
                if (divDDDate.Visible == true)
                {
                    ChqDate = Convert.ToDateTime(TxtDDDate.Text);
                    chqdates = TxtDDDate.Text;
                }
                else
                {
                    ChqDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff"));
                    chqdates = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff");
                }

                bool flag;
                if ((DdlPaymode.SelectedValue) == "1")
                    flag = true;
                else if (divDDno.Visible == true & TxtDDNo.Text == "")
                {
                    scrname = "<SCRIPT language='javascript'>alert('" + LblDDNo.Text + " can not be blank');" + "</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                else if (CheckDDno() == true)
                    flag = true;
                else
                    flag = false;



                string strextension = "";
                try
                {
                    if (flag == true)
                    {

                        if (divImage.Visible)
                        {
                            if (FlDoc.HasFile)
                            {
                                string filename = Path.GetFileName(FlDoc.PostedFile.FileName);
                                //string strname = Format(Now, Format(Now, "yyMMddhhmmssfff")) + "_1" + System.IO.Path.GetExtension(FlDoc.FileName);
                                string strname = DateTime.Now.ToString("yyMMddhhmmssfff") + "_1" + System.IO.Path.GetExtension(FlDoc.FileName);
                                string targetPath = Server.MapPath("img/UploadImage/" + strname);
                                Stream strm = FlDoc.PostedFile.InputStream;
                                var targetFile = targetPath;
                                GenerateThumbnails(0.5, strm, targetFile);
                                FlNm = strname;
                            }
                            else if (DdlPaymode.SelectedValue == "1")
                            {
                            }
                            else
                            {
                                scrname = "<SCRIPT language='javascript'>alert('Select Choose File');" + "</SCRIPT>";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                                return;
                            }
                        }

                        string str = "INSERT INTO WalletReq(ReqNo,ReqDate,Formno,PID,Paymode,Amount,ChqNo,ChqDate,BankName,BranchName,ScannedFile,Remarks,BankId,Transno,WalletAddress) " +
         " " + "Select ISNULL(Max(ReqNo)+1,'1001'),'" + DateTime.Now.ToString("dd-MMM-yyyy").ToString() + "'," +
         "'" + Session["Formno"].ToString() + "','" + DdlPaymode.SelectedValue + "','" + DdlPaymode.SelectedItem.Text + "','" + TxtAmount.Text + "'," +
         "'" + TxtDDNo.Text + "','" + DateTime.Now.ToString("dd-MMM-yyyy").ToString() + "','','','" + FlNm + "','','0','0','' FROM WalletReq " + "; " +
     "Insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) " +
     " Values" + "('" + Session["FormNo"] + "','" + Session["MemName"] + "','Payment Request','Payment Request', " +
     "'Amount: " + TxtAmount.Text + "',Getdate()," + Session["FormNo"] + ")";


                        int i = SqlHelper.ExecuteNonQuery(constr1, CommandType.Text, str);
                        int ReqNo;
                        str = " Select Max(ReqNo) as ReqNo FROM WalletReq WHERE Formno='" + Session["Formno"] + "' AND Amount='" + TxtAmount.Text + "'";
                        dt1 = new DataTable();
                        dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                        if (dt1.Rows.Count > 0)
                        {
                            ReqNo = Convert.ToInt32(dt1.Rows[0]["ReqNo"].ToString());
                            string message = "Payment Request Sent Successfully.\\nYour Request no. is " + ReqNo + "";
                            string urlStr = "AddFundPayment.aspx";
                            string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                            TxtAmount.Text = ""; TxtDDDate.Text = ""; TxtDDNo.Text = "";
                            FillPaymode();
                            CheckVisible();
                        }
                        else
                        {
                            scrname = "<SCRIPT language='javascript'>alert('Payment Request Sent UnSuccessfully.');" + "</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                string message = "Something Want Wrong.Please Try Again.!";
                string urlStr = "AddFundPayment.aspx";
                string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void RbtCheckOptin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbtCheckOptin.SelectedValue == "B")
        {
            DivMMIT.Visible = false;
            DIVBEP.Visible = true;
            GenerateQrCodeBEP(Session["Formno"].ToString());
        }
        else
        {
            DivMMIT.Visible = true;
            DIVBEP.Visible = false;
            divclicko.Visible = false;
            DivQr.Visible = true;
            GenerateQrCodeSummitSOl(Session["Formno"].ToString());
        }
    }
    public string GenerateQrCodeBEP(string Formno)
    {
        string completeUrl = "";
        try
        {
            int updateeffect = 0;
            string sql = "select orderid from ApiQrCodeReqResponse where orderid='" + Session["Orderid"] + "' ";
            sql += "AND ApiStatus='success' AND ApiType='Token Payout'";
            DataTable dt = new DataTable();
            dt = Obj.GetData(sql);

            if (dt.Rows.Count == 0)
            {
                DataSet dsLogin = new DataSet();
                DataTable dt_QrCode = new DataTable();
                string Str_QrcodeGet = "Exec Sp_GetFormnoqrCodeBEP '" + Formno + "'";
                dt_QrCode = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str_QrcodeGet).Tables[0];
                string responseString = string.Empty;
                if (dt_QrCode.Rows.Count == 0)
                {
                    completeUrl = Session["BEPCREATE"].ToString();
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
                    request1.ContentType = "application/json";
                    request1.Method = "GET";
                    HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse();
                    StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();

                    string Str_RR = "Insert Into M_FormnoqrCodeBEP(Formno, QrCode, ActiveStatus) Values('" + Formno + "','" + responseString + "','Y')";
                    int RR = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_RR));
                }
                else
                {
                    responseString = dt_QrCode.Rows[0]["QrCode"].ToString();
                }

                string json = responseString;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var Data = jss.Deserialize<Object>(responseString);
                dsLogin = ConvertJsonStringToDataSet(responseString);
                BEPUSDTAddress.InnerText = dsLogin.Tables[0].Rows[0]["address"].ToString();
                bepprivatekey.Value = dsLogin.Tables[0].Rows[0]["key"].ToString();
                BepUsdtImg.Src = dsLogin.Tables[0].Rows[0]["qrCodeDataURL"].ToString();
                fromidno.Value = Formno.ToString();
                HdnFormno.Value = Formno.ToString();
                BEPOrderId.Value = dsLogin.Tables[0].Rows[0]["key"].ToString();
                Session["Orderid"] = OrderId.Value;
                string strs = "";
                strs += "insert into ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, ";
                strs += "Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData) ";
                strs += "Values('" + fromidno.Value + "','" + OrderId.Value + "','" + SpnAddress.InnerText + "',";
                strs += "'" + privatekey.Value + "','" + completeUrl + "','" + responseString + "','Pending', getdate(), 'QrCode Generate BEP20', '', '0', '');";

                string Query = "Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                DAL objdal = new DAL();
                int i = objdal.SaveData(Query);

                if (i > 0)
                {
                    DIVBEP.Visible = true;
                    DivQrCodeAmount.Visible = true;
                    DivMMIT.Visible = false;

                    BEPhdnPaymentId.Value = BEPUSDTAddress.InnerText;
                }
                else
                {
                    string message = "Try Again.!";
                    string url = "AddFundPayment.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += message;
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                }
            }
            else
            {
                string message = "Try Again.!";
                string url = "AddFundPayment.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
            }
        }
        catch (Exception ex)
        {
            string errorQry = "";
            errorQry = "insert Into TrnLogData(ErrorText, LogDate, Url, WalletAddress,ForType)";
            errorQry += "values('" + ex.Message + "', getdate(), '" + completeUrl + "', '" + BEPUSDTAddress.InnerText + "','A')";
            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
        }
        return completeUrl;
    }
    public string Fund_Balance_CheckBEP(string walletAddress, string formNoV, string orderIV, string privateKeyV)
    {
        string URL = "";
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        string postData = "";
        string balance = "";
        string str = "";
        string statusApi = "";
        int resultRR = 0;
        DataSet dsLogin = new DataSet();
        DataTable dsLoginToAddress = new DataTable();
        string responseString = string.Empty;
        string toWalletAddress = "";
        try
        {
            decimal value = 0;
            string code = "";
            DataSet ds = new DataSet();
            DataSet data = new DataSet();

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                URL = Session["BEPBALANCE"].ToString();
                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;
                postData = "{\"walletAddress\": \"" + walletAddress.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType)";
                sqlReq += " VALUES('" + sResult.Trim() + "','" + formNoV.Trim() + "','" + URL.Trim() + "','" + postData.Trim() + "','BEPBALANCECHECK')";
                int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                str = tReader.ReadToEnd();
                string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + str.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND ApiType = 'BEPBALANCECHECK' ";
                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));
                string json = str;
                string strs = "";
                string strCheck = "";
                string tokenName = "";
                string tokenSymbol = "";
                decimal FinalAmount;
                DataTable dtCheck = new DataTable();

                try
                {
                    data = ConvertJsonStringToDataSet(str);
                    DataView dv = new DataView(data.Tables[1]);
                    dv.RowFilter = "To = '" + walletAddress.Trim() + "'";
                    dsLoginToAddress = dv.ToTable();
                    if (Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) > 0)
                    {
                        if (dsLoginToAddress.Rows.Count > 0)
                        {
                            foreach (DataRow row in dsLoginToAddress.Rows)
                            {
                                toWalletAddress = row["to"].ToString();
                                string fromWalletAddress = row["from"].ToString();
                                double originalValue = Convert.ToDouble(row["value"]);
                                FinalAmount = (decimal)(originalValue / Math.Pow(10, 18));
                                decimal accurateTotalAmount = decimal.Parse(FinalAmount.ToString(), NumberStyles.Float);
                                string hash = row["hash"].ToString();
                                string contractAddress = row["contractAddress"].ToString().Trim();
                                tokenName = row["tokenName"].ToString();
                                if (tokenName.ToUpper() == "BINANCE-PEG BSC-USD")
                                {

                                    string txnInsert = "INSERT INTO TrnvoucherTxnHash(Formno, From_walletAddress, To_walletAddress, Amount, Txnhash, To_PrivateKey,ReqFrom)";
                                    txnInsert += " VALUES('" + formNoV + "','" + fromWalletAddress + "','" + toWalletAddress + "',";
                                    txnInsert += "'" + decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) + "','" + hash + "','" + privateKeyV + "','USDT')";
                                    int xI = 0;
                                    try
                                    {
                                        xI = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, txnInsert);
                                    }
                                    catch (SqlException Ex)
                                    {
                                        if (Ex.Number == 2627) // Unique constraint violation error number
                                        {
                                            xI = 0;
                                        }
                                    }

                                    if (xI > 0)
                                    {
                                        strCheck = " EXEC Sp_CheckTxnHAsh '" + hash.ToString() + "'";
                                        dtCheck = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strCheck).Tables[0];
                                        if (dtCheck.Rows.Count != 0)
                                        {
                                            if (contractAddress == "0x55d398326f99059ff775485246999027b3197955")
                                            {
                                                strs = string.Empty;
                                                strs = "INSERT INTO ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData, TypeB, FromID, ToID) ";
                                                strs += "VALUES('" + formNoV + "','" + orderIV + "','" + walletAddress + "','" + privateKeyV + "',";
                                                strs += "'" + URL + "','" + str + "','" + statusApi + "',GETDATE(),'Re-Transcation BEP','" + hash + "',";
                                                strs += "'" + decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) + "','','QrCodeBEP','" + toWalletAddress + "','" + fromWalletAddress + "');";
                                                strs += " EXEC sp_FundAddUpdate_Fund_ '" + formNoV + "','" + decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) + "',";
                                                strs += "'" + orderIV + "','" + walletAddress + "','" + hash + "','" + fromWalletAddress + "','" + toWalletAddress + "';";
                                                string queryStr = "";
                                                queryStr = " BEGIN TRY BEGIN TRANSACTION " + strs + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                                                int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, queryStr);
                                                if (x > 0)
                                                {
                                                    resultRR = 1;
                                                    string sqlResToken = "";
                                                    string tokenResponse = Fund_Token_SendBEP(toWalletAddress, privateKeyV, decimal.Parse(FinalAmount.ToString(), NumberStyles.Float), HdnFormno.Value);
                                                    if (tokenResponse.ToUpper().Trim() == "SUCCESSFUL")
                                                    {
                                                        sqlResToken = "UPDATE TrnvoucherTxnHash SET Is_Pay = 'A', Update_DATe = GETDATE() WHERE To_walletAddress = '" + toWalletAddress.Trim() + "' AND Is_Pay = 'P'";
                                                        int xResToken = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sqlResToken);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                strs = string.Empty;
                                                strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                                strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                                int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                            }
                                        }
                                        else
                                        {
                                            strs = string.Empty;
                                            strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" + fromWalletAddress + "',To_walletAddress =  '" + toWalletAddress + "'";
                                            strs += " WHERE Txnhash  = '" + hash + "' AND Formno =  '" + formNoV + "'";
                                            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                        }
                                    }


                                }
                            }
                        }



                    }
                    if (resultRR > 0)
                    {
                        string message = "Payment Successfully Added in Wallet.!";
                        string urlStr = "pointWallet.aspx";
                        string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later');", true);
                    }
                }
                catch (Exception ex)
                {
                    string errorQry = "";
                    string errorMsg = "";

                    try
                    {
                        errorMsg = ex.Message;
                    }
                    catch (Exception eXX) { }
                    finally
                    {
                        if (errorMsg != "") { } else { errorMsg = ""; }
                    }

                    errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) ";
                    errorQry += "VALUES('" + errorMsg + "', GETDATE(),'" + URL + "','" + toWalletAddress.Trim() + "','" + str + "','" + formNoV + "')";
                    int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later.');", true);
                }
            }
            catch (Exception ex) { }
        }
        catch (Exception ex) { }

        return balance;
    }
    public string Fund_Token_SendBEP(string senderAddress, string senderPrivateKey, decimal Balance, string Formno_V)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string URL = "";
        string postData = "";
        string str = string.Empty;
        decimal value = 0;
        string Code = "";
        DataSet ds = new DataSet();
        DataSet data = new DataSet();
        string StatusApi = "";
        string RetunnStatus = "";

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            URL = Session["BEPTOKEN"].ToString();
            WebRequest tRequest = WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.ContentLength = 0;
            postData = "{\"senderAddress\":\"" + senderAddress.Trim() + "\",\"senderPrivateKey\":\"" + senderPrivateKey.Trim() + "\",";
            postData += "\"recipientAddress\":\"\",\"tokenContractAddress\":\"0x55d398326f99059ff775485246999027b3197955\"}";
            //postData = "{\"senderAddress\":\"" + senderAddress.Trim() + "\",\"senderPrivateKey\":\"" + senderPrivateKey.Trim() + "\",\"amount\":\"" + decimal.Parse(Balance.ToString(), NumberStyles.Float) + "\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata, ApiType)";
            sql_req += " Values('" + sResult.Trim() + "','" + Formno_V.Trim() + "','" + URL.Trim() + "','" + postData.Trim() + "','BEPTOKENCHECK')";
            int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            str = tReader.ReadToEnd();
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + str.Trim() + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'BEPTOKENCHECK' ";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
            string SqlUpdate = "";
            string json = str;
            data = ConvertJsonStringToDataSet(str);
            StatusApi = data.Tables[0].Rows[0]["transactionstatus"].ToString();
            string Query = "";
            string hash_ = "";
            if (StatusApi.ToUpper().Trim() == "SUCCESSFUL")
            {
                try
                {
                    hash_ = data.Tables[0].Rows[0]["transactionHash"].ToString();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (hash_ == "") { hash_ = ""; }
                }
            }
            string strs = "";
            strs += "insert into ApiQrCodeReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB) ";
            strs += "Values('" + Formno_V + "','" + senderPrivateKey + "','" + senderAddress + "','" + senderPrivateKey + "','" + URL + "','" + str + "','" + StatusApi + "',";
            strs += "getdate(),'Token PayoutBEP','" + hash_ + "','" + decimal.Parse(Balance.ToString(), NumberStyles.Float) + "','" + postData + "','QrCodeBEP');";

            Query = " Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, Query);

            if (x > 0)
            {
                RetunnStatus = StatusApi;
            }
        }
        catch (Exception ex)
        {
            string errorQry = "";
            string ErrorMsg = "";
            try
            {
                ErrorMsg = ex.Message;
            }
            catch (Exception eXX) { }
            finally
            {
                if (ErrorMsg == "") { ErrorMsg = ""; }
            }
            string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + ErrorMsg + "' Where ReqID = '" + sResult.Trim() + "' AND ApiType = 'TOKENCHECK' ";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
            errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno)values('" + ErrorMsg + "',getdate(),'" + URL + "','" + senderAddress.Trim() + "','" + postData + "','" + Formno_V + "')";
            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);

            if (x1 > 0)
            {
                RetunnStatus = "failed";
            }
        }

        return RetunnStatus;
    }
    protected void BtnDepositClickBEP_Click(object sender, EventArgs e)
    {
        try
        {
            Fund_Balance_CheckBEP(BEPhdnPaymentId.Value, HdnFormno.Value, BEPOrderId.Value, bepprivatekey.Value);
        }
        catch (Exception ex)
        {

        }
    }
    private void GenerateQrCodeSummitSOl(string Formno)
    {
        HdnRate.Value = MMIDUSDT();
        string requestBody;
        string responseString;
        string CompleteUrl = "";
        int i = 0;
        DataSet dsLogin = new DataSet();
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        try
        {
            // Generate a unique identifier
            //string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //int randomNumber = new Random().Next(0, 999);
            //string uniqueIdentifier = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
            // Retrieve application-specific values
            string appId = "a4tcpLeAUW9Zaogy";
            string appSecret = "165d35b8912071d978c8c14911207131";
            CompleteUrl = "https://ccpayment.com/ccpayment/v2/getOrCreateUserDepositAddress";
            hdnPayID.Value = Session["idno"].ToString();
            var content = new
            {
                userId = Session["idno"],
                chain = "SOL" // Chain is set to SOLANA
            };
            requestBody = JsonConvert.SerializeObject(content);
            // Generate timestamp and sign
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string signText = appId + timestamp + (requestBody.Length != 2 ? requestBody : "");
            string serverSign;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signText));
                serverSign = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CompleteUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Appid", appId);
            request.Headers.Add("Sign", serverSign);
            request.Headers.Add("Timestamp", timestamp.ToString());
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(requestBody);
            }
            string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType)";
            sqlReq += " VALUES('" + sResult.Trim() + "','" + Formno.Trim() + "','" + CompleteUrl.Trim() + "','" + requestBody.Trim() + "','SUMMITADDRESSGET')";
            int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));
            // Handle the API response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                responseString = streamReader.ReadToEnd();
                string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + responseString.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND ApiType = 'SUMMITADDRESSGET' ";
                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var Data = jss.Deserialize<Object>(responseString);
                dsLogin = ConvertJsonStringToDataSet(responseString);
                SpnAddress.InnerText = dsLogin.Tables[1].Rows[0]["address"].ToString();
                ImgQrCode.Src = GenerateQRCode(SpnAddress.InnerText);
                fromidno.Value = Formno.ToString();
                HdnFormno.Value = Formno.ToString();
                string strs = "";
                strs += "insert into ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request,Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData) ";
                strs += "Values('" + fromidno.Value + "','" + OrderId.Value + "','" + SpnAddress.InnerText + "','" + privatekey.Value + "','" + CompleteUrl + "','" + responseString + "','Pending', getdate(), 'QrCode Generate SUMMIT-SOL', '', '0', '" + requestBody + "');";
                string Query = "Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                DAL objdal = new DAL();
                i = objdal.SaveData(Query);
            }
            if (i > 0)
            {
                DivQr.Visible = true;
                DivQrCodeAmount.Visible = true;
                DivMMIT.Visible = false;
                hdnPaymentId.Value = SpnAddress.InnerText;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "callJavaScriptFunction", "callJavaScriptFunction();", true);
            }
            else
            {
                string message = "Try Again.!";
                string url = "AddFundPayment.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
            }

        }
        catch (WebException ex)
        {
            // Log web exceptions
            using (var errorStream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(errorStream))
            {
                string errorResponse = reader.ReadToEnd();
                Console.WriteLine($"Error Response: {errorResponse}");
            }
            Console.WriteLine($"WebException: {ex.Message}");
        }
        catch (Exception ex)
        {
            string errorQry = "";
            errorQry = "insert Into TrnLogData(ErrorText, LogDate, Url, WalletAddress,ForType)";
            errorQry += "values('" + ex.Message + "', getdate(), '" + CompleteUrl + "', '" + SpnAddress.InnerText + "','A')";
            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);

        }
    }
    public string GenerateQRCode(string address)
    {
        string QRCodeBase64 = string.Empty;  // Declare the variable here

        try
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            qrCodeImage.Save(memoryStream, ImageFormat.Png);
                            byte[] qrCodeBytes = memoryStream.ToArray();
                            QRCodeBase64 = "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating QR code: {ex.Message}");
        }

        return QRCodeBase64;  // Return the base64 string
    }
}
//public class ApiResponseSuMMIT
//{
//    public int Code { get; set; }
//    public string Msg { get; set; }
//    public Data Data { get; set; }
//}
//public class Data
//{
//    public string Address { get; set; }
//    public string Memo { get; set; }
//}
