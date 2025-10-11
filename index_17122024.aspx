<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about-us-area pt-10 pb-20 bg-light background-1">
        <div class="container">
            <!-- <div class="about-us-text mb-30 text-center">
            <h4>Explore Our Services</h4>
            <p class="d-lg-block d-sm-block d-none"> Welcome to My Mega Mart, your ultimate gateway to discovering a world of exceptional services tailored to meet your every need. <br> Explore Services is here to connect you with the best in the business. </p> -->
            <!-- </div> -->
            <div class="row justify-content-center mt-4">
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="OnLineShoppingRedirect.aspx" target="_blank">
                            <img src="img/services/thum_img_1.jpg" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Online Shopping
                            </h6>
                            <p class="mb-0 text-dark">
                                Your One-Stop Online Shop.
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="UtilityServicesRedirect.aspx" target="_blank">
                            <img src="img/services/thum_img_2.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Utility Portal
                            </h6>
                            <p class="mb-0 text-dark">
                                Bright your world with each switch flip.
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="HolidayPortalRedirect.aspx" target="_blank">
                            <img src="img/services/thum_img_3.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Holiday Portal
                            </h6>
                            <p class="mb-0 text-dark">
                                Creating Memories, One Trip at a Time
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="MM_Voucher.aspx">
                            <img src="img/services/thum_img_8.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">MM Voucher
                            </h6>
                            <p class="mb-0 text-dark">
                                Unlock Savings, One Voucher at a Time
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="MovieBookingRedirect.aspx" target="_blank">
                            <img src="img/services/thum_img_4.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Movie Booking
                            </h6>
                            <p class="mb-0 text-dark">
                                Catch the Latest Blockbusters
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="FoodBookingRedirect.aspx" target="_blank">
                            <img src="img/services/thum_img_5.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Food Booking
                            </h6>
                            <p class="mb-0 text-dark">
                                It's time to celebrate with
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="GiftVoucherRedirect.aspx" target="_blank">
                            <img src="img/services/thum_img_6.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Gift Vouchers
                            </h6>
                            <p class="mb-0 text-dark">
                                It's time to celebrate with
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-sm-6 col-12 px-2 mb-3 mmb-2">
                    <div class="card text-center">
                        <a href="#" target="_blank">
                            <img src="img/services/thum_img_7.jpg" alt="banner" class="img-fluid img-responsive w-100">
                        </a>
                        <div class="description mt-3 mb-3">
                            <h6 class="text-uppercase">Upcoming Petro Card
                            </h6>
                            <p class="mb-0 text-dark">
                                Top-Up Your Tank, Top-Up Your Life
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="MMVoucher">
    </div>
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
    <script
        type="text/javascript"
        src="https://d2jyl60qlhb39o.cloudfront.net/integration-plugin.js"
        id="wa-widget"
        widget-id="c56kdZ">
    </script>
</asp:Content>

