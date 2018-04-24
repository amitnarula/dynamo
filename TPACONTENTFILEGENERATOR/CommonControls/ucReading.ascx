<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucReading.ascx.cs" Inherits="CommonControls_ucReading" %>
<asp:Panel runat="server" ID="pnlMultiChoiceOrSelect" Visible="true">
    <table border="0" cellpadding="0" cellspacing="0">
        <asp:Repeater runat="server" ID="rptOptions" ClientIDMode="Static" OnItemDataBound="rptOptions_ItemDataBound">
            <HeaderTemplate>
                <tr>
                    <th>
                        Select Correct Option
                    </th>
                    <th>
                        Options
                    </th>
                    
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox Text='<%#Eval("ID") %>' ID="chkSelectOption" runat="server" />
                        <asp:RadioButton Text='<%#Eval("ID") %>' onclick='fnCheckUnCheck(this.id)' ID="radBtnOption" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOption" Width="400" TextMode="MultiLine" />
                    </td>
                </tr>
            </ItemTemplate>
            
        </asp:Repeater>
    </table>
    <asp:Button Text="Generate Options" OnClick="btnGenerateOptions_Click" ID="btnGenerateOptions" runat="server" />
    <br />
    <br />
    <br />
</asp:Panel>
<script type="text/javascript" language="javascript">

    function fnCheckUnCheck(objId) {
        var grd = document.getElementById("rptOptions");

        //Collect A
        var rdoArray = grd.getElementsByTagName("input");

        for (i = 0; i <= rdoArray.length - 1; i++) {
            if (rdoArray[i].type == 'radio') {
                if (rdoArray[i].id != objId) {
                    rdoArray[i].checked = false;
                }
            }
        }
    }  
</script>

