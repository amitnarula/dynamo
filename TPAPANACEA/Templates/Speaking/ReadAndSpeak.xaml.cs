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
    /// Interaction logic for ReadAndSpeak.xaml
    /// </summary>
    public partial class ReadAndSpeak : UserControl, ISwitchable
    {
        SpeakReadQuestion question;
        public ReadAndSpeak()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (SpeakReadQuestion)state;
            txtBlkInstruction.Text = question.Instruction.Trim().Replace("{newline}",string.Empty);
            if (question.Mode == Mode.QUESTION)
            {
                audioRecorder.Delay = question.Delay;
                audioRecorder.RecordingTime = question.RecordingTime;
                audioRecorder.OutputFile = question.OutputFile;
                audioRecorder.PlayBeepSound = question.PlayBeep;
                audioRecorder.InitializeRecorder();

                audioCorrectAnswer.Visibility = Visibility.Hidden;
                audioUserAnswer.Visibility = Visibility.Hidden;
            }
            else if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
            {
                string[] correctAnswers = question.CorrectAnswers;
                //audioCorrectAnswer.Visibility = Visibility.Visible;
                audioCorrectAnswer.Media = MediaReader.GetMediaPath(correctAnswers[0] + ".mp3","SPK"); //TODO:Temporary


                audioUserAnswer.Visibility = Visibility.Visible;
                audioUserAnswer.Media = MediaReader.GetTempMediaPath(question.OutputFile + ".wav");

                audioUserAnswer.PlayStopClicked +=
                    new Common.SmallAudioPlayer.PlayStopEventHandler(audioUserAnswer_PlayStopClicked);
                audioCorrectAnswer.PlayStopClicked +=
                    new Common.SmallAudioPlayer.PlayStopEventHandler(audioCorrectAnswer_PlayStopClicked);

            }
            txtBlkDescription.Text = question.Description.Replace("{newline}",string.Empty);
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
    }
}
