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
    /// Interaction logic for ListenAndSpeak.xaml
    /// </summary>
    public partial class LookListenAndSpeak : UserControl, ISwitchable
    {
        LookSpeakListenQuestion question;
        public LookListenAndSpeak()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (LookSpeakListenQuestion)state;
            txtBlkInstruction.Text = question.Instruction;
            transcriptButton.Visibility = Visibility.Collapsed;
            if (question.Mode == Mode.QUESTION)
            {
                audioRecorder.PlayBeepSound = question.PlayBeep;

                audioPlayer.Delay = question.AudioDelay;
                audioPlayer.Media = question.Media;
                audioPlayer.AudioEnded += audioPlayer_AudioEnded;
                audioCorrectAnswer.Visibility = Visibility.Hidden;
                audioUserAnswer.Visibility = Visibility.Collapsed;
                
                
            }
            if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
            {
                string[] correctAnswers = question.CorrectAnswers;
                //audioCorrectAnswer.Visibility = Visibility.Visible;
                audioCorrectAnswer.Media = MediaReader.GetMediaPath(correctAnswers[0] + ".mp3", "SPK");//TODO Temporary
                audioCorrectAnswer.PlayStopClicked += new Common.SmallAudioPlayer.PlayStopEventHandler(audioCorrectAnswer_PlayStopClicked);

                audioUserAnswer.Visibility = Visibility.Visible;
                audioUserAnswer.Media = MediaReader.GetTempMediaPath(question.OutputFile + ".wav");
                audioUserAnswer.PlayStopClicked += new Common.SmallAudioPlayer.PlayStopEventHandler(audioUserAnswer_PlayStopClicked);

                if (question.ShowTranscript)
                {
                    transcriptButton.Visibility = Visibility.Visible;
                    transcriptButton.Transcript = question.SampleTranscript;//TO DO Dynamic
                    transcriptButton.Media = question.Media;
                }
            }
            
            
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

        void audioPlayer_AudioEnded(object sender, EventArgs e)
        {
            audioRecorder.Delay = question.Delay;
            audioRecorder.OutputFile = question.OutputFile;
            audioRecorder.RecordingTime = question.RecordingTime;
            audioRecorder.InitializeRecorder();

        }
        private void imgDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(question.Picture))
            {
                question.Picture = "noimage.jpg";
                //imgDisplay.Visibility = Visibility.Collapsed;
                return;
            }

            // ... Create a new BitmapImage.
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(MediaReader.GetMediaPath(question.Picture));
            bmpImage.EndInit();

            // ... Get Image reference from sender.
            var image = sender as Image;
            // ... Assign Source.
            image.Source = bmpImage;

            //When image is present try to adjust the screen
            //image.Stretch = Stretch.None;
            audioPlayer.HorizontalAlignment = audioRecorder.HorizontalAlignment = HorizontalAlignment.Right;
            audioPlayer.Margin = audioRecorder.Margin = new Thickness(0, 0, 100, 0);

        }
    }
}
