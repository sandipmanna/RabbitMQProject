<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SenderApplication.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <div style="margin-top: 5%; display: flex">
        <div style="margin-left: 6px">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMessage" Style="border-radius: 5px" />
                        </td>
                        <td style="padding-left: 10px">
                            <asp:Button Text="Send Message" runat="server" Style="border-radius: 5px" ID="btnMessage" OnClick="btnMessage_Click" OnClientClick="return ValidateMessage();" CssClass="btn btn-primary" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-top: 10px">
                            <asp:Button Text="Fetch Record(s)" runat="server" ClientIDMode="Static" ID="btnFetchData" OnClick="btnFetchData_Click" CssClass="btn btn-primary" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div>
    </div>
    <script>
        function ValidateMessage() {
            var Message = $('#txtMessage').val();
            if (Message == "") {
                alert("Provide Message !!");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
