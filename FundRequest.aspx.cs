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

public partial class FundRequest : System.Web.UI.Page
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
            this.BtnSaveDB.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSaveDB));
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringActive(6);
                    FillPaymode();
                    CheckVisible();
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
            //if (Dr[0]["Isimage"].ToString() == "N")
            //    divImage.Visible = false;
            //else
            //    divImage.Visible = true;

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
                    TxtDDNo.Text = "";
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
    private void CompressAndSaveImage(Stream inputStream, string savePath, string extension, long quality = 50L)
    {
        using (System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream))
        {
            EncoderParameters encoderParams = new EncoderParameters(1);
            ImageCodecInfo codec = null;

            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/jpeg");
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                    break;

                case ".png":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/png");
                    encoderParams = null; // PNG doesn't support quality settings
                    break;

                case ".gif":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/gif");
                    encoderParams = null;
                    break;

                default:
                    throw new Exception("Unsupported file type.");
            }

            if (codec != null)
            {
                if (encoderParams != null)
                {
                    img.Save(savePath, codec, encoderParams);
                }
                else
                {
                    img.Save(savePath, codec, null);
                }
            }
        }
    }
    protected void SaveRequest()
    {
        try
        {
            string StrSql = "Insert into Trnfundwithdrawcpanel (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
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
                            if (FlDoc.Enabled)
                            {
                                if (!FlDoc.HasFile)
                                {
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                                    return;
                                }
                            }
                            if (FlDoc.HasFile)
                            {
                                strextension = System.IO.Path.GetExtension(FlDoc.FileName);
                                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                                {
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(FlDoc.PostedFile.InputStream);
                                    int height = img.Height;
                                    int width = img.Width;
                                    decimal size = Math.Round((decimal)(FlDoc.PostedFile.ContentLength) / 1024, 1);
                                    if (size > 1024)
                                    {
                                        string scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                                        return;
                                    }
                                    else
                                    {

                                        string flAddrs = FlDoc.PostedFile.FileName;
                                        //ImageUpload.PostedFile.SaveAs(Server.MapPath("MM_voucher/") + flAddrs);
                                        string fileName = DateTime.Now.ToString("yyMMddhhmmssfff") + "_1" + System.IO.Path.GetExtension(FlDoc.FileName);
                                        string savePath = Server.MapPath("images/UploadImage/") + fileName;
                                        FlDoc.PostedFile.SaveAs(savePath);
                                        CompressAndSaveImage(FlDoc.PostedFile.InputStream, savePath, strextension, 50L); // Quality 50%
                                        FlNm = fileName ;
                                    }
                                }
                                else
                                {
                                    string scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                                    return;
                                }
                            }
                            else
                            {
                                FlNm = "";
                            }
                            //if (FlDoc.HasFile)
                            //{
                            //    string filename = Path.GetFileName(FlDoc.PostedFile.FileName);
                            //    //string strname = Format(Now, Format(Now, "yyMMddhhmmssfff")) + "_1" + System.IO.Path.GetExtension(FlDoc.FileName);
                            //    string strname = DateTime.Now.ToString("yyMMddhhmmssfff") + "_1" + System.IO.Path.GetExtension(FlDoc.FileName);
                            //    string targetPath = Server.MapPath("images/UploadImage/" + strname);
                            //    Stream strm = FlDoc.PostedFile.InputStream;
                            //    var targetFile = targetPath;
                            //    GenerateThumbnails(0.5, strm, targetFile);
                            //    FlNm = strname;
                            //}
                            //else if (DdlPaymode.SelectedValue == "1")
                            //{
                            //}
                            //else
                            //{
                            //    scrname = "<SCRIPT language='javascript'>alert('Select Choose File');" + "</SCRIPT>";
                            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                            //    return;
                            //}
                        }

                        string str = "INSERT INTO WalletReq(ReqNo,ReqDate,Formno,PID,Paymode,Amount,ChqNo,ChqDate,BankName,BranchName,ScannedFile,Remarks,BankId,Transno) " +
         " " + "Select ISNULL(Max(ReqNo)+1,'1001'),'" + DateTime.Now.ToString("dd-MMM-yyyy").ToString() + "'," +
         "'" + Session["Formno"].ToString() + "','" + DdlPaymode.SelectedValue + "','" + DdlPaymode.SelectedItem.Text + "','" + TxtAmount.Text + "'," +
         "'" + TxtDDNo.Text + "','" + ChqDate.ToString() + "','','','" + FlNm + "','','0','0' FROM WalletReq " + "; " +
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
                            //  string message = "Payment Request Sent Successfully.\\nYour Request no. is " + ReqNo + "";
                            scrname = "<SCRIPT language='javascript'>alert('Payment Request Sent Successfully.Your Request no. is " + ReqNo + "');" + "</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                            // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
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
                string urlStr = "FundRequest.aspx";
                string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void TxtDDNo_TextChanged(object sender, EventArgs e)
    {
        CheckDDno();
    }
}
