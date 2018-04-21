<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucWriting.ascx.cs" Inherits="CommonControls_ucWriting" %>
<table>
    <tr>
        <td>Max Word Count:</td>
        <td>
            <asp:TextBox runat="server" ID="txtMaxWordCount" />  
            
        </td>
        
    </tr>
    <tr>
        <td valign="top">
            Correct Answer:
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtCorrectAnswer" TextMode="MultiLine" Height="400" Width="700" />
        </td>
    </tr>
</table>
