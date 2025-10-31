<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Activation.aspx.cs" Inherits="Activation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="product-details-area pt-30 pb-20 bg-white">
        <div class="container">
            <div class="row align-items-center mt-4">

                <!-- Product Image -->
                <div class="col-xl-5 col-lg-12 text-center">
                    <img id="Img" src="" runat="server" alt="product-image" class="img-fluid card" />
                    <%--     <img src="img/packages/food_6000.jpg" class="img-fluid card" alt="product-image">--%>
                    <hr class="mt-0" color="#000">
                </div>

                <!-- Product Info -->
                <div class="col-xl-7 col-lg-12">
                    <div class="product-info">
                        <h4 class="text-dark">
                            <asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label></h> 
          <%--<p class="fontsize"> 12 X 500 =  Rs. 6000/-  </p>--%>
                            <hr class="m-2">
                        <p>
                            <strong>User Name:</strong>
                            <asp:Label ID="LblUserName" runat="server" Text=""></asp:Label>
                            &nbsp; <strong>ID Number:</strong>
                            <asp:Label ID="LblUserID" runat="server" Text=""></asp:Label>
                        </p>
                        <h5><span style="text-decoration: line-through; color: red; display: none;">MRP: ₹<asp:Label ID="LblPakageAmount" runat="server" Text="0.00"></asp:Label></span> &nbsp; 
                            <span style="color: green;">Offer Price: ₹
                            <asp:Label ID="lbljoinamount" runat="server" Text="0.00"></asp:Label></span></h5>
                        <div class="form-group" runat="server" visible="false">
                            <label class="mt-2 text-white">
                                Amount (Gift Wallet) &nbsp;&nbsp;<span style="color: #5c13b1 !important; font-weight: bolder">(You Can Use Upto 70% Amount From Gift Wallet Here)</span></label>
                            <asp:TextBox ID="TextBox1" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>

                        </div>
                        <div class="form-group mt-3 text-dark" style="display: none;">
                            <label for="paymode">Select Paymode</label>
                            <asp:DropDownList ID="DDLPaymode" runat="server" class="form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="DDLPaymode_SelectedIndexChanged" Enabled="true">
                                <%--  <asp:ListItem Text="---Select Paymode---" Value="0"></asp:ListItem>--%>
                                <asp:ListItem Text="Wallet" Value="1"></asp:ListItem>
                                <%--<asp:ListItem Text="Payment Getway" Value="2"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group mt-2">
                            <label>Point Wallet Balance :</label>
                            <asp:TextBox ID="LblGiftWalletBala" runat="server" class="form-control" aria-describedby="emailHelp"
                                ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group mt-2">
                            <label>Amount</label>
                            <asp:TextBox ID="TxtAmount" runat="server" class="form-control" aria-describedby="emailHelp"
                                placeholder="Enter Amount" AutoPostBack="true" OnTextChanged="TxtAmount_TextChanged" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group mt-2">
                            <label class="control-label">Transaction Password</label>
                            <div class="controls">
                                <asp:TextBox ID="TxtTransPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        <%--  <a href="packages-details.asp">--%>
                        <asp:Button ID="BtnProceedToPay" runat="server" Text="Buy" class="btn-danger mt-1 p-2" OnClick="BtnProceedToPay_Click" />
                        <%-- <button class="btn-danger mt-1 p-2"><i class="fa fa-mobile-phone" aria-hidden="true"></i>Send OTP - Mobile / Email    </button>--%>
                        <%-- </a>--%>

                        <%--                        <div class="form-group mt-2">
                            <input type="text" class="form-control mt-3" placeholder="Enter OTP">
                        </div>--%>
                    </div>
                </div>
            </div>

            <!-- Product Description Tabs -->
            <div class="row mt-4">
                <div class="col-12" runat="server" id="DivMDescription_Food" visible="False">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item active" role="presentation">
                            <a class="nav-link" href="#desc" aria-controls="desc" role="tab" data-toggle="tab">Description</a>
                        </li>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" href="#how" aria-controls="how" role="tab" data-toggle="tab">How to Use</a>
                        </li>
                      <%--  <li class="nav-item" role="presentation">
                            <a class="nav-link" href="#rupay" aria-controls="rupay" role="tab" data-toggle="tab">How to use Rupay Card?</a>
                        </li>--%>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" href="#terms" aria-controls="terms" role="tab" data-toggle="tab">Terms & Conditions</a>
                        </li>
                    </ul>

                    <div class="tab-content p-4">
                        <div id="desc" class="tab-pane active" role="tabpanel">
                            <p>
                                <asp:Label ID="LblFoodDis" runat="server" Text=""></asp:Label>
                            </p>
                        </div>
                        <div id="how" class="tab-pane" role="tabpanel">
                            <p>
                                <asp:Label ID="LblFoodUse" runat="server" Text="Label"></asp:Label>
                            </p>
                        </div>
                     <%--   <div id="rupay" class="tab-pane" role="tabpanel">
                            <p>
                                <asp:Label ID="lblhowtouse" runat="server" Text=""></asp:Label>
                            </p>
                        </div>--%>
                        <div id="terms" class="tab-pane" role="tabpanel">
                            <asp:Label ID="LblFoodTerms" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>


                </div>
                <%--<div class="col-12">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item active" role="presentation">
                            <a class="nav-link" href="#desc" aria-controls="desc" role="tab" data-toggle="tab">Description</a>
                        </li>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" href="#how" aria-controls="how" role="tab" data-toggle="tab">How to Use</a>
                        </li>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" href="#rupay" aria-controls="rupay" role="tab" data-toggle="tab">How to use Rupay Card?</a>
                        </li>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" href="#terms" aria-controls="terms" role="tab" data-toggle="tab">Terms & Conditions</a>
                        </li>

                    </ul>

                    <div class="tab-content p-4">
                        <div id="desc" class="tab-pane active" role="tabpanel">
                            <p>Product description here. Highlight benefits, usage, and features.</p>
                        </div>
                        <div id="how" class="tab-pane" role="tabpanel">
                            <p>Step by step guide on how to use the product.</p>
                        </div>
                        <div id="rupay" class="tab-pane" role="tabpanel">
                            <p>Instructions for using Rupay card with this product.</p>
                        </div>
                        <div id="terms" class="tab-pane" role="tabpanel">
                            <ul>
                                <li>Coupons cannot be transferred to another user or name.</li>
                                <li>Coupons can be used by the member name only.</li>
                                <li>Not applicable for bulk buying or wholesale activity.</li>
                                <li>Maximum one coupon can be used at a time.</li>
                                <li>Only one coupon can be used in a day.</li>
                                <li>User can use the coupon on the same service 30 days after the date of last usage.</li>
                            </ul>
                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>



</asp:Content>

