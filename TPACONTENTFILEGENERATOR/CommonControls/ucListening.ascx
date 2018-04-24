<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucListening.ascx.cs" Inherits="CommonControls_ucListening" %>
<table>
    <tr>
        <td>
            Media / Audio File (filename):
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtMedia" />
        </td>
    </tr>
    <tr>
        <td>
            Delay / Audio delay (seconds):
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtDelay" Text="0" />
        </td>
    </tr>
   <%-- <tr id="trTemplateListenWrite" runat="server" visible="false">
        <td>
            Max Word Count:
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtMaxWordCount" Text="0" />
        </td>
    </tr>
    <tr>
        <td>
            Answer:
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAnswer" />
        </td>
    </tr>--%>
    <tr id="trTemplateListenWithSelectionOptions" runat="server">
        <td>
            Question Options:
        </td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <asp:Repeater runat="server" ID="rptOptions" OnItemDataBound="rptOptions_ItemDataBound">
                    <HeaderTemplate>
                        <tr>
                            <th>
                                
                            </th>
                            <th>
                                Option
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox Text='<%#Eval("ID") %>' ID="chkSelectOption" runat="server" />
                                <input type="radio" id="radSelectOption" name="gpSame" runat="server" value='<%#Eval("ID") %>' />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOption" Width="400" TextMode="MultiLine" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
</table>
