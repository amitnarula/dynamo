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
using TPA.CoreFramework;
using TPA.Entities;

namespace TPA.Templates.Speaking
{
    /// <summary>
    /// Interaction logic for LookAndSpeak.xaml
    /// </summary>
    public partial class LookAndSpeak : UserControl,ISwitchable
    {
        SpeakLookQuestion question;
        public LookAndSpeak()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (SpeakLookQuestion)state;
            if (question.Mode==Mode.QUESTION)
            {
                audioRecorder.Delay = question.Delay;
                audioRecorder.RecordingTime = question.RecordingTime;
                audioRecorder.OutputFile = question.OutputFile;
                audioRecorder.Picture = question.Picture;
                audioRecorder.PlayBeepSound = question.PlayBeep;
                audioRecorder.InitializeRecorder();

                audioCorrectAnswer.Visibility = Visibility.Hidden;
                audioUserAnswer.Visibility = Visibility.Hidden;
            }
            else if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
            {
                string[] correctAnswers = question.CorrectAnswers;
                //audioCorrectAnswer.Visibility = Visibility.Visible;
                audioCorrectAnswer.Media = MediaReader.GetMediaPath(correctAnswers[0] + ".mp3","SPK");//TODO Temporary
                audioCorrectAnswer.PlayStopClicked += 
                    new Common.SmallAudioPlayer.PlayStopEventHandler(audioCorrectAnswer_PlayStopClicked);

                audioUserAnswer.Visibility = Visibility.Visible;
                audioUserAnswer.Media = MediaReader.GetTempMediaPath(question.OutputFile + ".wav");
                audioUserAnswer.PlayStopClicked += 
                    new Common.SmallAudioPlayer.PlayStopEventHandler(audioUserAnswer_PlayStopClicked);
            }
            txtBlkInstruction.Text = question.Instruction;
            prevNext.QuestionContext = question;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.SPEAKING;

        }

        void audioUserAnswer_PlayStopClicked(object sender, EventArgs e)
        {
            audioCorrectAnswer.Stop();
        }

        void audioCorrectAnswer_PlayStopClicked(object sender, EventArgs e)
        {
            audioUserAnswer.Stop();
        }

        private void imgDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            // ... Create a new BitmapImage.
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(MediaReader.GetMediaPath(question.Picture));
            bmpImage.EndInit();

            // ... Get Image reference from sender.
            var image = sender as Image;
            // ... Assign Source.
            image.Source = bmpImage;
        }
    }
}
