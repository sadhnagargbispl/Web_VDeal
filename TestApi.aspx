<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestApi.aspx.cs" Inherits="TestApi" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QR Code Generator</title>
    <script type="text/javascript">
        function copyText() {

            var range, selection, worked;
            var element = document.getElementById("SpnAddress");
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
                alert('address copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Generated QR Code for CCPayment Address</h2>
            <img id="qrCodeImage" src="" runat="server" alt="QR Code for Address" style="height: 200px; width: auto;" />
            <div class="qrcode">
                <p id="SpnAddress" runat="server"></p>
            </div>
            <asp:Button ID="btncopy" CssClass="btn btn-primary" OnClientClick="return copyText();"
                runat="server" Text="Copy" /><br />
            <br />
            <b class="text-capitalize text-success"><span class="blink" style="opacity: 0.618249;">waiting for user payment initiation</span></b>
            <div style="display: none">
                <asp:HiddenField ID="hdnvalueCoin" runat="server" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hdnStatus" runat="server" />
                <asp:HiddenField ID="hdntxnId" runat="server" />
                <asp:HiddenField ID="hdntxnout" runat="server" />
                <asp:HiddenField ID="hdnMessage" runat="server" />
                <asp:Label ID="hdnPaymentId" runat="server" />
                <asp:HiddenField ID="hdnresponse" runat="server" />
                <asp:HiddenField ID="hdnPayID" runat="server" />
                <asp:HiddenField ID="hdncallbckurl" runat="server" />
                <asp:HiddenField ID="HdnUrl" runat="server" />

                <asp:HiddenField ID="HdnFormno" runat="server" />

                <asp:HiddenField ID="HdnEmail" runat="server" />
            </div>
        </div>
    </form>
</body>
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
        debugger;
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
    function check_status(ajax_url) {
        debugger;

        let is_paid = false;
        status_loop(is_paid);

    }


    function status_loop(is_paid) {
        debugger;

        //alert("1");
        var s = $("#" + '<%= HdnUrl.ClientID %>').val();
        var ID = $("#hdnPaymentId").text(); //$("#hdnPaymentId").val();
        var idno = $("#hdnPayID").val(); //$("#hdnPaymentId").val();
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
                //alert(d.response);
                //                 if(d.Status=="success")
                //                 {
                if (d.response == "Success") {
                    $("#dtext").html(d.message);
                    alert("Payment Successfully Added in Wallet");
                    is_paid = true;
                    window.location.href = s;


                }
                else {
                    $("#dtext").html(data.message);
                }

                //}
            },
            complete: function (data) {
                setTimeout(status_loop, 5000);
            }
        });
    }


</script>
<script id="ca-payment-js-after">
    var ID = $("#hdnPaymentId").val();
    var idno = $("#hdnPayID").val(); //$("#hdnPaymentId").val();
    jQuery(function () { let ajax_url = 'handler/QrCodehenr.ashx?payment_id=' + ID + 'idno=' + idno; setTimeout(function () { check_status(ajax_url) }, 500) })
</script>
</html>
