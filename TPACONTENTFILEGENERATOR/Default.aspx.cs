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


    public List<Option> MultiChoiceOptions
    {

        get
        {

            List<Option> multiChoiceOptions = ViewState["MultiChoiceOptions"] as List<Option>;
            if (multiChoiceOptions == null)
            {
                multiChoiceOptions = new List<Option>();

            }
            List<Option> lstValidOptions = new List<Option>();
            var count = 0;
            foreach (var item in multiChoiceOptions)
            {
                if (!string.IsNullOrEmpty(item.OptionText))
                {
                    lstValidOptions.Add(new Option()
                    {
                        ID = count,
                        OptionText = item.OptionText,
                        Selected = item.Selected
                    });
                    count++;
                }

            }

            return lstValidOptions;
        }
        set
        {
            ViewState["MultiChoiceOptions"] = value;
        }
    }

    public Dictionary<string, List<Option>> FillInBlanksWithOptions
    {
        get
        {
            Dictionary<string, List<Option>> fillInBlanks = ViewState["FillInBlanksWithOptions"] as Dictionary<string, List<Option>>;
            if (fillInBlanks == null)
            {
                fillInBlanks = new Dictionary<string, List<Option>>();
            }
            return fillInBlanks;
        }
        set
        {
            ViewState["FillInBlanksWithOptions"] = value;
        }
    }

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
        //if (ddlModule.SelectedValue == "READING")
        //{
        //    CommonControls_ucReading dynamicControl = (CommonControls_ucReading)Page.LoadControl("~/commoncontrols/ucreading.ascx");
        //    dynamicControl.OnGenerateOptionsClick += new CommonControls_ucReading.GenerateOptionsClick(dynamicControl_OnGenerateOptionsClick);

        //    if (ddlTemplates.SelectedValue == "MULTI_CHOICE_MULTIPLE_ANSWER")
        //    {
        //        dynamicControl.Mode = "ReadingMultiSelect";

        //    }
        //    else if (ddlTemplates.SelectedValue == "MULTI_CHOICE_SINGLE_ANSWER")
        //    {
        //        dynamicControl.Mode = "ReadingMultiChoice";
        //    }

        //    pnlTemplateContent.Controls.Add(dynamicControl);
        //}
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
            {
                dynamicControl.ShowAudioBox = true;
                dynamicControl.ShowPictureBox = true;
            }

            if (ddlTemplates.SelectedValue == "SPEAK_ANSWER_SHORT_QUESTION")
            {
                dynamicControl.ShowAudioBox = true;
                dynamicControl.ShowPictureBox = true;
            }

            dynamicControl.ShowTranscriptBox = true; //show transcript anyhow.

            if (pnlTemplateContent.FindControl("dynamicControl") == null)
                pnlTemplateContent.Controls.Add(dynamicControl);

        }
        else if (ddlModule.SelectedValue == "LISTENING")
        {
            pnlListeningCommon.Visible = true;
            CommonControls_ucChoice dynamicControlChoice = (CommonControls_ucChoice)Page.LoadControl("~/commoncontrols/ucchoice.ascx");
            dynamicControlChoice.OnGenerateOptionsClick += new CommonControls_ucChoice.GenerateOptionsClick(dynamicControl_OnGenerateOptionsClick);

            if (ddlTemplates.SelectedValue == "LISTEN_MULTI_CHOICE" ||
                ddlTemplates.SelectedValue == "LISTEN_HIGHLIGHT_CORRECT_SUMMARY")
            {
                dynamicControlChoice.Mode = "MultiChoice";
                pnlTemplateContent.Controls.Add(dynamicControlChoice);
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_MULTI_SELECT")
            {
                dynamicControlChoice.Mode = "MultiSelect";
                pnlTemplateContent.Controls.Add(dynamicControlChoice);
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_SELECT_MISSING_WORD")
            {
                CommonControls_ucBlank dynamicControlBlank = (CommonControls_ucBlank)Page.LoadControl("~/commoncontrols/ucblank.ascx");
                dynamicControlBlank.OnGenerateAnswerClick += new CommonControls_ucBlank.GenerateAnswer(dynamicControlBlank_OnGenerateAnswerClick);

                pnlTemplateContent.Controls.Add(dynamicControlBlank);
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_AND_WRITE")
            {
                litMaxWordCount.Visible = txtMaxWordCount.Visible = true;
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_AND_HIGHLIGHT"
                || ddlTemplates.SelectedValue == "LISTEN_AND_FILL_BLANKS")
            {
                litMaxWordCount.Visible = txtMaxWordCount.Visible =
                    rptHighlightWords.Visible = litHighlightWords.Visible = true;
            }

        }
        else if (ddlModule.SelectedValue == "READING")
        {
            CommonControls_ucChoice dynamicControlChoice = (CommonControls_ucChoice)Page.LoadControl("~/commoncontrols/ucchoice.ascx");
            dynamicControlChoice.OnGenerateOptionsClick += new CommonControls_ucChoice.GenerateOptionsClick(dynamicControl_OnGenerateOptionsClick);

            if (ddlTemplates.SelectedValue == "MULTI_CHOICE_MULTIPLE_ANSWER")
            {

                dynamicControlChoice.Mode = "MultiSelect";
                pnlTemplateContent.Controls.Add(dynamicControlChoice);
            }
            else if (ddlTemplates.SelectedValue == "MULTI_CHOICE_SINGLE_ANSWER")
            {
                dynamicControlChoice.Mode = "MultiChoice";
                pnlTemplateContent.Controls.Add(dynamicControlChoice);
            }
            else if (ddlTemplates.SelectedValue == "FILL_IN_BLANKS" ||
                ddlTemplates.SelectedValue == "FILL_IN_BLANK_WITH_OPTIONS")
            {
                CommonControls_ucBlank dynamicControlBlank = (CommonControls_ucBlank)Page.LoadControl("~/commoncontrols/ucblank.ascx");
                dynamicControlBlank.OnGenerateAnswerClick += new CommonControls_ucBlank.GenerateAnswer(dynamicControlBlank_OnGenerateAnswerClick);

                pnlTemplateContent.Controls.Add(dynamicControlBlank);
            }
            //reorder is at selected index changed event of ddltemplate dropdown

        }

    }

    void dynamicControlBlank_OnGenerateAnswerClick(object sender, Dictionary<string, List<Option>> blanks)
    {
        string correctAnswerString = string.Empty;
        foreach (var item in blanks)
        {
            correctAnswerString += item.Value.SingleOrDefault(x => x.Selected).ID;
            correctAnswerString += "|";
        }
        FillInBlanksWithOptions = blanks;
        correctAnswerString = correctAnswerString.TrimEnd(new char[] { '|' });
        txtBoxAnswers.Text = correctAnswerString;
    }

    void dynamicControl_OnGenerateOptionsClick(object sender, List<Option> options)
    {
        string correctAnswerString = string.Empty;
        foreach (var item in options)
        {
            if (item.Selected)
            {
                correctAnswerString += item.ID;

                correctAnswerString += "|";
            }
        }
        MultiChoiceOptions = options;
        correctAnswerString = correctAnswerString.TrimEnd(new char[] { '|' });
        txtBoxAnswers.Text = correctAnswerString;
    }
    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {

        pnlTemplateSelection.Visible = false;
        pnlQuestionTemplate.Visible = true;

        lblLegend.Text = ddlPracticeSets.SelectedItem.Text + " >> " + ddlModule.SelectedItem.Text + " >> " + ddlTemplates.SelectedItem.Text;


        if (ddlTemplates.SelectedValue == "REORDER")
        {
            pnlReadingReorder.Visible = true;
            List<Option> lstOptions = new List<Option>();
            for (int count = 0; count < 10; count++)
            {
                lstOptions.Add(new Option()
                {
                    ID = count,
                    OptionText = string.Empty
                });
            }
            rptReoder.DataSource = lstOptions;
            rptReoder.DataBind();
        }
        else if (ddlTemplates.SelectedValue == "LISTEN_AND_HIGHLIGHT" ||
            ddlTemplates.SelectedValue == "LISTEN_AND_FILL_BLANKS")
        {
            List<Option> lstOptions = new List<Option>();
            for (int count   = 0; count < 10; count++)
            {
                lstOptions.Add(new Option()
                {
                    ID = count,
                    OptionText = string.Empty
                });
            }
            rptHighlightWords.DataSource = lstOptions;
            rptHighlightWords.DataBind();
        }
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetTemplateGenerator();

    }

    private void ResetTemplateGenerator()
    {
        pnlTemplateSelection.Visible = true;
        pnlQuestionTemplate.Visible = false;
        pnlReadingReorder.Visible = false;
        pnlListeningCommon.Visible = false;
        

        ddlTemplates.SelectedIndex = 0;
        ddlPracticeSets.SelectedIndex = 0;
        ddlModule.SelectedIndex = 0;

        MultiChoiceOptions = null;
        FillInBlanksWithOptions = null;
        

        ucQuestionCommon.Reset();
    }
    protected void btnGenerateTemplate_Click(object sender, EventArgs e)
    {

        string questionId = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        StringBuilder sb = new StringBuilder();
        sb.Append("<question>");
        sb.Append(string.Format("<id>{0}</id>", questionId));
        sb.Append(string.Format("<practiceSet>{0}</practiceSet>", ddlPracticeSets.SelectedValue));
        sb.Append(string.Format("<type>{0}</type>", ddlTemplates.SelectedValue));
        sb.Append(string.Format("<itemType>{0}</itemType>", ddlModule.SelectedValue));
        sb.Append(string.Format("<instruction>{0}</instruction>", ucQuestionCommon.Instruction));
        sb.Append(string.Format("<delayTime>{0}</delayTime>", ucQuestionCommon.DelayTime));
        sb.Append(string.Format("<attemptTime>{0}</attemptTime>", ucQuestionCommon.AttemptTime));


        //template
        sb.Append("<template>");

        //WRITE ESSAY AND SUMMARIZE TEXT
        sb.Append(string.Format("<description>{0}</description>", ucQuestionCommon.Description));
        sb.Append(string.Format("<title>{0}</title>", ucQuestionCommon.Title));

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

            sb.Append(string.Format("<delay>{0}</delay>", dynamicControl.DelayInRecording));
            sb.Append(string.Format("<recordingTime>{0}</recordingTime>", dynamicControl.RecordingTime));
            sb.Append(string.Format("<output>{0}</output>", questionId));
            sb.Append(string.Format("<answer>{0}</answer>", "sampleresponse"));
            sb.Append(string.Format("<beep>{0}</beep>", dynamicControl.AddBeep));

            if (ddlTemplates.SelectedValue == "SPEAK_LOOK")
            {
                sb.Append(string.Format("<picture>{0}.tpi</picture>", dynamicControl.Picture));

            }
            if (ddlTemplates.SelectedValue == "LOOK_SPEAK_LISTEN" || ddlTemplates.SelectedValue == "SPEAK_LISTEN")
            {
                sb.Append(string.Format("<audio>{0}.tpm</audio>", dynamicControl.AudioFile));
                sb.Append(string.Format("<audioDelay>{0}</audioDelay>", dynamicControl.AudioDelay));

                if (ddlTemplates.SelectedValue == "LOOK_SPEAK_LISTEN")
                    sb.Append(string.Format("<picture>{0}.tpi</picture>", dynamicControl.Picture));

            }
            if (ddlTemplates.SelectedValue == "SPEAK_ANSWER_SHORT_QUESTION")
            {
                sb.Append(string.Format("<audio>{0}.tpm</audio>", dynamicControl.AudioFile));

                if(!string.IsNullOrEmpty(dynamicControl.Picture))
                    sb.Append(string.Format("<picture>{0}.tpi</picture>", dynamicControl.Picture));

            }

            sb.Append(string.Format("<transcript>{0}</transcript>", dynamicControl.Transcript)); //adding transcript anyhow for all speaking templates
        }

        else if (ddlModule.SelectedValue == "LISTENING")
        {
            sb.Append(string.Format("<media>{0}.tpm</media>", txtMediaFileName.Text));
            sb.Append(string.Format("<delay>{0}</delay>", txtDelayAudio.Text));

            if (ddlTemplates.SelectedValue == "LISTEN_MULTI_CHOICE" ||
                ddlTemplates.SelectedValue == "LISTEN_MULTI_SELECT" ||
                ddlTemplates.SelectedValue == "LISTEN_HIGHLIGHT_CORRECT_SUMMARY")
            {
                sb.Append("<options>");

                for (int count = 0; count < MultiChoiceOptions.Count; count++)
                {
                    sb.Append(string.Format("<o{0}>{1}</o{0}>", count, MultiChoiceOptions[count].OptionText));
                }

                sb.Append("</options>");
                sb.Append(string.Format("<answer>{0}</answer>", txtBoxAnswers.Text));
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_SELECT_MISSING_WORD")
            {
                sb.Append("<options>");
                for (int count = 0; count < MultiChoiceOptions.Count; count++)
                {
                    sb.Append(string.Format("<o{0}>{1}</o{0}>", count, MultiChoiceOptions[count].OptionText));
                }

                sb.Append("</options>");
                sb.Append(string.Format("<answer>{0}</answer>", txtBoxAnswers.Text));
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_AND_WRITE")
            {
                sb.Append(string.Format("<maxWordCount>{0}</maxWordCount>", txtMaxWordCount.Text));
                sb.Append(string.Format("<answer>{0}</answer>", txtBoxAnswers.Text));
            }
            else if (ddlTemplates.SelectedValue == "LISTEN_AND_HIGHLIGHT" ||
                ddlTemplates.SelectedValue == "LISTEN_AND_FILL_BLANKS")
            {
                sb.Append(string.Format("<maxWordCount>{0}</maxWordCount>", txtMaxWordCount.Text));
                
                sb.Append("<options>");
                string correctAnswserString = string.Empty;
                
                foreach (RepeaterItem item in rptHighlightWords.Items)
                {
                    TextBox txtOption = item.FindControl("txtCorrectHighlightOption") as TextBox;
                    
                    if (!string.IsNullOrEmpty(txtOption.Text))
                    {
                        correctAnswserString += txtOption.Text;
                        correctAnswserString += "|";
                    }
                }
                sb.Append("</options>");
                sb.Append(string.Format("<answer>{0}</answer>", correctAnswserString.TrimEnd(new char[] { '|' })));
            }

        }
        else if (ddlModule.SelectedValue == "READING")
        {

            if (ddlTemplates.SelectedValue == "MULTI_CHOICE_SINGLE_ANSWER" ||
                ddlTemplates.SelectedValue == "MULTI_CHOICE_MULTIPLE_ANSWER")
            {
                sb.Append("<options>");
                for (int count = 0; count < MultiChoiceOptions.Count; count++)
                {
                    sb.Append(string.Format("<o{0}>{1}</o{0}>", count, MultiChoiceOptions[count].OptionText));
                }

                sb.Append("</options>");
                sb.Append(string.Format("<answer>{0}</answer>", txtBoxAnswers.Text));
            }
            else if (ddlTemplates.SelectedValue == "FILL_IN_BLANK_WITH_OPTIONS" ||
                ddlTemplates.SelectedValue == "FILL_IN_BLANKS")
            {
                sb.Append("<blanks>");

                foreach (var item in FillInBlanksWithOptions)
                {
                    sb.Append("<blank>");

                    for (int count = 0; count < item.Value.Count; count++)
                    {
                        sb.Append(string.Format("<o{0}>{1}</o{0}>", count, item.Value[count].OptionText));
                    }


                    sb.Append("</blank>");

                    if (ddlTemplates.SelectedValue == "FILL_IN_BLANKS")
                        break; //only 1 blank is required there. so pick first one..
                }


                sb.Append("</blanks>");
                sb.Append(string.Format("<answer>{0}</answer>", txtBoxAnswers.Text));
            }
            else if (ddlTemplates.SelectedValue == "REORDER")
            {
                sb.Append("<options>");
                string correctAnswserString = string.Empty;
                int count = 0;
                foreach (RepeaterItem item in rptReoder.Items)
                {
                    TextBox txtOption = item.FindControl("txtOptionText") as TextBox;
                    TextBox txtOrder = item.FindControl("txtOptionCorrectOreder") as TextBox;

                    if (!string.IsNullOrEmpty(txtOption.Text))
                    {
                        count += 1;
                        sb.Append(string.Format("<o{0}>{1}</o{0}>", count, txtOption.Text));

                        correctAnswserString += txtOrder.Text;
                        correctAnswserString += "|";
                    }
                }
                sb.Append("</options>");
                sb.Append(string.Format("<answer>{0}</answer>", correctAnswserString.TrimEnd(new char[] { '|' })));
            }


        }


        sb.Append("</template>");
        sb.Append(string.Format("</question>"));



        XDocument doc = XDocument.Parse(sb.ToString());
        //File.WriteAllText(Server.MapPath(string.Format("~/data/{0}.xml", questionId + "_" + ddlModule.SelectedValue)), doc.ToString());

        //download as xml
        string attachment = string.Format("attachment; filename={0}.xml", ddlModule.SelectedValue + "_" + "_" + ddlPracticeSets.SelectedItem.Text + "_" + questionId);
        Response.ClearContent();
        Response.ContentType = "application/xml";
        Response.AddHeader("content-disposition", attachment);
        Response.Write(doc.ToString());
        Response.End();

        ResetTemplateGenerator();


    }
}