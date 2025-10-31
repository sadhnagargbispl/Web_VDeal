<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="IdActivation.aspx.cs" Inherits="IdActivation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="register-section py-5">
        <div class="container">
            <div class="row justify-content-center">

                <div class="col-xl-5 col-lg-6 col-md-8">
                    <div class="card shadow border-0 rounded-4">
                        <div class="card-body p-4">

                            <h4 class="text-center text-dark mb-1"><b>ID Activation</b></h4>
                            <hr class="mb-4">
                            <h4>Available Fund Wallet Balance:<span class="red" id="AvailableBal" style="color: Red" runat="server"></span>
                            </h4>
                            <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="error-message"></asp:Label>
                            <div>
                                <!-- Full Name -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Member Id<span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtMemberId" runat="server" CssClass="form-control validate[required]"
                                            AutoPostBack="true" OnTextChanged="txtMemberId_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                                        <asp:HiddenField ID="hdnMacadrs" runat="server" />
                                        <asp:HiddenField ID="HdnTopupSeq" runat="server" />
                                        <asp:HiddenField ID="HdnMemberMacAdrs" runat="server" />
                                        <asp:HiddenField ID="HdnMemberTopupseq" runat="server" />
                                        <asp:HiddenField ID="MemberStatus" runat="server" />
                                        <asp:HiddenField ID="hdnFormno" runat="server" />
                                        <asp:HiddenField ID="hdnemail" runat="server" />
                                    </div>
                                </div>

                                <div class="mb-3" id="DivMemberName" runat="server">
                                    <label class="form-label fw-bold">
                                        Member Name</label>
                                    <div class="input-group">
                                        <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group" style="display: none">
                                    <label class="form-label fw-bold">
                                        Payment Type</label>
                                    <asp:DropDownList ID="DDLPaymode" runat="server" class="form-control" AutoPostBack="true">
                                        <asp:ListItem Text="Wallet" Value="1"></asp:ListItem>

                                    </asp:DropDownList>
                                </div>
                                <asp:Label ID="LblCondition" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="kitid" runat="server" Visible="false"></asp:Label>
                                <div class="mb-3" id="DivCurrency" runat="server" visible="false">
                                    <label class="form-label fw-bold">
                                        Currency <span class="red">*</span></label>
                                    <asp:DropDownList ID="ddlcurrency" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="mb-3" id="Div1" runat="server">
                                    <label class="form-label fw-bold">
                                        Package</label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="CmbKit" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CmbKit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="mb-3" runat="server">
                                    <label class="form-label fw-bold">
                                        Amount <span class="red"><span style="color: red !important; font-weight: bolder; font-size: 0.9em;">*</span></span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAmount" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged" Text="0" onkeypress="return isNumberKey(event);" ReadOnly="true"></asp:TextBox>
                                        <asp:Label ID="LblAmount" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblAmountUse" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Transction Password</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="TxtTransPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <asp:Button ID="cmdSave1" runat="server" Text="Submit" class="btn btn-danger" ValidationGroup="Validation" OnClick="cmdSave1_Click" />
                                    <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-12 text-center">
                    <br />
                    <div class="form-group" id="div2" runat="server" style="text-align: center;">
                        <%-- <img class="img-fluid" src="assets/img/QRcode.jpeg" alt="QR Code" width="320" />--%>
                    </div>
                </div>
            </div>
        </div>
    </section>


</asp:Content>

