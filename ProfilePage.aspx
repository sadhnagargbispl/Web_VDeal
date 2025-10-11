<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ProfilePage.aspx.cs" Inherits="ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css" >
        .tb{
            Width: 100% !important;
            padding: 10px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row justify-content-center mt-3">
                <div class="col-lg-6 col-sm-12 col-12 px-2 mb-3 mmb-2">
                    <div class="text-title text-center">
                        <h4 class="text-white">Profile Update
                        </h4>
                        <p class="text-white">
                            Discover the ultimate convenience with our all-in-one platform
                        </p>
                    </div>
                    <div class="packagebox p-4">
                        <div class="clr">
                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                        <div class="form-group" id="sponsordetail" runat="server">
                            Sponsor ID
                            <br />
                            <P  >
                        <asp:TextBox ID="txtReferalId" class="form-control"  style="width:100%; padding:5px 8px; border-radius:5px; border :0 !important ; box-shadow :none !important; background-color: #e9ecef; " TabIndex="1" runat="server" AutoPostBack="True"
                            Enabled="False" ></asp:TextBox>
                                </P>
                        </div>
                        <div class="form-group " id="DivSponsorName" runat="server" visible="false">
                            <label class="control-label col-sm-3">
                                Sponsor Name<span class="red">*</span></label>
                            <asp:TextBox ID="TxtReferalNm" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                        </div>
                        <div class="form-group " id="DivUplinerId" runat="server" visible="false">
                            <label class="control-label col-sm-3">
                                Placement ID<span class="red">*</span></label>
                            <asp:TextBox ID="TxtUplinerid" class="form-control" TabIndex="1" runat="server" AutoPostBack="True"
                                Enabled="False"></asp:TextBox>
                        </div>
                        <div class="form-group " id="DivUplinerName" runat="server" visible="false">
                            <label class="control-label col-sm-3">
                                Placement Name<span class="red">*</span></label>
                            <asp:TextBox ID="TxtUplinerName" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                        </div>
                        <div class="form-group greybt" style="display: none;">
                            <label class="control-label col-sm-2">
                                Position<span class="red">*</span></label>
                            <asp:TextBox ID="lblPosition" class="form-control" runat="server" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group ">
                            Your Name
                        <asp:HiddenField ID="hdnSessn" runat="server" />
                            <asp:TextBox ID="txtFrstNm" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                runat="server" TabIndex="3" ValidationGroup="eInformation" ></asp:TextBox>
                        </div>
                        <div class="col-sm-2" style="display: none;">
                            <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server" TabIndex="7">
                                <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                <%-- <asp:ListItem Value="H/O" Text="H/O"></asp:ListItem>--%>
                                <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group ">
                            Father's Name
                        <asp:TextBox ID="txtFNm" runat="server" TabIndex="8" CssClass="form-control" ValidationGroup="eInformation"></asp:TextBox>
                        </div>
                        <div class="form-group ">
                            <label>
                                Country <span class="red">*</span></label>
                            <asp:DropDownList ID="ddlCountryName" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group" id="DivDOB" runat="server">
                            <div class="form-group  greybt" style="display: none">
                                <label class="control-label col-sm-2">
                                    Date of Birth</label>
                                <asp:TextBox ID="TxtDobDate" class="form-control" runat="server" TabIndex="9"></asp:TextBox>
                                <%--          <ajaxtoolkit:calendarextender id="CalendarExtender2" runat="server" targetcontrolid="TxtDobDate"
                                format="dd-MM-yyyy">
                            </ajaxtoolkit:calendarextender>--%>
                            </div>
                        </div>
                        <%--<div class="form-group ">
                            Mobile No.
                        <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required,custom[mobile]]"
                            TabIndex="15" runat="server" MaxLength="10" ValidationGroup="eInformation" Enabled="False"></asp:TextBox>
                        </div>--%>
                         <div class="row">
                <div class="col-sm-3" style="padding-Top: 30px;">
                  <asp:TextBox ID="ddlMobileNAme"  CssClass="form-control "
                runat="server"  ValidationGroup="eInformation" autocomplete="off" ></asp:TextBox>

                </div>
                <div class="col-sm-9" >
                <label>
             Mobile No.<span class="red">*</span></label>
    <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required,custom[mobile]]"
        TabIndex="15" runat="server" MaxLength="10" ValidationGroup="eInformation"></asp:TextBox>
                
                </div>
 
            </div>
                        <div class="form-group " style="display: none">
                            <label class="control-label col-sm-2">
                                Phone No.</label>
                            <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control "
                                TabIndex="16" runat="server" MaxLength="10" ValidationGroup="eInformation"></asp:TextBox>
                        </div>
                        <div class="form-group " id="Divcardno" runat="server" visible="false">
                            <label class="control-label col-sm-2">
                                Card No.</label>
                            <asp:TextBox ID="txtCardNo" CssClass="form-control" TabIndex="16" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group greybt ">
                           <%-- <label class="control-label col-sm-2">--%>
                                E-Mail ID<%--</label>--%>
                            <asp:TextBox ID="txtEMailId" CssClass="form-control" TabIndex="17" runat="server"
                                Enabled="False"></asp:TextBox>
                        </div>
                        <div class="form-group ">
                           <%-- <label class="control-label col-sm-3">--%>
                                Nominee Name<%--</label>--%>
                            <asp:TextBox ID="txtNominee" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                TabIndex="18" runat="server" Enabled="true"></asp:TextBox>
                        </div>
                        <div class="form-group greybt ">
                            <label class="control-label col-sm-2">
                                Relation</label>
                            <asp:TextBox ID="txtRelation" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                TabIndex="19" runat="server" Enabled="False"></asp:TextBox>
                        </div>
                        <div class="form-group ">
                            <asp:Button ID="CmdSave" runat="server" Text="Update" class="btn btn-primary buttoncss text-white w-100"
                                TabIndex="27" ValidationGroup="eInformation" OnClick="CmdSave_Click" />
                            &nbsp;<asp:Button ID="CmdCancel" runat="server" Text="Cancel" class="btn btn-primary buttoncss text-white w-100"
                                TabIndex="28" ValidationGroup="Form-Reset" Visible="false" />
                        </div>
                        <hr>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

