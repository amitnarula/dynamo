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
using System.Windows.Shapes;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for TranscriptBox.xaml
    /// </summary>
    public partial class TranscriptBox : Window
    {
        public string Transcript { get; set; }

        public TranscriptBox()
        {
            InitializeComponent();
            //txtBlkTranscript.Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MAKING THE SAMPLE ANSWER a bit different

            string[] transcriptSplit = new string[] { };

            if(!string.IsNullOrEmpty(Transcript))
            {
                transcriptSplit = Transcript.Split(new string[] { "Sample Answer:" }, StringSplitOptions.RemoveEmptyEntries);


                txtBlkTranscript.Inlines.Add(new Run(transcriptSplit[0]));

                if (transcriptSplit.Length == 2)
                {
                    Run rnSampleAnswerHeading = new Run("Sample Answer:");
                    rnSampleAnswerHeading.FontWeight = FontWeights.Bold;
                    rnSampleAnswerHeading.FontSize = 14;
                    txtBlkTranscript.Inlines.Add(rnSampleAnswerHeading);

                    Run rnSampleAnswerContent = new Run(transcriptSplit[1]);
                    rnSampleAnswerContent.FontStyle = FontStyles.Italic;
                    txtBlkTranscript.Inlines.Add(rnSampleAnswerContent);
                }
                
                //txtBlkTranscript.Text = Transcript;
                this.Height = 450;

            }

           
        }
    }
}
