using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucSpeak : BaseUserControl
{
    public string DelayInRecording { get { return new TimeSpan(0, 0, Convert.ToInt32(txtDelayInPlaying.Text)).ToString(); } }
    public string RecordingTime { get { return new TimeSpan(0, 0, Convert.ToInt32(txtRecordingTime.Text)).ToString(); } }
    public string Picture { get { return txtPictureFile.Text; } }
    public string AudioDelay { get { return new TimeSpan(0, 0, Convert.ToInt32(txtAudioDelay.Text)).ToString(); } }
    public string AudioFile { get { return txtAudioFileName.Text; } }
    public string Transcript { get { return txtTranscript.Text; } }
    public bool ShowPictureBox { get; set; }
    public bool ShowAudioBox { get; set; }
    public bool ShowTranscriptBox { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        lblPicture.Visible = txtPictureFile.Visible = ShowPictureBox;

        lblAudioFileName.Visible = txtAudioFileName.Visible = txtAudioDelay.Visible = lblAudioDelay.Visible = ShowAudioBox;

        lblTranscript.Visible = txtTranscript.Visible = ShowTranscriptBox;

    }
}