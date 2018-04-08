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
using System.IO;
using TPA.CoreFramework;
using TPA.Entities;

namespace TPA.Templates.Speaking
{
    /// <summary>
    /// Interaction logic for ListenAndSpeak.xaml
    /// </summary>
    public partial class ListenAndSpeak : UserControl, ISwitchable
    {
        SpeakListenQuestion question;
        public ListenAndSpeak()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (SpeakListenQuestion)state;
            txtBlkInstruction.Text = question.Instruction.Trim().Replace("{newline}",string.Empty);
            if (question.Mode == Mode.QUESTION)
            {
                audioRecorder.PlayBeepSound = question.PlayBeep;

                audioPlayer.Delay = question.AudioDelay;
                audioPlayer.Media = question.Media;
                audioPlayer.AudioEnded += audioPlayer_AudioEnded;
                audioCorrectAnswer.Visibility = Visibility.Hidden;
                audioUserAnswer.Visibility = Visibility.Hidden;
                transcriptButton.Visibility = Visibility.Hidden;
                
            }
            if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.TIME_OUT)
            {
                string[] correctAnswers = question.CorrectAnswers;
                //audioCorrectAnswer.Visibility = Visibility.Visible;

                //audioCorrectAnswer.Media = MediaReader.GetMediaPath(correctAnswers[0] + ".mp3","SPK");//TODO Temporary previous
                audioCorrectAnswer.Media = MediaReader.GetMediaPath(correctAnswers[0], "SPK");//TODO Temporary
                
                audioCorrectAnswer.PlayStopClicked += 
                    new Common.SmallAudioPlayer.PlayStopEventHandler(audioCorrectAnswer_PlayStopClicked);

                audioUserAnswer.Visibility = Visibility.Visible;
                audioUserAnswer.Media = MediaReader.GetTempMediaPath(question.OutputFile + ".wav");
                audioUserAnswer.PlayStopClicked += 
                    new Common.SmallAudioPlayer.PlayStopEventHandler(audioUserAnswer_PlayStopClicked);

                if(question.ShowTranscript)
                {
                    transcriptButton.Visibility = Visibility.Visible;
                    transcriptButton.Transcript = question.SampleTranscript;//TO DO Dynamic
                }
            }
            
            
            prevNext.QuestionContext = question;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.SPEAKING;
        }

        void audioCorrectAnswer_PlayStopClicked(object sender, EventArgs e)
        {
            audioUserAnswer.Stop();
        }

        void audioUserAnswer_PlayStopClicked(object sender, EventArgs e)
        {
            audioCorrectAnswer.Stop();
        }

        void audioPlayer_AudioEnded(object sender, EventArgs e)
        {
            audioRecorder.Delay = question.Delay;
            audioRecorder.OutputFile = question.OutputFile;
            audioRecorder.RecordingTime = question.RecordingTime;
            audioRecorder.InitializeRecorder();

        }
    }
}
