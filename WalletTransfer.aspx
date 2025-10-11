<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="WalletTransfer.aspx.cs" Inherits="WalletTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- About Section Start -->
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-center">
                    <h4 class="text-white">Wallet Transfer
                    </h4>
                    <hr>
                </div>
                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                <div class="col-lg-6 col-sm-6 col-12 px-2 mb-3 mmb-2" runat="server" id="DivLoginCheck">
                    <div class="packagebox p-5">
                        <div class="form-group">
                            <h5 class="mt-2 text-white">Available Wallet Balance : <b>Rs.
            <asp:Label ID="LblGiftWalletBala" runat="server" Text="0"></asp:Label></b>
                            </h5>
                        </div>
                        <div class="form-group">
                            <label for="exampleInputEmail1">
                                User ID</label>
                            <asp:TextBox ID="txtMemberId" runat="server" class="form-control" aria-describedby="emailHelp"
                                placeholder="Enter User ID " AutoPostBack="true" OnTextChanged="TxtUserID_TextChanged"></asp:TextBox>
                        </div>
                        <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                        <div class="form-group" id="DivMemberName" runat="server">
                            <label for="inputdefault">
                                Member Name</label>
                            <asp:HiddenField ID="HdnFormno" runat="server" />
                            <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                            <asp:TextBox ID="TxtMemberName" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="exampleInputEmail1">
                                Amount</label>
                            <asp:TextBox ID="txtAmount" runat="server" class="form-control" aria-describedby="emailHelp"
                                placeholder="Enter Amount " AutoPostBack="true" OnTextChanged="TxtAmount_TextChanged"></asp:TextBox>
                            <asp:Label ID="LblAmount" runat="server" Visible="false"></asp:Label>
                        </div>
                        <asp:Button ID="BtnProceedToPay" runat="server" Text="Send Otp" class="btn btn-primary buttoncss text-white w-100" OnClick="BtnProceedToPay_Click" />
                        <div runat="server" id="divotp" visible="false">
                            <div class="form-group mb-3">
                                <label for="inputdefault">
                                    Enter OTP Sent on your E-mail Id.
                                </label>
                                <asp:TextBox ID="TxtOtp" class="form-control validate[required]" runat="server"
                                    autocomplete="off" placeholder="Enter OTP"></asp:TextBox>
                                <span style="color: Red; font-weight: bold; font-size: 1em">OTP is only valid for 5 min.</span>
                            </div>
                        </div>

                        <div class="form-group mb-3">
                            <asp:Button ID="BtnOtp" runat="server" Text="Submit" class="btn btn-primary buttoncss text-white w-100" Visible="false " OnClick="BtnOtp_Click" />
                            <asp:Button ID="ResendOtp" runat="server" Text="Resend Otp" class="btn btn-primary buttoncss text-white w-100"
                                Visible="false " OnClick="ResendOtp_Click" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="None" ControlToValidate="txtOtp"
                                runat="server" ErrorMessage="Opt Required" SetFocusOnError="true" ValidationGroup="eInformation"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- About Section End -->
</asp:Content>


