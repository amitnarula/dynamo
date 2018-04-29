<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucChoice.ascx.cs" Inherits="CommonControls_ucChoice" %>
<asp:Panel runat="server" ID="pnlMultiChoiceOrSelect" Visible="true">
    <table border="0" cellpadding="0" cellspacing="0">
        <asp:Repeater runat="server" ID="rptOptions" OnItemDataBound="rptOptions_ItemDataBound">
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
                        <asp:RadioButton Text='<%#Eval("ID") %>' onclick='CheckOtherIsCheckedByGVID(this)' ID="radBtnOption" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOption" Width="400" TextMode="MultiLine" />
                    </td>
                </tr>
            </ItemTemplate>
            
        </asp:Repeater>
    </table>
    <asp:Button Text="Generate Answers" OnClick="btnGenerateOptions_Click" ID="btnGenerateOptions" runat="server" />
    <br />
    <br />
</asp:Panel>
<script type="text/javascript" language="javascript">

            function CheckOtherIsCheckedByGVID(spanChk) {

                var IsChecked = spanChk.checked;

                if (IsChecked) {

                    spanChk.parentElement.parentElement.style.backgroundColor = '#228b22';

                    spanChk.parentElement.parentElement.style.color = 'white';

                }

                var CurrentRdbID = spanChk.id;

                var Chk = spanChk;

                Parent = document.getElementById("<%=pnlMultiChoiceOrSelect.ClientID%>");

                var items = Parent.getElementsByTagName('input');

                for (i = 0; i < items.length; i++) {

                    if (items[i].id != CurrentRdbID && items[i].type == "radio") {

                        if (items[i].checked) {

                            items[i].checked = false;

                            items[i].parentElement.parentElement.style.backgroundColor = 'white'
 
                            items[i].parentElement.parentElement.style.color = 'black';
                        }

                    }

                }

            }

</script>

