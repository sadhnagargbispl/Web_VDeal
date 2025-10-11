<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PointWallet.aspx.cs" Inherits="PointWallet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about-us-area pt-30 pb-20" style="background-color: #ad72dd;">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-12 col-sm-6 col-12 px-2 mb-3 mmb-2 text-center">
                    <h4 class="text-white">Point Wallet
                    </h4>
                    <hr>
                </div>
                <div class="col-lg-5 col-sm-6 col-8 px-2 mb-3 mmb-2">
                    <div class="packagebox p-3">
                        <ul>
                            <li>Deposit :
                                <asp:Label ID="LblDeposit" runat="server" Text="0.00"></asp:Label></li>
                            <li>Used :
                                <asp:Label ID="LblUsed" runat="server" Text="0.00"></asp:Label></li>
                            <li>Balance :
                                <asp:Label ID="LblBalance" runat="server" Text="0.00"></asp:Label></li>
                        </ul>
                    </div>
                </div>
                <div class="col-lg-4 col-sm-6 col-4 px-2 mb-3 mmb-2">
                    <a href="AddFundPayment.aspx" class="btn btn-primary btn-theme-edit p-3">ADD FUND</a>
                </div>
                <div class="col-lg-12 col-sm-12 col-12 px-2 mb-3 mmb-2">
                    <div class="packagebox">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="True" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" CssClass="table table-bordered table-striped"
                                        AllowSorting="False" ShowHeader="true" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="RptDirects_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

