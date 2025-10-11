<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Registartion.aspx.cs" Inherits="Registartion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta charset="utf-8">
    <script type="text/javascript" src="js/jquery.min.js">
    </script>

    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>

    <script type="text/javascript" src="js/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="js/jquery.validationEngine.js"></script>

    <link href="js/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function DivOnOff() {
            if (document.getElementById("<%= chkterms.ClientID %>").checked == true) {
                document.getElementById("DivTerms").style.display = "block";
            }
            else {
                document.getElementById("DivTerms").style.display = "none";
            }
        }
        var jq = $.noConflict();
<%--        function pageLoad(sender, args) {
            jq(document).ready(function () {
                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });
            jq("#<%=BtnProceedToPay.ClientID %>").click(function () {
                var valid = jq("#aspnetForm").validationEngine('validate');
                var vars = jq("#aspnetForm").serialize();
                if (valid == true) {
                    return true;
                }
                else {
                    return false;
                }
            });
        }--%>

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="register-section py-5">
        <div class="container">
            <div class="row justify-content-center">
                <div class="clr">
                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                </div>

                <div class="col-xl-5 col-lg-6 col-md-8">
                    <div class="card shadow border-0 rounded-4">
                        <div class="card-body p-4">

                            <h4 class="text-center text-dark mb-1"><b>New Registration</b></h4>
                            <hr class="mb-4">

                            <div>
                                <!-- Full Name -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Sponsor ID<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="TxtSponsorid" ClientIDMode="Static" class="form-control validate[required,custom[onlyLetterNumber]]" MaxLength="20"
                                            placeholder="Enter Sponsor ID" runat="server" TabIndex="1" OnTextChanged="TxtSponsorid_TextChanged" AutoPostBack="true"></asp:TextBox>

                                        <asp:HiddenField ID="lblrefformno" runat="server" />
                                        <asp:HiddenField ID="HDnRefFormno" runat="server" />
                                        <%--<span class="input-group-text"><i class="fa fa-user"></i></span>
                                        <input type="text" class="form-control" placeholder="Enter your full name" required>--%>
                                    </div>
                                    <asp:Label ID="lblRefralNm" ForeColor="#D11F7B" runat="server"></asp:Label>
                                </div>

                                <div class="form-group greybt" runat="server" id="DivLeg1" style="display: none">
                                    <label class="control-label col-sm-2">
                                        Leg<span class="red">*</span></label>
                                    <div class="col-sm-10">
                                        <asp:RadioButtonList ID="RbtnLegNo" runat="server" TabIndex="3" RepeatDirection="Horizontal"
                                            Style="width: 150px" />
                                    </div>
                                    <label>
                                        Country Name<span class="red">*</span>
                                    </label>
                                    <asp:DropDownList ID="ddlCountryNAme" OnSelectedIndexChanged="ddlCountryNAme_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="ddlMobileNAme" CssClass="form-control "
                                        runat="server" ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>

                                </div>
                                <!-- Email -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Name<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="Txtname" ClientIDMode="Static" class="form-control validate[required,custom[onlyLetterNumberChar]]"
                                            placeholder="Enter Name" runat="server" TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- Mobile -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Father / Husband's Name<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtfather" ClientIDMode="Static" class="form-control validate[required,custom[onlyLetterNumberChar]]"
                                            placeholder="Enter Father / Husband's Name" runat="server" TabIndex="3"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- Password -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Mobile No.<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtmobl" ClientIDMode="Static" class="form-control validate[required,custom[mobile]]"
                                            onkeypress="return isNumberKey(event);" placeholder="Enter Mobile No." runat="server"
                                            TabIndex="4" MaxLength="10" OnTextChanged="txtmobl_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:HiddenField ID="kitamount" runat="server" />
                                    </div>
                                </div>

                                <!-- Confirm Password -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">E-Mail ID. <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtemail" class="form-control validate[required,custom[email]]"
                                            placeholder="Enter Email ID" runat="server" TabIndex="5" OnTextChanged="txtemail_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Password<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                                    <div class="input-group">
                                        <asp:TextBox ID="TxtPasswd" class="validate[required] form-control"
                                            TabIndex="51" runat="server" TextMode="Password" ValidationGroup="eInformation"
                                            autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="exampleInputEmail1 text-black">
                                        Address
                                    </label>
                                    <asp:TextBox ID="TxtAddress" ClientIDMode="Static" class="form-control " placeholder="Enter Address"
                                        runat="server" TabIndex="6"></asp:TextBox>
                                    <asp:HiddenField ID="Address" runat="server" />
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="exampleInputEmail1 text-black">
                                        Pin code
                                    </label>
                                    <asp:TextBox ID="txtPinCode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                        TabIndex="7" runat="server" MaxLength="6" autocomplete="off" placeholder="Enter Pin code"></asp:TextBox>
                                    <asp:HiddenField ID="Pincode" runat="server" />
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="exampleInputEmail1 text-black">
                                        State</label>
                                    <asp:DropDownList ID="ddlStatename" runat="server" CssClass="form-control" TabIndex="8">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="StateCode" runat="server" />
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="exampleInputEmail1 text-black">
                                        District</label>
                                    <asp:TextBox ID="txtDistrict" CssClass="form-control  " TabIndex="9" runat="server"
                                        placeholder="Enter District"></asp:TextBox>
                                    <asp:HiddenField ID="HDistrictCode" runat="server" />
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="exampleInputEmail1 text-black">
                                        City</label>
                                    <asp:TextBox ID="txtTehsil" CssClass="form-control" TabIndex="10" runat="server"
                                        ValidationGroup="eInformation" autocomplete="off" placeholder="Enter City"></asp:TextBox>
                                    <asp:HiddenField ID="HCityCode" runat="server" />
                                </div>
                                <div id="divlogin" runat="server" visible="false">
                                    <h4>Login Information</h4>
                                    <div class="form-group ">
                                        <label class="control-label col-sm-2">
                                            Password<span class="red">*</span></label>
                                        <div class="col-sm-10">
                                            <%--<asp:TextBox ID="TxtPasswd" class="validate[required,minSize[5],maxSize[10]] form-control"
                                            TabIndex="51" runat="server" TextMode="Password" ValidationGroup="eInformation"
                                            autocomplete="off"></asp:TextBox>--%>
                                        </div>
                                    </div>
                                    <div id="Div8" class="form-group" visible="false" runat="server">
                                        <label class="control-label col-sm-2">
                                            Transaction Password<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtTransactionPassword" class="validate[required,minSize[5],maxSize[10]] form-control"
                                            TabIndex="52" runat="server" TextMode="Password" ValidationGroup="eInformation"
                                            autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="mb-3">

                                    <div class="input-group">
                                        <textarea name="ctl00$ContentPlaceHolder1$TCAllCompany" id="ctl00_ContentPlaceHolder1_TCAllCompany"
                                            style="font-size: 13px; width: 100%; height: 80px; text-align: left;" cols="5"
                                            rows="10" readonly="readonly">Terms &amp; Conditions :-
- It is kind advise to you that you promote Business as per actual. Company will not responsible for your miss commitments in the market through any manner.
- Registration is FREE in our system.
- Company provides you online account as your ID with password. It contains all your legal information , Transaction Balance, Team detail, Bonus details etc.
- Year starts from 1st April every year.
- KYC Documents is mandatory.
 You must sign your application form &amp; submitted in nearest company Branch/office along with one colour passport size photo &amp; copy of self attested ID proof or address proof / PAN No.
                 </textarea>
                                    </div>
                                </div>


                                <!-- Terms -->
                                <div class="form-check mb-3">
                                    <asp:CheckBox ID="chkterms" runat="server" onclick="DivOnOff();" TabIndex="11" class="form-check-input" />
                                    <label class="form-check-label" for="terms">
                                        I agree to the <a href="#" class="text-primary text-decoration-none">Terms & Conditions</a>
                                    </label>
                                    <%--<input class="form-check-input" type="checkbox" id="terms" required>
                                    <label class="form-check-label" for="terms">
                                        I agree to the <a href="#" class="text-primary text-decoration-none">Terms & Conditions</a>
                                    </label>--%>
                                </div>

                                <!-- Submit Button -->
                                <div class="d-grid mb-3">
                                    <asp:Button ID="BtnProceedToPay" runat="server" Text="Submit" class="btn-danger btn-lg rounded-pill w-100" OnClick="BtnProceedToPay_Click" />
                                    <%-- <button type="submit" class="btn-danger btn-lg rounded-pill w-100"><i class="fa fa-user-plus me-2"></i>Register </button>--%>
                                </div>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                    ShowSummary="False" ValidationGroup="eSponsor" />
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                                    ShowSummary="False" ValidationGroup="eInformation" />
                                <!-- Login Link -->
                                <p class="text-center small mb-0">
                                    Already have an account? <a href="login.aspx" class="text-decoration-none fw-bold">Login Here</a>
                                </p>

                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>


</asp:Content>

