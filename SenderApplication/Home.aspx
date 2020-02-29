<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SenderApplication.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .DropDown {
            border-radius: 4px
        }

    </style>
    <div style="margin-top: 5%; display: flex">
        Select Table &nbsp;
        <asp:DropDownList CssClass="DropDown" ID="ddlTable" ClientIDMode="Static" runat="server"></asp:DropDownList>
        <asp:XmlDataSource ID="xmlTableList" runat="server" DataFile="~/TableList.xml"></asp:XmlDataSource>
        <div style="margin-left: 6px">
            <asp:Button Text="Audit Data" runat="server" style="border-radius: 5px" ID="btnAudit" OnClick="BtnAudit_Click" />
        </div>
    </div>
    <div>
        <input type="text" name="name" value="" />
        <asp:Button Text="Search" runat="server" OnClick="Unnamed_Click" />
    </div>
</asp:Content>
