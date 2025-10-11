<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Service Section Start -->
    <div class="about-us-area pt-10 pb-20 bg-light services">
        <div class="container" style="max-width: 1500px;">
            <div class="row justify-content-center mt-4">

                <div class="col-lg-12 col-sm-12 col-12 px-2 mb-1 text-center">
                    <h3>Smart Shopping Starts with Smart Deals. </h3>
                    <small class="text-dark">Where every purchase brings Smart Savings! </small>
                    <p class="text-dark">Discover quality products, unbeatable prices, and exclusive deals that make every buy truly worth it.  </p>
                </div>
                <asp:Repeater ID="RepFoodMovie" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                            <div class="card text-center">
                                <img src='<%#Eval("kitimg")%>' alt="banner" class="img-fluid img-responsive w-100">
                                <div class="description mt-3 mb-3">
                                    <h5 class="text-uppercase"><%#Eval("kitname")%></h5>
                                    <p class="mb-0 text-dark"><%#Eval("DescriptionWithBR")%> </p>
                                    <a href='<%# String.Format("MM_Voucher.aspx?CategoryID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("id").ToString()))) %>'
                                        type="button" class="btn-danger mt-2 p-1">Packages</a>
                                    <a href='<%#Eval("WebsiteURL")%>' target="_blank" type="button" class="btn-info mt-2 p-1">View Website
                                    </a>
                                    <a href='<%#Eval("WebsiteURL")%>' target="_blank" type="button" class="btn-danger mt-2 p-1">Redeem 
                                    </a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>


            </div>
        </div>
    </div>

    <div class="about-us-area pt-30 pb-20 bg-white services">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-xl-5 col-lg-12 d-lg-block d-sm-block d-none" align="center">
                    <img src="img/homeimg.png" class="img-fluid card" alt="shopping-image">
                    <hr class="mt-0" color="#000">
                </div>
                <div class="col-xl-7 col-lg-12" align="justify">
                    <div class="about-us-text">
                        <h4>Every Deal for Savings, Full of Quality. </h4>
                        <p class="text-dark mt-4 mb-2">When the night comes, it's great to relax with a movie at home. You can order food from an app, choose a movie you like, and enjoy it on your couch. </p>
                        <p class="text-dark mb-2">Gift vouchers aren't just for buying things at stores. They're also a cool way to give someone a special meal at a nice restaurant. You can surprise a friend or family member with a gift voucher to a popular restaurant they like.</p>
                        <p class="text-dark mb-2">Having a movie night with friends is a great way to hang out. You can order yummy snacks online and get them delivered to your house. To make it even better, you could all chip in and get gift cards for a movie night out together.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- About Section End -->
</asp:Content>

