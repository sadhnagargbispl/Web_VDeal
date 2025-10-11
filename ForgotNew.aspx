<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ForgotNew.aspx.cs" Inherits="ForgotNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row justify-content-center mt-5">
                <div class="col-lg-4 col-sm-12 col-12 px-2 mb-3 mmb-2">
                    <div class="text-title text-center">
                        <h4 class="text-white">Forgot Your Password</h4>
                        <p class="text-white">
                            Enter your email address and we'll send you a link to reset your password.
                        </p>
                    </div>
                    <div class="packagebox p-5">
                        <div class="centered">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>
                                        Member Id</label>
                                    <asp:TextBox ID="txtIDNo" runat="server" class="form-control" MaxLength="15"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireIDNo" runat="server" ControlToValidate="txtIDNo"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Email id<br />
                                    </label>
                                    <asp:TextBox ID="TxtMobileNo" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireMoblNo" runat="server" ControlToValidate="TxtMobileNo"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Button ID="Submit" runat="server" Text="Submit" CssClass="btn btn-primary buttoncss text-white w-100"  OnClick ="Submit_Click" />
                                    <asp:HiddenField ID="hdnSms" runat="server" />
                                </div>
                            </div>
                        </div>
                        <p class="text-center text-dark">
                            Not a Member ? <b><a href="registartion.aspx" class="text-dark"><u>Signup Now </u></a>
                            </b>
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;
        </p>
    </div>
</asp:Content>

