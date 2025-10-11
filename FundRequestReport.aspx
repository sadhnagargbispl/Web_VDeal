<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="FundRequestReport.aspx.cs" Inherits="FundRequestReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table td, .table th {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="product-details-area pt-30 pb-20 bg-white">
        <div class="container">
            <div class="row align-items-center mt-4">



                <!-- Product Info -->
                <div class="col-xl-12 col-lg-12">
                    <div class="product-info">
                        <h4 class="text-dark">Fund Request Report</h> 
            <hr class="m-2">
                            <div class="table-responsive">
                                <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="false" RowStyle-Height="2px"
                                    GridLines="Both" AllowPaging="true" CssClass="table table-bordered table-responsive-md"
                                    AllowSorting="False" ShowHeader="true" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="RptDirects_PageIndexChanging">

                                    <Columns>
                                        <asp:TemplateField HeaderText="ReqNo">
                                            <ItemTemplate>
                                                <%#Eval("ReqNo")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request Date">
                                            <ItemTemplate>
                                                <%#Eval("ReqDate") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Mode">
                                            <ItemTemplate>
                                                <%#Eval("PayMode") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque/ TransactionNo">
                                            <ItemTemplate>
                                                <%# Eval("ChqNo") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque/ Transaction Date">
                                            <ItemTemplate>
                                                <%#Eval("ChequeDate") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bank Name">
                                            <ItemTemplate>
                                                <%# Eval("BankName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch Name">
                                            <ItemTemplate>
                                                <%# Eval("BranchName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <%# Eval("Amount") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <%# Eval("Remarks") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <%# Eval("Status") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image">
                                            <ItemTemplate>
                                                <a href='<%# "Img.aspx?type=Payment&ID=" + Eval("Reqno") %>'
                                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe', width: 585, height: 380, marginTop: 0 })">
                                                    <asp:Image ID="Image1" runat="server"
                                                        ImageUrl='<%# Eval("ScannedFile") %>'
                                                        Height="100px"
                                                        Width="100px"
                                                        Visible='<%# Convert.ToBoolean(Eval("ScannedFileStatus")) %>' />
                                                </a>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                    </div>
                </div>
            </div>



        </div>
    </div>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

<script src="plugins/datatables/dataTables.bootstrap.min.js"></script>

<script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>

</asp:Content>

