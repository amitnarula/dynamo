using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page
{
    private void BindTemplates()
    {
        DataSet dsTemplates = new DataSet();
        dsTemplates.ReadXml(Server.MapPath("~/data/templates.xml"));

        ddlTemplates.DataSource = dsTemplates;
        ddlTemplates.DataTextField = "value";
        ddlTemplates.DataValueField = "titleKey";
        ddlTemplates.DataBind();

        ddlTemplates.Items.Insert(0, "--SELECT--");
    }

    protected void Page_OnInit(object sender, EventArgs e)
    {
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTemplates();
        }

        if (ddlTemplates.SelectedValue == "WRITE_ESSAY" || ddlTemplates.SelectedValue == "SUMMARIZE_TEXT")
        {
            Control dynamicControl = (Control)Page.LoadControl("~/commoncontrols/ucwriting.ascx");
            dynamicControl.ID = "dynamicControl";
            if (pnlTemplateContent.FindControl("dynamicControl") == null)
                pnlTemplateContent.Controls.Add(dynamicControl);

        }
        else if (ddlTemplates.SelectedValue == "SPEAK_LOOK" ||
            ddlTemplates.SelectedValue == "SPEAK_READ" ||
            ddlTemplates.SelectedValue == "SPEAK_LISTEN" ||
            ddlTemplates.SelectedValue == "LOOK_SPEAK_LISTEN" ||
            ddlTemplates.SelectedValue == "SPEAK_ANSWER_SHORT_QUESTION")
        {
            CommonControls_ucSpeak dynamicControl = (CommonControls_ucSpeak)Page.LoadControl("~/commoncontrols/ucspeak.ascx");
            dynamicControl.ID = "dynamicControl";

            if (ddlTemplates.SelectedValue == "SPEAK_LOOK")
                dynamicControl.ShowPictureBox = true;

            if (ddlTemplates.SelectedValue == "LOOK_SPEAK_LISTEN" || ddlTemplates.SelectedValue == "SPEAK_LISTEN")
                dynamicControl.ShowAudioBox = true;

            if (ddlTemplates.SelectedValue == "SPEAK_ANSWER_SHORT_QUESTION")
                dynamicControl.ShowTranscriptBox = true;

            if (pnlTemplateContent.FindControl("dynamicControl") == null)
                pnlTemplateContent.Controls.Add(dynamicControl);

        }
        else if (ddlModule.SelectedValue == "LISTENING")
        {
            CommonControls_ucListening dynamicControl = (CommonControls_ucListening)Page.LoadControl("~/commoncontrols/uclistening.ascx");
            dynamicControl.ID = "dynamicControl";

            if (ddlTemplates.SelectedValue == "LISTEN_AND_WRITE")
            {
                dynamicControl.ListenAndWrite = true;
            }
        }

    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        pnlTemplateSelection.Visible = false;
        pnlQuestionTemplate.Visible = true;

        lblLegend.Text = ddlPracticeSets.SelectedItem.Text + " >> " + ddlModule.SelectedItem.Text + " >> " + ddlTemplates.SelectedItem.Text;

        

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetTemplateGenerator();

    }

    private void ResetTemplateGenerator()
    {
        pnlTemplateSelection.Visible = true;
        pnlQuestionTemplate.Visible = false;

        ddlTemplates.SelectedIndex = 0;
        ddlPracticeSets.SelectedIndex = 0;
        ddlModule.SelectedIndex = 0;

        ucQuestionCommon.Reset();
    }
    protected void btnGenerateTemplate_Click(object sender, EventArgs e)
    {
        string questionId = Guid.NewGuid().ToString().Replace("-","").ToLower();
        StringBuilder sb = new StringBuilder();
        sb.Append("<question>");
        sb.Append(string.Format("<id>{0}</id>",questionId));
        sb.Append(string.Format("<practiceSet>{0}</practiceSet>", ddlPracticeSets.SelectedValue));
        sb.Append(string.Format("<type>{0}</type>", ddlTemplates.SelectedValue));
        sb.Append(string.Format("<itemType>{0}</itemType>", ddlModule.SelectedValue));
        sb.Append(string.Format("<instruction>{0}</instruction>", ucQuestionCommon.Instruction));
        sb.Append(string.Format("<delayTime>{0}</delayTime>", ucQuestionCommon.DelayTime));
        sb.Append(string.Format("<attemptTime>{0}</attemptTime>", ucQuestionCommon.AttemptTime));


        //template
        sb.Append("<template>");

        //WRITE ESSAY AND SUMMARIZE TEXT
        sb.Append(string.Format("<description>{0}</description>",ucQuestionCommon.Description));
        sb.Append(string.Format("<title>{0}</title>",ucQuestionCommon.Title));

        if (ddlTemplates.SelectedValue == "WRITE_ESSAY" || ddlTemplates.SelectedValue == "SUMMARIZE_TEXT")
        {
            var dynamicControl = pnlTemplateContent.FindControl("dynamicControl") as CommonControls_ucWriting;
            sb.Append(string.Format("<maxWordCount>{0}</maxWordCount>", dynamicControl.MaxWordCount));
            sb.Append(string.Format("<answer>{0}</answer>", dynamicControl.CorrectAnswer));
        }
        else if (ddlTemplates.SelectedValue == "SPEAK_READ" ||
            ddlTemplates.SelectedValue == "SPEAK_LOOK" ||
            ddlTemplates.SelectedValue == "LOOK_SPEAK_LISTEN" ||
            ddlTemplates.SelectedValue == "SPEAK_LISTEN" ||
            ddlTemplates.SelectedValue == "SPEAK_ANSWER_SHORT_QUESTION")
        {
            var dynamicControl = pnlTemplateContent.FindControl("dynamicControl") as CommonControls_ucSpeak;
            
            sb.Append(string.Format("<delay>{0}</delay>",dynamicControl.DelayInRecording));
            sb.Append(string.Format("<recordingTime>{0}</recordingTime>", dynamicControl.RecordingTime));
            sb.Append(string.Format("<output>{0}</output>", questionId));
            sb.Append(string.Format("<answer>{0}</answer>", "sampleresponse"));
            sb.Append(string.Format("<beep>{0}</beep>", dynamicControl.AddBeep));

            if (ddlTemplates.SelectedValue == "SPEAK_LOOK")
            {
                sb.Append(string.Format("<picture>{0}.tpi</picture>", dynamicControl.Picture));

            }
            if (ddlTemplates.SelectedValue == "LOOK_SPEAK_LISTEN" || ddlTemplates.SelectedValue=="SPEAK_LISTEN")
            {
                sb.Append(string.Format("<audio>{0}.tpm</audio>", dynamicControl.AudioFile));
                sb.Append(string.Format("<audioDelay>{0}</audioDelay>", dynamicControl.AudioDelay));

            }
            if (ddlTemplates.SelectedValue == "SPEAK_ANSWER_SHORT_QUESTION")
                sb.Append(string.Format("<transcript>{0}</transcript", dynamicControl.Transcript));
        }

        else if (ddlModule.SelectedValue == "LISTENING")
        {
            var dynamicControl = pnlTemplateContent.FindControl("dynamicControl") as CommonControls_ucListening;
            
            
        }
        

        sb.Append("</template>");
        sb.Append(string.Format("</question>"));

        

        XDocument doc = XDocument.Parse(sb.ToString());
        //File.WriteAllText(Server.MapPath(string.Format("~/data/{0}.xml", questionId + "_" + ddlModule.SelectedValue)), doc.ToString());

        //download as xml
        string attachment = string.Format("attachment; filename={0}.xml",ddlModule.SelectedValue+"_"+"_"+ddlPracticeSets.SelectedItem.Text+"_"+ questionId);
        Response.ClearContent();
        Response.ContentType = "application/xml";
        Response.AddHeader("content-disposition", attachment);
        Response.Write(doc.ToString());
        Response.End();

        ResetTemplateGenerator();

        
    }
}