using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using TPA.Entities;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for Transcript.xaml
    /// </summary>
    public partial class Transcript : UserControl
    {
        public string Content { get; set; }
        public Mode Mode { get; set; }
        public string[] TranscriptAnswserArray { get; set; }
        public int ID { get; set; }
        public bool HighlightAnswer { get; set; }

        public Transcript()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTranscript();


        }

        public void LoadTranscript()
        {
            Regex regEx = new Regex(@"{by}([A-Za-z0-9\-]+)\{:by}");

            Match match = regEx.Match(Content);
            string matchValue = match.Groups[1].Value;

            if (!string.IsNullOrEmpty(matchValue))
            {
                lblFrom.Content = matchValue + ":";

                Content = Content.Replace(match.Value, string.Empty);
            }

            string[] splitDescriptions = Content.Split(new string[] { "{blank}" }, StringSplitOptions.None);

            string[] answerArrayForTranscriptLine = new string[] { };


            if (TranscriptAnswserArray.Any())
            {
                //Fetching only a portion of whole answer
                answerArrayForTranscriptLine = TranscriptAnswserArray.Skip(this.ID).Take(splitDescriptions.Length).ToArray();

            }

            //wrapContent.Children.Clear(); //14 May 2016 inline changes
            wrapContent.Inlines.Clear();

            for (int count = 0; count < splitDescriptions.Length; count++)
            {
                Run lblDesc = new Run(splitDescriptions[count]);
                lblDesc.BaselineAlignment = BaselineAlignment.Center;
                wrapContent.Inlines.Add(lblDesc);

                //Label lblDesc = new Label();
                //lblDesc.Content = splitDescriptions[count];
                //lblDesc.FontSize = 14;
                //wrapContent.Children.Add(lblDesc);

                TextBox txtBx = new TextBox();
                txtBx.Height = 20;
                txtBx.FontSize = 11;
                txtBx.Width = 90;
                txtBx.Background = !HighlightAnswer ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Wheat);

                txtBx.Margin = new Thickness(1, 0, 0, 2);

                if (Mode == Mode.ANSWER_KEY || Mode == Mode.TIME_OUT)
                    txtBx.IsReadOnly = true;

                //wrapContent.Children.Add(txtBx);

                //Prevent adding the textbox for the last desscription line
                if (!object.Equals(splitDescriptions.Last(), splitDescriptions[count]))
                {
                    wrapContent.Inlines.Add(txtBx);
                }

                if (Mode == Mode.ANSWER_KEY || Mode == Mode.QUESTION || Mode == Mode.TIME_OUT)
                {
                    if (!object.Equals(splitDescriptions.Last(), splitDescriptions[count])) //if it is not last
                    {
                        if (answerArrayForTranscriptLine.Any() && !string.IsNullOrEmpty(answerArrayForTranscriptLine[count]))
                        {
                            txtBx.Text = answerArrayForTranscriptLine[count].Trim();
                        }
                    }
                }


            }
        }
    }
}
