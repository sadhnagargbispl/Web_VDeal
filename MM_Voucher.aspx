<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="MM_Voucher.aspx.cs" Inherits="MM_Voucher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- About Section Start -->
    <div class="about-us-area pt-30 pb-20 bg-white">
        <div class="container" style="max-width: 1500px;">
            <div class="row align-items-center justify-content-center">

                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-1 text-center">
                    <h3>
                        <asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label></h3>
                    <p class="text-dark">
                        <asp:Label ID="LblDis" runat="server" Text=""></asp:Label>
                    </p>
                </div>

                <asp:Repeater ID="RepFoodMovie" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                            <div class="card text-center">
                                <a href='<%# String.Format("activation.aspx?KitID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("KitID").ToString()))) %>'>
                                    <img src='<%#Eval("kitimg")%>' alt="banner" class="img-fluid img-responsive w-100">
                                </a>
                                <div class="description mt-3 mb-3">
                                    <a href='<%# String.Format("activation.aspx?KitID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("KitID").ToString()))) %>'
                                        type="button" class="btn-danger mt-2 p-1"><i class="fa fa-cart-arrow-down" aria-hidden="true"></i>Buy Packages</a>
                                    <%--  <a href="packages-details.asp">
                <button class="btn-danger mt-2 p-1"><i class="fa fa-cart-arrow-down" aria-hidden="true"></i>Buy Packages</button>
            </a>--%>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>



            </div>
        </div>
    </div>



    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-center">
                    <h4 class="text-white">Food Booking / Movie Package
                    </h4>
                    <p class="mt-2 text-white">
                        Discover the ultimate convenience with our all-in-one platform: shop online, manage
                        utilities, book holidays and movies,
                        <br>
                        order food, and find perfect gift vouchers effortlessly. Stay tuned for our upcoming
                        Petro Card to top-up your tank and your life with ease.
                    </p>
                    <hr>
                </div>

            </div>
        </div>
    </div>
    <!-- About Section End -->
</asp:Content>

