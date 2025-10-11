<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

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
    <section class="login-section">
        <div class="container">
            <div class="row justify-content-center">

                <div class="col-xl-5 col-lg-6 col-md-8">
                    <div class="card shadow border-0 rounded-4 ">
                        <div class="card-body p-4">

                            <h4 class="text-center text-dark mb-1"><b>Login to Your Account</b></h4>
                            <hr class="mb-4">

                            <!-- Email -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">User ID</label>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="fa fa-envelope"></i></span>
                                    <asp:TextBox ID="TxtUserID" runat="server" placeholder="Enter User ID" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Password -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">Password</label>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="fa fa-lock"></i></span>
                                    <asp:TextBox ID="TxtPassword" runat="server" placeholder="Enter Password" class="form-control showeye"
                                        TextMode="Password"></asp:TextBox>

                                    <%--<p style="color: black;">
                                        <input type="checkbox" onclick="myFunction()">
                                        Show Password
                                    </p>--%>
                                </div>
                            </div>

                            <!-- Remember & Forgot -->
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="rememberMe">
                                    <label class="form-check-label" for="rememberMe">Remember Me</label>
                                </div>
                                <a href="#" class="text-decoration-none text-primary small">Forgot Password?</a>
                            </div>

                            <!-- Submit Button -->
                            <div class="d-grid mb-3">
                                 <asp:Button ID="BtnLogin" runat="server" Text="Login" class="btn-danger btn-lg rounded-pill w-100" OnClick="BtnLogin_Click" />
                               <%-- <button type="submit" class="btn-danger btn-lg rounded-pill w-100"><i class="fa-solid fa-right-to-bracket me-2"></i>Login </button>--%>
                            </div>

                            <!-- Register Link -->
                            <p class="text-center small mb-0">
                                Don’t have an account? <a href="Registartion.aspx" class="text-decoration-none fw-bold">Register Now</a>
                            </p>



                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

    
    <!-- About Section End -->
</asp:Content>

