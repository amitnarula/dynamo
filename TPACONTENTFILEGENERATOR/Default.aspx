﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="~/CommonControls/ucQuestionCommon.ascx" TagPrefix="uc1" TagName="ucQuestionCommon" %>
<%@ Register Src="~/CommonControls/ucWriting.ascx" TagPrefix="uc1" TagName="ucWriting" %>
<%@ Register Src="~/CommonControls/ucSpeak.ascx" TagPrefix="uc1" TagName="ucSpeakRead" %>
<%@ Register Src="~/CommonControls/ucListening.ascx" TagPrefix="uc1" TagName="ucListening" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <asp:Panel runat="server" ID="pnlTemplateSelection">
                <table>
                    <tr>
                        <td>Select Practice Set:</td>
                        <td>
                            <asp:DropDownList ID="ddlPracticeSets" runat="server">
                                <asp:ListItem Text="Practice Set 1" Value="94fbb82b4fd1485eb61cc3c671da384d" />
                                <asp:ListItem Text="Practice Set 2" Value="7fdd81499d6147a1b4a817e56db88024" />
                                <asp:ListItem Text="Practice Set 3" Value="88f631b360734b7da3b7c3e6151bd840" />
                                <asp:ListItem Text="Practice Set 4" Value="7b04cda952c144788c9d27aa2d5dc710" />
                                <asp:ListItem Text="Practice Set 5" Value="d9d885229cf64aceb84b19095b1ffd9c" />
                                <asp:ListItem Text="Practice Set 6" Value="5dcbc550333c488996e65d41dad0bfe7" />
                                <asp:ListItem Text="Practice Set 7" Value="0b7002c915a441eebdb4448e4388ba5b" />
                                <asp:ListItem Text="Practice Set 8" Value="05424cf5c42745de8f6fdef719e5351b" />
                                <asp:ListItem Text="Practice Set 9" Value="db28642c43f44f048e0e1be5cf8984f6" />
                                <asp:ListItem Text="Practice Set 10" Value="4e1a53fbe93d491289754abda2ea6984" />
                                
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            Select Module:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlModule">
                                <asp:ListItem Text="Writing" Value="WRITING" />
                                <asp:ListItem Text="Reading" Value="READING" />
                                <asp:ListItem Text="Speaking" Value="SPEAKING" />
                                <asp:ListItem  Text="Listening" Value="LISTENING" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            Select Template:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlTemplates" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                </table>
                


            </asp:Panel>
            <asp:Panel runat="server" ID="pnlQuestionTemplate" Visible="false">
                <asp:Label runat="server" ID="lblLegend" Font-Bold="True" Font-Size="Medium"></asp:Label>
                <uc1:ucQuestionCommon runat="server" ID="ucQuestionCommon" />
                <asp:Panel runat="server" ID="pnlTemplateContent">
                </asp:Panel>
                <asp:Button Text="Generate Template" ID="btnGenerateTemplate" OnClick="btnGenerateTemplate_Click" runat="server" />
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            </asp:Panel>
            
        </div>
    </form>
</body>
</html>