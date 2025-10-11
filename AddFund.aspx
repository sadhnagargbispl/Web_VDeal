<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="AddFund.aspx.cs" Inherits="AddFund" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script>
        function myFunction() {
            debugger;
            var x = document.getElementById("ContentPlaceHolder1_TxtPassword");
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- About Section Start -->
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-center">
                    <h4 class="text-white">Add Fund
                    </h4>
                    <hr>
                </div>
                <div class="col-lg-6 col-sm-6 col-12 px-2 mb-3 mmb-2" runat="server" id="DivLoginCheck">
                    <div class="packagebox p-5">

                        <div class="form-group">
                            <label for="exampleInputEmail1">
                                User ID</label>
                            <asp:TextBox ID="TxtUserID" runat="server" class="form-control" aria-describedby="emailHelp"
                                placeholder="Enter User ID "></asp:TextBox>

                        </div>
                        <div class="form-group">
                            <label for="exampleInputPassword1">
                                Password</label>
                            <asp:TextBox ID="TxtPassword" runat="server" class="form-control" aria-describedby="emailHelp"
                                placeholder="Enter Password " TextMode="Password"></asp:TextBox>
                            <p style="color: black;">
                                <input type="checkbox" onclick="myFunction()">
                                Show Password
                            </p>
                        </div>

                        <asp:Button ID="BtnProceedToPay" runat="server" Text="Submit" class="btn btn-primary buttoncss text-white w-100" OnClick="BtnProceedToPay_Click" />

                    </div>
                </div>

                <div class="col-lg-6 col-sm-6 col-12 px-2 mb-3 mmb-2" runat="server" id="DivConfirm" visible="false">
                    <div class="packagebox p-5">
                        <div class="form-group">
                            <h5 class="mt-2" style="color: #5c13b1 !important;">Available Wallet Balance : <b>Rs.
                                    <asp:Label ID="LblGiftWalletBala" runat="server" Text="0"></asp:Label></b>
                            </h5>
                        </div>
                        <div class="form-group">
                            <h6 style="color: #5c13b1 !important;">Enter Amount </h6>
                            <asp:TextBox ID="TxtReqAmount" runat="server" CssClass="form-control" aria-describedby="emailHelp"
                                placeholder="Enter Request Amount"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="BtnconfirmBtotton" runat="server" Text="Send Otp" class="btn btn-primary buttoncss text-white w-100" OnClick="BtnconfirmBtotton_Click" />
                        </div>
                        <div runat="server" id="divotp" visible="false">
                            <div class="form-group">
                                <h6 style="color: #5c13b1 !important;">Enter OTP Sent on your E-mail Id (<b><asp:Label ID="LblUserEmail" runat="server" Text=""></asp:Label></b>).
                                    <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span>

                                </h6>
                                <asp:TextBox ID="TxtOtp" CssClass="form-control validate[required]" runat="server"
                                    autocomplete="off" placeholder="Enter OTP"></asp:TextBox>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="TxtOtp"
                                    runat="server" ValidationGroup="Save">Otp Required
                                </asp:RequiredFieldValidator>--%>
                            </div>

                            <br />
                        </div>
                        <div class="form-group">

                            <div class="row ">
                                <div class="col-md-6">
                                    <asp:Button ID="BtnOtp" runat="server" Text="Submit" class="btn btn-primary buttoncss text-white w-100"
                                        Visible="false"
                                        ValidationGroup="Save" OnClick="BtnOtp_Click" />
                                    <br />
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Button ID="ResendOtp" runat="server" Text="Resend Otp" class="btn btn-primary buttoncss text-white w-100"
                                        Visible="false" OnClick="ResendOtp_Click" />
                                </div>
                            </div>

                        </div>
                        <asp:HiddenField ID="HdnFirstName" runat="server" />
                        <asp:HiddenField ID="Hdndoj" runat="server" />
                        <asp:HiddenField ID="HdnEmail" runat="server" />
                        <asp:HiddenField ID="HdnMobileNo" runat="server" />
                        <asp:HiddenField ID="HdnCity" runat="server" />
                        <asp:HiddenField ID="HdnIsActive" runat="server" />
                        <asp:HiddenField ID="Hdnkitid" runat="server" />
                        <asp:HiddenField ID="Hdnnoofid" runat="server" />
                        <asp:HiddenField ID="Hdnkitstatus" runat="server" />
                        <asp:HiddenField ID="Hdncoupon" runat="server" />
                        <asp:HiddenField ID="Hdnshoppoint" runat="server" />
                        <asp:HiddenField ID="HdnAmount" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- About Section End -->
</asp:Content>

