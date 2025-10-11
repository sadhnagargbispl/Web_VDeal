<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="WellCome.aspx.cs" Inherits="WellCome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- About Section Start -->
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row align-items-left">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-left">
                    <h4 class="text-white">My Mega Mart : Product Packages
                    </h4>
                    <hr>
                </div>
                <hr>
                <div class="col-lg-8 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2">
                        <div class="thankyou">
                            <h4 class="text-white">Thank You for Your Purchase!
                            </h4>
                            <p class="mt-2 text-white">
                                Thank you for your recent purchase of <b>Shopping Package! </b>
                            </p>
                            <p class="mt-2 text-white">
                                We are thrilled to have you as a customer and hope you enjoy your new [product/package].
                                Your support means a lot to us, and we are committed to providing you with the best
                                experience possible.
                            </p>
                            <h4 class="text-white">Order Details :
                            </h4>
                            <p class="mt-2 text-white">
                                Order Number: [Order Number]
                            </p>
                            <p class="mt-2 text-white">
                                Order Date: [Order Date]
                            </p>
                            <p class="mt-2 text-white">
                                Item(s) Purchased: [List of Items]
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <img src="img/thankyou.png" alt="banner" class="img-fluid img-responsive w-100">
                </div>
            </div>
        </div>
    </div>
    <!-- About Section End -->
    <!-- About Section Start -->
    <div class="about-us-area pt-30 pb-20 bg-black">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-xl-5 col-lg-12 d-lg-block d-sm-block d-none" align="center">
                    <img src="img/homeimg.png" class="img-fluid" alt="shopping-image">
                    <hr class="mt-0" color="#000">
                </div>
                <div class="col-xl-7 col-lg-12" align="justify">
                    <div class="about-us-text">
                        <h4>Know More About : My Mega Mart</h4>
                        <p class="text-dark mt-4">
                            When the night comes, it's great to relax with a movie at home. You can order food
                            from an app, choose a movie you like, and enjoy it on your couch.
                        </p>
                        <p class="text-dark ">
                            Gift vouchers aren't just for buying things at stores. They're also a cool way to
                            give someone a special meal at a nice restaurant. You can surprise a friend or family
                            member with a gift voucher to a popular restaurant they like.
                        </p>
                        <p class="text-dark ">
                            Having a movie night with friends is a great way to hang out. You can order yummy
                            snacks online and get them delivered to your house. To make it even better, you
                            could all chip in and get gift cards for a movie night out together.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- About Section End -->
</asp:Content>

