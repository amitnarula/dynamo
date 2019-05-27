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
