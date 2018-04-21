<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucListening.ascx.cs" Inherits="CommonControls_ucListening" %>
<table>
    <tr>
        <td>Media / Audio File (filename):</td>
        <td>
            <asp:TextBox runat="server" ID="txtMedia" />
        </td>
    </tr>
    <tr>
        <td>Delay / Audio delay (seconds):</td>
        <td>
            <asp:TextBox runat="server" ID="txtDelay" Text="0" />
        </td>
    </tr>
    <tr id="trTemplateListenWrite" runat="server" visible="false">
        <td>
            Max Word Count:
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtMaxWordCount"  Text="0" />
        </td>
    </tr>
    <tr>
        <td>
            Answer:
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAnswer" />
        </td>
    </tr>
    
</table>
