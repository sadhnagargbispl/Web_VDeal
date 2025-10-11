<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="FundRequest.aspx.cs" Inherits="FundRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="register-section py-5">
        <div class="container">
            <div class="row justify-content-center">

                <div class="col-xl-5 col-lg-6 col-md-8">
                    <div class="card shadow border-0 rounded-4">
                        <div class="card-body p-4">

                            <h4 class="text-center text-dark mb-1"><b>Fund Request</b></h4>
                            <hr class="mb-4">
                            <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="error-message"></asp:Label>
                            <div>
                                <!-- Full Name -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Enter Amount<span class="red">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" onkeypress="return isNumberKey(event);" MaxLength="8"
                                            TabIndex="1" ID="TxtAmount" class="form-control validate[required]" Text="0"></asp:TextBox>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Select Paymode<span class="red">*</span></label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control"
                                            TabIndex="2" OnSelectedIndexChanged="DdlPaymode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!-- Email -->


                                <!-- Mobile -->
                                <div class="mb-3" id="divDDno" runat="server" visible="false">
                                    <label class="form-label fw-bold">
                                        <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="TxtDDNo" onkeypress="return isNumberKey(event);" class="form-control validate[required]"
                                            TabIndex="3" runat="server" MaxLength="16" AutoPostBack="true" OnTextChanged="TxtDDNo_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- Password -->
                                <div class="mb-3" id="divDDDate" runat="server">
                                    <label class="form-label fw-bold">
                                        <asp:Label ID="LblDDDate" runat="server" Text="Transaction Date *"></asp:Label></label>
                                    <div class="input-group">

                                        <asp:TextBox ID="TxtDDDate" runat="server" class="form-control validate[required]" TabIndex="4"></asp:TextBox>



                                    </div>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate"
                                        Format="dd-MMM-yyyy" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtDDDate"
                                        ErrorMessage="Invalid Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>

                                <!-- Confirm Password -->
                                <div class="mb-3" id="divImage" runat="server">
                                    <label class="form-label fw-bold">Scanned Copy:</label>
                                    <div class="input-group">
                                        <asp:FileUpload runat="server" ID="FlDoc" class="form-control" TabIndex="7" />
                                        <asp:Label ID="LblImage" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <!-- Submit Button -->
                                <div class="d-grid mb-3">
                                    <asp:Button ID="BtnSaveDB" runat="server" Text="Confirmed" class="btn-danger btn-lg rounded-pill" OnClick="BtnSaveDB_Click" />
                                    <%--  <button type="submit" class="btn-danger btn-lg rounded-pill w-100"><i class="fa fa-user-plus me-2"></i>Register </button>--%>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-12 text-center">
                    <br />
                    <div class="form-group" id="div1" runat="server" style="text-align: center;">
                        <%-- <img class="img-fluid" src="assets/img/QRcode.jpeg" alt="QR Code" width="320" />--%>
                    </div>
                </div>
            </div>
        </div>
    </section>




</asp:Content>

