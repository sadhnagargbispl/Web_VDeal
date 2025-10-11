using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Img : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DAL obj = new DAL();
            string type = Request["Type"];
            string FormNo = Request["ID"];

            if (Request["Type"] == "ad")
            {
                string sql = "select '" + Session["AdminWeb"].ToString() + "/images/UploadImage/' + ImgPath as Img1Path from M_AdvertiseMaster where AdID='" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["Img1Path"].ToString();
                }
            }
            else if (Request["Type"] == "Glry")
            {
                string sql = "select '" + Session["AdminWeb"].ToString() + "/images/UploadImage/' + ImagePath as ImgPath from ProductGallery where PGID='" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImgPath"].ToString();
                }
            }
            else if (Request["Type"] == "PinRequest")
            {
                string sql = "select Case when ImgPath='' then '' when ImgPath like 'http%' then ImgPath else '" + Session["CompWeb"].ToString() + "/images/UploadImage/' + ImgPath end as ImagePath from TrnPinReqMain where ReqNo='" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
                }
            }
            else if (Request["Type"] == "Payment")
            {

                string sql = "select Case when ScannedFile='' then '' else 'images/UploadImage/' + ScannedFile end as ImagePath from WalletReq where ReqNo='" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);


                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
                }
            }
            else if (Request["Type"] == "vendorbill")
            {

                string sql = "select Case when ScannedFile='' then '' else 'images/UploadImage/' + ScannedFile end as ImagePath from BillUploadReq where ReqNo = '" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);


                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
                }
            }
            else if (Request["Type"] == "walletpayment")
            {
                string sql = "select CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://cpanel.solfit.in/images/UploadImage/' + ScannedFile end as ImagePath from WalletReq where ReqNo='" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
                }
            }
            else if (Request["Type"] == "booking")
            {
                string sql = "select Case when ScanneFile='' then '' else '" + Session["CompWeb"].ToString() + "images/UploadImage/' + ScanneFile end as ImagePath from BookingRequest where ReqNo='" + Convert.ToInt32(Request["ID"]) + "'";
                dt = obj.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
                }
            }
            else if (Request["Type"] == "Invoice")
            {
                string sql = "select Invoiceurl as ImagePath from Invoice where Formno='" + Convert.ToInt32(Request["ID"]) + "' and Id='" + Convert.ToInt32(Request["Reqid"]) + "'";
                dt = obj.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
                }
            }
            else if (Request["Type"] != null)
            {
                // Handle other cases if needed
            }
            else
            {
                // Default case if none of the above match
                // Image1.ImageUrl = "ImgHandler.ashx?id=" + Request["ID"];
            }
        }
        catch (Exception ex)
        {
            // Handle error (optional)
            Response.Write("Error: " + ex.Message);
        }
    }
}