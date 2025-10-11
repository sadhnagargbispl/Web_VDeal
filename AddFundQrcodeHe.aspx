<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="AddFundQrcodeHe.aspx.cs" Inherits="AddFundQrcodeHe" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function copyText() {
            debugger;
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_SpnAddress");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }
            try {
                document.execCommand('copy');
                alert('Address Copied.!');
            }
            catch (err) {
                alert('Unable To Copy Link');
            }
            return false;
        }
        function copyText1() {
            debugger;
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_TokenNoOF");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }
            try {
                document.execCommand('copy');
                alert('No.Of Token Copied.!');
            }
            catch (err) {
                alert('Unable To Copy Link');
            }
            return false;
        }

        function copyText2() {
            debugger;
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_BEPUSDTAddress");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }
            try {
                document.execCommand('copy');
                alert('Address Copied.!');
            }
            catch (err) {
                alert('Unable To Copy Link');
            }
            return false;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style>
        .buttoncss1 {
            color: #fff;
            display: inline-block;
            font-size: 14px;
            font-weight: 500;
            line-height: 1;
            padding: 15px 8px;
            position: relative;
            text-transform: capitalize;
            transform: perspective(1px) translateZ(0px);
            transition: .3s;
            vertical-align: middle;
            background: #8427e1;
            border-radius: 5px !important;
            border: none;
            border-radius: 10px 10px 10px 10px;
            width: 85px;
        }

        @media only screen and (max-width:767px) {
            .qrcode p {
                width: 200px !important;
                text-overflow: ellipsis !important;
                white-space: nowrap !important;
                overflow: hidden !important;
            }

            .packagebox {
                padding: 1rem !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-center">
                    <%--<h4 class="text-white">Add Fund (Deposit Only MMID)
                    </h4>--%>
                    <hr>
                </div>
                <div class="col-lg-6 col-sm-6 col-12 px-2 mb-3 mmb-2" runat="server" id="DivLoginCheck">
                    <div class="packagebox p-5">
                        <div runat="server" id="DivQrCodeAmount" visible="true">
                            <div class="form-group" runat="server" visible="true" id="divclicko">
                                <h6 style="color: #5c13b1 !important;">Click Option :</h6>
                                <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" CssClass="form-control"
                                    RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="RbtStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="&nbsp;&nbsp;By Other" Value="C" style="padding: 10px 10px;"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;&nbsp;By Bank" Value="B" Selected="True" style="padding: 10px 10px;"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div runat="server" id="divByBank" visible="false">
                                <div class="form-group">
                                    <label for="inputdefault">
                                        Enter Amount<span class="red">*</span></label>
                                    <asp:TextBox runat="server" onkeypress="return isNumberKey(event);" MaxLength="8"
                                        TabIndex="1" ID="TxtAmount" class="form-control validate[required]" Text="0"></asp:TextBox>
                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                </div>
                                <div class="form-group">
                                    <label for="inputdefault">
                                        Select Paymode<span class="red">*</span></label>
                                    <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control"
                                        TabIndex="2" OnSelectedIndexChanged="DdlPaymode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" id="divDDno" runat="server" visible="false">
                                    <label for="inputdefault">
                                        <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="TxtDDNo" onkeypress="return isNumberKey(event);" class="form-control validate[required]"
                                        TabIndex="3" runat="server" MaxLength="16" AutoPostBack="true"></asp:TextBox>
                                </div>
                                <div class="form-group" id="divDDDate" runat="server">
                                    <label for="inputdefault">
                                        <asp:Label ID="LblDDDate" runat="server" Text="Transaction Date *"></asp:Label>
                                    </label>
                                    <div class="form-group">
                                        <asp:TextBox ID="TxtDDDate" runat="server" class="form-control validate[required]"
                                            TabIndex="4"></asp:TextBox>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <div class="form-group">
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgDatePicker"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtDDDate"
                                            ErrorMessage="Invalid Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>

                                        <div class="clearfix">
                                        </div>
                                        <style>
                                            .imgdatewalletrequest {
                                                position: absolute;
                                                margin-top: -70px;
                                                left: 85%;
                                            }

                                            @media only screen and (max-width:768) {
                                                .imgdatewalletrequest {
                                                    left: 82%;
                                                    margin-top: -48px;
                                                }
                                            }
                                        </style>
                                        <asp:ImageButton ID="imgDatePicker" runat="Server" AlternateText="Click to show calendar"
                                            ImageAlign="Middle" ImageUrl="img/QrCodeImg/calender.jpg" Width="25px" class="imgdatewalletrequest" />
                                    </div>
                                    <div class="form-group" id="divImage" runat="server">
                                        <label for="inputdefault">
                                            Scanned Copy:</label>
                                        <asp:FileUpload runat="server" ID="FlDoc" class="form-control" TabIndex="7" />
                                        <asp:Label ID="LblImage" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <asp:Button ID="BtnSaveDB" runat="server" Text="Confirmed" class="btn btn-primary buttoncss text-white w-100" OnClick="BtnSaveDB_Click" />
                                <br />

                                <!-- Genex Business -->
                                <div id="Div1" class="clearfix gen-profile-box">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <center>
                                            <h6 class="text-white">Scan the QR code using your UPI apps
                                            </h6>
                                        </center>
                                    </div>
                                    <div class="col-md-12">

                                        <center>
                                            <img src="img/QrCodeImg/qrcode.jpg" width="40%"></center>




                                    </div>
                                    <div class="col-md-12">
                                        <div id="DivVerify" runat="server">
                                            <br />
                                            <center>
                                                <asp:Label ID="LblVerification" Text="ACCOUNT HOLDER NAME :  " runat="server" Style="font-size: 0.7em; font-weight: bold;"></asp:Label>
                                                <asp:Label ID="lblverstatus" runat="server" Text="MY MEGA MART" Style="font-size: 0.9em;"></asp:Label>
                                                <br />
                                                <asp:Label ID="VerifyDate" runat="server" Text="ACCOUNT NO : " Style="font-size: 0.7em; font-weight: bold;"></asp:Label>
                                                <asp:Label ID="Lblverdate" runat="server" Text="7778888826" Style="font-size: 0.9em;"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfRemark" Text="IFSC CODE : " runat="server" Style="font-size: 0.7em; font-weight: bold;"></asp:Label>
                                                <asp:Label ID="LblRemark" runat="server" Text="KKBK0000877" Style="font-size: 0.9em;"></asp:Label>
                                            </center>

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div runat="server" id="divByCrpto" visible="true">
                                <div class="form-group">
                                    <h6 style="color: #5c13b1 !important;">Check Option :</h6>
                                    <asp:RadioButtonList ID="RbtCheckOptin" runat="server" RepeatDirection="Horizontal" CssClass="form-control"
                                        RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="RbtCheckOptin_SelectedIndexChanged">
                                        <asp:ListItem Text="&nbsp;&nbsp;SUMMIT-SOL TOKEN" Value="M" Selected="True" style="padding: 10px 10px;" OnClientClick="callJavaScriptFunction(); return true;"></asp:ListItem>
                                        <asp:ListItem Text="&nbsp;&nbsp;USDT-BEP20" Value="B" style="padding: 10px 10px;"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div runat="server" id="DIVBEP" visible="false">
                                    <div class="img-responsive" align="center">
                                        <h5 class="text-white">Add Fund (Deposit Only USDT-BEP20)
                                        </h5>
                                        <img id="BepUsdtImg" src="" runat="server" alt="Image Alternative text" class="img-thumbnail" />
                                    </div>
                                    <div class="qrcode">
                                        <div class="mt-2" align="center">
                                            <b>
                                                <p id="BEPUSDTAddress" runat="server" style="color: Black; font-size: 0.8em; font-weight: bold;"></p>
                                            </b>

                                        </div>
                                        <div class="d-flex align-content-center gx-2">
                                            <a class="float-right" href="#" onclick="return copyText2();" style="margin-left: 10px;">
                                                <asp:Button ID="Button2" class="btn btn-primary buttoncss1 text-white  w-100" runat="server" Text="Copy"
                                                    Style="float: none !important; background-color: #eb7c2a; padding: 12px 25px;" />
                                            </a>

                                            <asp:Button ID="BtnDepositClickBEP" class="btn btn-primary buttoncss1 text-white w-100" runat="server" Text="After Deposit Click Here"
                                                Style="float: none !important; background-color: #eb7c2a; margin-left: 5px; padding: 12px 25px;" OnClick="BtnDepositClickBEP_Click" />
                                        </div>


                                    </div>


                                    <div class="d-block mt-3" align="center">
                                        <p style="font-size: 0.8em; color: red">
                                            <b>After Deposit click on the "After Deposit Click Here" button. AMOUNT will be deposit
in your account.</b>
                                        </p>
                                        <p style="font-size: 0.8em; color: red">
                                            <b>Kindly Click Button after 60 Seconds of Payment confirmation on blockchain.</b>
                                        </p>
                                    </div>
                                </div>


                                <div runat="server" id="DivMMIT" visible="true">
                                    <div class="form-group" style="display: none;">
                                        <h6 style="color: #5c13b1 !important;">Enter Amount in INR</h6>
                                        <asp:TextBox ID="TxtReqAmount" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="Enter Amount in INR" AutoPostBack="true"
                                            onkeypress="return isNumberKey(event);" OnTextChanged="TxtReqAmount_TextChanged"></asp:TextBox>
                                        <asp:HiddenField ID="HdnRate" runat="server" />
                                    </div>
                                    <div class="form-group" runat="server" visible="false">
                                        <h6 style="color: #5c13b1 !important;">INR</h6>
                                        <asp:TextBox ID="TxtINRValue" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="INR" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="form-group" style="display: none;">
                                        <h6 style="color: #5c13b1 !important;">No.Of Token</h6>
                                        <asp:TextBox ID="TxtTokenAmount" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="No.Of Token" ReadOnly="true"></asp:TextBox>
                                        <asp:Label ID="lvlnote" runat="server" Text="Note: The Final Deposit Amount May Very Due To Live Rate." Style="font-size: 11px;" ForeColor="Red"></asp:Label>
                                    </div>
                                    <asp:Button ID="BtnconfirmBtotton" runat="server" Text="Confirmed" class="btn btn-primary buttoncss text-white w-100" OnClick="BtnconfirmBtotton_Click" Visible="false" />
                                </div>

                            </div>
                        </div>
                        <div runat="server" id="DivQr" visible="false">
                            <div class="img-responsive" align="center">
                                <h5 class="text-white">Add Fund (Deposit Only SUMMIT-SOL)
                                </h5>
                                <img id="ImgQrCode" src="" runat="server" alt="Image Alternative text" class="img-thumbnail" style="height: 250px; width: auto;" />
                                <%--<img id="qrCodeImage" src="<%= QRCodeBase64 %>" alt="Image Alternative text" class="img-thumbnail" />--%>
                            </div>
                            <div class="qrcode">
                                <div class="mt-2" align="center">
                                    <b>
                                        <p id="SpnAddress" runat="server" style="color: Black; font-size: 0.8em; font-weight: bold;"></p>
                                    </b>

                                </div>
                                <div class="d-flex align-content-center  justify-content-center  gx-2 mt-2">
                                    <a class="float-right" href="#" onclick="return copyText();" style="margin-left: 10px;">
                                        <asp:Button ID="Button1" class="btn btn-primary buttoncss1 text-white  w-100" runat="server" Text="Copy"
                                            Style="float: none !important; background-color: #eb7c2a; padding: 12px 25px;" />
                                    </a>

                                    <asp:Button ID="btnRetry" class="btn btn-primary buttoncss1 text-white w-100" runat="server" Text="After Deposit Click Here"
                                        Style="float: none !important; background-color: #eb7c2a; margin-left: 5px; padding: 12px 25px;" OnClick="btnRetry_Click" Visible="false" />
                                </div>


                            </div>
                            <div class="d-flex align-content-center  justify-content-center  gx-2 mt-2" style="display: none">
                                <b>
                                    <p style="color: Black; font-size: 0.8em; font-weight: bold; display: none;">
                                        Transfer Token : &nbsp;  
                                        <span id="TokenNoOF" runat="server" style="color: Black; font-weight: bold;"></span>
                                    </p>

                                </b>
                                <a class="float-right" href="#" onclick="return copyText1();" style="margin-left: 10px; display: none;">
                                    <i class="fa fa-copy text-orange" style="font-size: 1.5em;"></i>
                                </a>
                            </div>

                            <div class="d-block mt-3" align="center">
                                <p style="font-size: 1em; color: red">
                                    <b class="blink" style="opacity: 0.618249;">waiting for user payment initiation</b>
                                </p>
                                <p style="font-size: 0.8em; color: red; display: none;">
                                    <b>Kindly Click Button after 60 Seconds of Payment confirmation on blockchain.</b>
                                </p>
                            </div>
                        </div>
                        <div style="display: none">
                            <asp:HiddenField ID="hdnvalueCoin" runat="server" />
                            <asp:HiddenField ID="hdnStatus" runat="server" />
                            <asp:HiddenField ID="hdntxnId" runat="server" />
                            <asp:HiddenField ID="hdntxnout" runat="server" />
                            <asp:HiddenField ID="hdnMessage" runat="server" />
                            <asp:HiddenField ID="hdnPaymentId" runat="server" />
                            <asp:HiddenField ID="OrderId" runat="server" />
                            <asp:HiddenField ID="fromidno" runat="server" />
                            <asp:HiddenField ID="privatekey" runat="server" />
                            <asp:HiddenField ID="hdnresponse" runat="server" />
                            <asp:HiddenField ID="amountreq" runat="server" />
                            <asp:HiddenField ID="hdnPayID" runat="server" />
                            <asp:HiddenField ID="hdncallbckurl" runat="server" />
                            <asp:HiddenField ID="HdnFormno" runat="server" />
                            <asp:HiddenField ID="HdnUrl" runat="server" />
                            <asp:HiddenField ID="HdnAmountApi" runat="server" />
                            <asp:HiddenField ID="bepprivatekey" runat="server" />
                            <asp:HiddenField ID="BEPOrderId" runat="server" />
                            <asp:HiddenField ID="BEPhdnPaymentId" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script type="text/javascript">
        function blink_text() {
            $('.blink').fadeOut(500);
            $('.blink').fadeIn(500);
        }
        setInterval(blink_text, 1000);



    </script>

    <script type="text/javascript">

        $('.ca_copy').on('click', function () {
            debugger;
            copyToClipboard($(this).attr('data-tocopy'));
            let tip = $(this).find('.ca_tooltip.tip');
            let success = $(this).find('.ca_tooltip.success');

            success.show();
            tip.hide();

            setTimeout(function () {
                success.hide();
                tip.show();
            }, 5000);
        })
        function copyToClipboard(text) {
            if (window.clipboardData && window.clipboardData.setData) {
                return clipboardData.setData("Text", text);

            } else if (document.queryCommandSupported && document.queryCommandSupported("copy")) {
                var textarea = document.createElement("textarea");
                textarea.textContent = text;
                textarea.style.position = "fixed";
                document.body.appendChild(textarea);
                textarea.select();
                try {
                    return document.execCommand("copy");
                } catch (ex) {
                    console.warn("Copy to clipboard failed.", ex);
                    return false;
                } finally {
                    document.body.removeChild(textarea);
                }
            }
        }
    </script>
    <script type="text/javascript">
        function callJavaScriptFunction() {
            debugger;
            var ID = $("#ContentPlaceHolder1_hdnPaymentId").val(); // Payment ID
            var idno = $("#ContentPlaceHolder1_hdnPayID").val(); // ID number
            var ajax_url = 'handler/QrCodehenr.ashx?payment_id=' + ID + '&idno=' + idno; // Generate ajax URL

            // Call the check_status function with the generated ajax_url
            check_status(ajax_url);
        }

        function check_status(ajax_url) {
            debugger;
            let is_paid = false;
            status_loop(is_paid, ajax_url);
        }

        function status_loop(is_paid, ajax_url) {
            debugger;
            var s = "pointWallet.aspx";
            var ID = $("#ContentPlaceHolder1_hdnPaymentId").val(); // Payment ID
            var idno = $("#ContentPlaceHolder1_hdnPayID").val(); // ID number
            if (is_paid) return;

            $.ajax({
                type: "GET",
                url: "QrCodehenr.ashx?payment_id=" + ID + "&idno=" + idno + "",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var d = JSON.parse(data);
                    // alert("Payment Successfully Added in Wallet!");
                    debugger;
                    if (d.Response == "Success") {
                        $("#dtext").html(d.message);
                        alert("Payment Successfully Added in Wallet!");
                        is_paid = true;
                        window.location.href = s;
                    } else {
                        $("#dtext").html(data.message);
                    }
                },
                complete: function (data) {
                    setTimeout(function () {
                        status_loop(is_paid, ajax_url); // Keep checking every 5 seconds
                    }, 5000);
                }
            });
        }
    </script>

    <%--<script type="text/javascript">
        function callJavaScriptFunction() {
            if (typeof ajax_url !== "undefined") {
                check_status(ajax_url); // Call the check_status function with the ajax_url
            } else {
                alert("ajax_url is not defined.");
            }
        }
    </script>

    <script type="text/javascript">

        function check_status(ajax_url) {
            debugger;

            let is_paid = false;
            status_loop(is_paid);

        }


        function status_loop(is_paid) {
            debugger;

            //alert("1");
            var s = "pointWallet.aspx";
            var ID = $("#ContentPlaceHolder1_hdnPaymentId").val(); //$("#hdnPaymentId").val();
            var idno = $("#ContentPlaceHolder1_hdnPayID").val(); //$("#hdnPaymentId").val();
            if (is_paid) return;
            $.ajax({
                type: "GET",
                url: "QrCodehenr.ashx?payment_id=" + ID + "&idno=" + idno + "",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    // alert(data.str);
                    var d = JSON.parse(data);
                    // alert(d.response);
                    if (d.response == "Success") {
                        $("#dtext").html(d.message);
                        alert("Payment Successfully Added in Wallet.!");
                        is_paid = true;
                        window.location.href = s;
                    }
                    else {
                        //alert("No transaction found. Please try again later.!");
                        //is_paid = true;
                        $("#dtext").html(data.message);
                    }
                },
                complete: function (data) {
                    setTimeout(status_loop, 5000);
                }
            });
        }


    </script>
    <script id="ca-payment-js-after">
        var ID = $("#ContentPlaceHolder1_hdnPaymentId").val(); //$("#hdnPaymentId").val();
        var idno = $("#ContentPlaceHolder1_hdnPayID").val(); //$("#hdnPaymentId").val();
        jQuery(function () { let ajax_url = 'handler/QrCodehenr.ashx?payment_id=' + ID + 'idno=' + idno; setTimeout(function () { check_status(ajax_url) }, 500) })
    </script>--%>
</asp:Content>
