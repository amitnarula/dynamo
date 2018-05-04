<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucBlank.ascx.cs" Inherits="CommonControls_ucBlank" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            Select Blank
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlBlankGeneral">
                <asp:ListItem Text="Blank1" />
                <asp:ListItem Text="Blank2" />
                <asp:ListItem Text="Blank3" />
                <asp:ListItem Text="Blank4" />
                <asp:ListItem Text="Blank5" />
                <asp:ListItem Text="Blank6" />
                <asp:ListItem Text="Blank7" />
                <asp:ListItem Text="Blank8" />
                <asp:ListItem Text="Blank9" />
                <asp:ListItem Text="Blank10" />
            </asp:DropDownList>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtOptionText" />
        </td>
        <td>
            <asp:Button Text="Add" ID="btnAdd" runat="server" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
<asp:Panel runat="server" ID="pnlRenderOptions" >
    <table border="1" cellpadding="0" cellspacing="0" >
        <tr>
            <th>
                Blank Name
            </th>
            <th>
                Options
            </th>
            <th>
                Correct Option Index
            </th>
            <td>
                
                <asp:Repeater ID="rptBlank" runat="server" OnItemDataBound="rptBlank_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal Text='<%#Eval("Key") %>' runat="server" ID="litBlankName"></asp:Literal>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    
                                
                                    <asp:Repeater runat="server" ID="rptOptions">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <td>
                                                        <asp:Literal ID="litID" Text='<%#Eval("ID") %>' runat="server" />
                                                        <asp:Literal ID="litOptions" Text='<%#Eval("OptionText") %>'
                                                     runat="server" />
                                                    </td>
                                                    
                                                </td>
                                                
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCorrectOption" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                
            </td>
        </tr>
    </table>
</asp:Panel>
<br />
<table>
    <tr>
        <td>
            Create the exact replica(s) of the Blank1

        </td>

    </tr>
    <tr>
        <td>Number of Copies:</td>
        <td><asp:TextBox runat="server" ID="txtNumberOfCopies" /></td>
        <td><asp:Button ID="btnGenerateCopies" Text="Generate Copies" runat="server" OnClick="btnGenerateCopies_Click"  /></td>
    </tr>
</table>

<br />
<asp:Button Text="Generate Correct Answers" ID="btnGenerateCorrectAnswers" OnClick="btnGenerateAnswers_Click" runat="server" />
<br />
