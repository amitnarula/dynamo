<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucSpeak.ascx.cs" Inherits="CommonControls_ucSpeak" %>
<table>
    <tr>
        <td>
            Delay in playing the recording (seconds):
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtDelayInPlaying" Text="0" />
        </td>
    </tr>
    <tr>
        <td>
            Recording Time (seconds):
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtRecordingTime" Text="0"  />
            
        </td>
    </tr>
    <tr>
        <td>Beep:</td>
        <td>
            <asp:CheckBox Text="" ID="chkBeep" Checked="true" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPicture" Text="Picture (Filename)" runat="server" Visible="false" />
            
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtPictureFile" Visible="false"/>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblAudioFileName" Text="Audio (Filename)" runat="server" Visible="false" /></td>
        <td>
            <asp:TextBox runat="server" ID="txtAudioFileName" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAudioDelay" Text="Audio Delay (Seconds)" runat="server" Visible="false" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAudioDelay" Text="0" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblTranscript" Text="Transcript" runat="server" Visible="false" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtTranscript"  Visible="false" TextMode="MultiLine" Width="500" Height="100" />
        </td>
    </tr>
</table>
