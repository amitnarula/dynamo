using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TPA.CoreFramework;
using TPA.Entities;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for BreadCrumb.xaml
    /// </summary>
    public partial class BreadCrumb : UserControl
    {
        public string PracticeSetId { get; set; }
        public QuestionTemplates QuestionTemplate { get; set; }
        public QuestionType QuestionType { get; set; }

        private string GetQuestionTypeInfo()
        {
            string questionType=string.Empty;
            switch (this.QuestionType)
            {
                case QuestionType.READING:
                    questionType= "Reading questions";
                    break;
                case QuestionType.WRITING:
                    questionType = "Writing questions";
                    break;
                case QuestionType.SPEAKING:
                    questionType = "Speaking questions";
                    break;
                case QuestionType.LISTENING:
                    questionType = "Listening questions";
                    break;
                default:
                    break;
            }
            return questionType;
        }

        private string GetQuestionTemplateTitle()
        { 
            string questionTemplate = string.Empty;

            DataTable dtTitle = FileReader.ReadFile(FileReader.FileType.QUESTION_TITLES).Tables[0];
            DataRow dRow = dtTitle.Select("titleKey = '" + this.QuestionTemplate.ToString() + "'").SingleOrDefault();

            if (dRow != null)
            {
                return dRow["value"].ToString();
            }
            return "Invalid Template Key";
        }

        private string GetQuestionTemplateInfo()
        {
            string questionTemplate = string.Empty;
            switch (this.QuestionTemplate)
            {
                case QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER:
                    questionTemplate = "Multiple choice, choose single answer";
                    break;
                case QuestionTemplates.WRITE_ESSAY:
                    questionTemplate = "Write an essay";
                    break;
                case QuestionTemplates.SUMMARIZE_TEXT:
                    questionTemplate = "Summarize the text";
                    break;
                case QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER:
                    questionTemplate = "Multiple choice, choose multiple answers";
                    break;
                case QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS:
                    questionTemplate = "Fill in the blanks";
                    break;
                case QuestionTemplates.REORDER:
                    questionTemplate = "Reorder paragraphs";
                    break;
                case QuestionTemplates.FILL_IN_BLANKS:
                    questionTemplate = "Fill in the blanks";
                    break;
                case QuestionTemplates.LISTEN_MULTI_CHOICE:
                    questionTemplate = "Multiple choice, choose single answer";
                    break;
                case QuestionTemplates.LISTEN_MULTI_SELECT:
                    questionTemplate = "Multiple choice, choose multiple answers";
                    break;
                case QuestionTemplates.LISTEN_AND_WRITE:
                    questionTemplate = "Summarize the spoken text";
                    break;
                case QuestionTemplates.LISTEN_AND_FILL_BLANKS:
                    questionTemplate = "Listen and fill in the blanks";
                    break;
                case QuestionTemplates.SPEAK_LISTEN:
                    questionTemplate = "Speak the spoken text";
                    break;
                case QuestionTemplates.LOOK_SPEAK_LISTEN:
                    questionTemplate = "Re-tell lecture";
                    break;
                case QuestionTemplates.SPEAK_LOOK:
                    questionTemplate = "Look at the picture and speak";
                    break;
                case QuestionTemplates.SPEAK_READ:
                    questionTemplate = "Read and speak accordingly";
                    break;
                case QuestionTemplates.LISTEN_AND_HIGHLIGHT:
                    questionTemplate = "Highlight the incorrect words";
                    break;
                default:
                    break;
            }
            return questionTemplate;
        }

        private string GetPracticeSetInfo(string practiceSetId)
        {
           DataSet dsPracticeSet =  FileReader.ReadFile(FileReader.FileType.PRACTICE_SET);
           return Convert.ToString(dsPracticeSet.Tables[0].Select("id='" + practiceSetId + "'")[0]["Name"]);
        }

        public BreadCrumb()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.PracticeSetId))
            {
                txtLegend.Inlines.Add(new Run(GetPracticeSetInfo(PracticeSetId) + " : "));
                txtLegend.Inlines.Add(new Run(GetQuestionTypeInfo()+" - "));

                Run italicRun = new Run(GetQuestionTemplateTitle());
                italicRun.FontStyle = FontStyles.Italic;
                txtLegend.Inlines.Add(italicRun);
                

                lblLegend.Content = GetQuestionTypeInfo() + ":" + GetQuestionTemplateTitle() + "-" + GetPracticeSetInfo(PracticeSetId);
            }
            
        }
    }
}
