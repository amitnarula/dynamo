<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucQuestionCommon.ascx.cs" Inherits="CommonControls_ucQuestionCommon" %>

<table>
    <tr>
        <td>Enter Question Title:</td>
        <td><asp:TextBox ID="txtTitle" runat="server" TextMode="MultiLine" Width="700" /></td>
    </tr>
    <tr>
        <td>Enter Question Instruction:</td>
        <td><asp:TextBox ID="txtInstruction" runat="server" TextMode="MultiLine" Width="700" /></td>
    </tr>
    <tr>
        <td>Question Max. Attempt Time (mm:ss):</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlAttemptTimeMinutes">
                
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlAttemptTimeSeconds">
                
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Question Timer Delay Time(mm:ss):</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlDelayTimeMinutes">
                
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlDelayTimeSeconds">
                
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td valign="top">Enter Question Description:</td>
        <td>
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="700" Height="500" />    
        </td>
    </tr>
</table>
