<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ActivationThankYou.aspx.cs" Inherits="ActivationThankYou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row align-items-left">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-left thankyou">
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
</asp:Content>

