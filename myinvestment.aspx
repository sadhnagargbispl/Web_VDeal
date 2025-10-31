<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="myinvestment.aspx.cs" Inherits="myinvestment" %>


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
                        <h4 class="text-dark">My Purchase Detail</h> 
            <hr class="m-2">
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gv" runat="server" CssClass="table datatable" AutoGenerateColumns="true"
                                            ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                    </div>
                </div>
            </div>



        </div>
    </div>



</asp:Content>

