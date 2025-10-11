<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="GiftWallet.aspx.cs" Inherits="GiftWallet" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
    .table td, .table th {
        font-size: 11px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container pt-5 pb-5">
        <div class="row justify-content-center">
            <div class="col-lg-12 col-sm-6 col-12">
                <section class="login-card-section">
                    <div class="form-container">
                        <p class="title">Wallet Report</p>
                        <div class="basic-form">
                            <div class="row">
                                <div class="mb-3 col-md-3">
                                    <label class="form-label">
                                        Wallet Type:</label>
                                    <asp:DropDownList ID="Rbtnwallet" class="form-control" TabIndex="2" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="mb-3 col-md-3" style="padding: 2%">
                                    <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" class="btn-danger btn-lg rounded-pill" OnClick="BtnSubmit_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <div class="row">
                                <div class="col-lg-4">
                                    <table id="ctl00_cphData_tbUserDetails" class="table table-bordered table-responsive-md"
                                        cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td>Deposit
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="MCredit" runat="server">0.00
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Used
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="MDebit" runat="server">0.00
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Balance
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="MBal" runat="server">0.00
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-lg-12">
                                    <div class="table-responsive">
                                        <%--          <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-responsive-md"
        AllowPaging="true" PageSize="10" OnPageIndexChanging="RptDirects_PageIndexChanging" >--%>
                                        <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="True" RowStyle-Height="25px"
                                            GridLines="Both" AllowPaging="true" CssClass="table table-bordered table-responsive-md"
                                            AllowSorting="False" ShowHeader="true" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="RptDirects_PageIndexChanging">
                                            <Columns>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>


