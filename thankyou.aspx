<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="thankyou.aspx.cs" Inherits="thankyou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="login-section">
        <div class="container">
            <div class="row justify-content-center">

                <div class="col-xl-5 col-lg-6 col-md-8">
                    <div class="card shadow border-0 rounded-4 ">
                        <div class="card-body p-4">

                            <h4 class="text-center text-dark mb-1"><b>Thank You for your Registration</b></h4>
                            <hr class="mb-4">

                            <!-- Email -->
                            <div class="mb-3">
                                <hr>
                                <h4 class="text-center">USER NAME : <span class="themetextclr">
                                    <%=Session["idno"]%></span></h4>
                                <h4 class="text-center">PASSWORD : <span class="themetextclr">
                                    <%=Session["passw"] %></span></h4>
                                <hr>
                            </div>

                            <!-- Register Link -->
                            <p class="text-center small mb-0">
                                Already have an account? <a href="login.aspx" class="text-decoration-none fw-bold">Login Now</a>
                            </p>



                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>







</asp:Content>

