<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activate.aspx.cs" Inherits="Activate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel runat="server" ID="pnlPrePaid">
            <table border="0" width="70%" cellpadding="5" cellspacing="5">
                <tr>
                    <td>
                        <asp:Label ID="lblInfo" Text="Payment Code:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" MaxLengt="15" ID="txtPaymentCode" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="valGrpPrePaid" ErrorMessage="Please enter valid payemnt code" 
                        ControlToValidate="txtPaymentCode" Display="Static"
                            runat="server" />
                        
                    </td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:TextBox runat="server" MaxLength="50" ID="txtFirstname" />
                        <asp:RequiredFieldValidator runat="server" ID="reqValFirstname" ValidationGroup="valGrpPrePaid" ErrorMessage="Please enter name"
                        ControlToValidate="txtFirstname" Display="Static"
                        ></asp:RequiredFieldValidator>
                        </td>
                </tr>
                <tr>
                    <td>Lastname:</td>
                    <td>
                        <asp:TextBox runat="server"  MaxLength="50" ID="txtLastname"/>
                        <asp:RequiredFieldValidator ValidationGroup="valGrpPrePaid" ID="reqValLastname" ErrorMessage="Please enter lastname" ControlToValidate="txtLastname"
                            runat="server" Display="Static" />
                    </td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" MaxLength="50" />
                        <asp:RequiredFieldValidator ErrorMessage="Please enter email" ID="reqValEmail" ControlToValidate="txtEmail"
                            runat="server" Display="Dynamic" ValidationGroup="valGrpPrePaid" />
                        <asp:RegularExpressionValidator Display="Dynamic" ID="regExpValEmail"
                         ValidationGroup="valGrpPrePaid" ErrorMessage="Please enter valid email" ControlToValidate="txtEmail"
                            runat="server" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button Text="Validate" runat="server" ID="btnValidate" 
                            ValidationGroup="valGrpPrePaid" onclick="btnValidate_Click1"
                         />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label Text="" ID="lblMessage" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInformation">
            
            <asp:Literal Text="" ID="litMessage" runat="server" />
            <br />
            <br />
            <asp:Button Text="Download License" ID="btnDownloadFile" runat="server" /> 
        </asp:Panel>
    </div>
    </form>
</body>
</html>
