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
using TPA.Entities;
using TPA.CoreFramework;
using WinForms = System.Windows.Forms;
using System.Windows.Threading;
using TPAPanacea.Templates.Common;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for PreviousNext.xaml
    /// </summary>
    public partial class PreviousNext : UserControl
    {
        private DispatcherTimer timer;
        private DispatcherTimer delayTimer;
        private string CurrentPracticeSetId { get; set; }
        private QuestionType CurrentQuestionType { get; set; }
        private bool IsPreviousQuestionSelected{get;set;}
        private int CurrentQuestionIndex { get; set; }
        private int TotalQuestionsCount { get; set; }
        private Mode CurrentMode { get; set; }
        public QuestionBase QuestionContext { get; set; }
        public string UserAnswer { get; set; }
        public TimeSpan DelayTimer { get; set; }
        private bool IsTimeOut { get; set; }
        
        
        private TimeSpan AttemptTime { get; set; }
        private AttemptTimeType AttemptTimeType { get; set; }
        
        public delegate void PrevNextEventHandler(object sender, EventArgs e);
        public event PrevNextEventHandler PrevNextClicked;

        public delegate void TimeOutEventHandler(object sender, EventArgs e);
        public event TimeOutEventHandler TimeOut;

        public delegate void YourResponseEventHandler(object sender, YourResponseEventArgs e);
        public event YourResponseEventHandler YourResponseClicked;

        public PreviousNext()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);

            delayTimer = new DispatcherTimer();
            delayTimer.Interval = TimeSpan.FromSeconds(1);
            delayTimer.Tick += new EventHandler(delayTimer_Tick);

        }

        void delayTimer_Tick(object sender, EventArgs e)
        {
            if (DelayTimer == TimeSpan.Zero)
            {
                timer.Start(); //Start the timer immediately and stop the delay timer
                delayTimer.Stop();
            }
            else
            {
                TimeSpan timeLeft = DelayTimer.Subtract(TimeSpan.FromSeconds(1));

                DelayTimer = timeLeft;

                if (timeLeft == TimeSpan.Zero)
                {
                    delayTimer.Stop();
                    timer.Start();
                }
            }
        }

        public string GetAttemptTimeLeft()
        {
            if (IsTimeOut)
                return "TIME_OUT";
            else
                return AttemptTime.ToString();
        }
        public void SwitchToEvaluationMode()
        {
            btnEvaluate.Visibility = Visibility.Visible;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeLeft = AttemptTime.Subtract(TimeSpan.FromSeconds(1));
            //Tick event is handled here
            lblTimer.Content = "Time left : " + timeLeft.ToString();
            AttemptTime = timeLeft;

            if (timeLeft == TimeSpan.Zero)
            {
                WinForms.MessageBox.Show("Timeout, Please press OK for next question");

                object currentSender = (btnSubmit.Visibility==Visibility.Hidden) ? btnNext : btnSubmit; //Taking care of next/submit scenarios
                IsTimeOut = true;
                btnNextPrevious_Click(currentSender, null);

                if (TimeOut != null)
                    TimeOut(this, e); //Raise the timeout event so that other parent controls may handle it
            }

        }

        private void btnNextPrevious_Click(object sender, RoutedEventArgs e)
        {
            Button btnSender = sender as Button;
            Entities.Question questionState = new Entities.Question();
            questionState.PracticeSetId = this.CurrentPracticeSetId;
            questionState.QuestionType = this.CurrentQuestionType;
            questionState.QuestionMode = this.CurrentMode;
            timer.Stop();

            //Next/Previous button click question button click raise event
            OnPrevNextClicked(e);
            if(btnSender.Name=="btnNext" || btnSender.Name=="btnSubmit" || btnSender.Name == "btnSaveAndExit")
            {
                if (btnSender.Name == "btnSubmit")
                {
                    WinForms.DialogResult result= WinForms.MessageBox.Show("Thanks for your attempt, Your answers are now saved, Press OK to go back to Home");
                    if (result == WinForms.DialogResult.OK)
                    {
                        Switcher.Switch(new HomePanacia());
                        //Switcher.Switch(new Home());
                        return;
                    }
                    
                }
                else if (btnSender.Name == "btnSaveAndExit")
                {
                    WinForms.DialogResult result = WinForms.MessageBox.Show("Please press OK to save and exit");
                    if (result == WinForms.DialogResult.OK)
                    {
                        //Save state here
                        TPACache.SetItem(CurrentPracticeSetId+CurrentQuestionType.ToString(), new CurrentState()
                        {
                            QuestionIndex = CurrentQuestionIndex,
                            PracticeSetId = CurrentPracticeSetId,
                            QuestionType = CurrentQuestionType
                        }, new TimeSpan(0, 0, 5, 0, 0));

                        //Switcher.Switch(new Home());
                        Switcher.Switch(new HomePanacia());
                        return;
                    }
                }
                
                questionState.IsNextQuestionSelected = true;
                questionState.IsPreviousQuestionSelected = false;
            
            }
            else
            {
                questionState.IsNextQuestionSelected = false;
                questionState.IsPreviousQuestionSelected = true;

            }
            Switcher.Switch(new QuestionSwitcher(), questionState);
        }

        protected virtual void OnPrevNextClicked(EventArgs e)
        {
            if (PrevNextClicked != null)
                PrevNextClicked(this, e);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (QuestionContext != null)
            {
                this.CurrentQuestionIndex = QuestionContext.QuestionIndex;
                this.TotalQuestionsCount = QuestionContext.TotalQuestionsCount;
                this.CurrentQuestionType = QuestionContext.CurrentQuestionType;
                this.CurrentPracticeSetId = QuestionContext.CurrentPracticeSetId;
                this.CurrentMode = QuestionContext.Mode;
                this.AttemptTime = QuestionContext.AttemptTime;
                this.AttemptTimeType = QuestionContext.AttemptTimeType;
                this.DelayTimer = QuestionContext.AttemptTimeType == AttemptTimeType.INDIVIDUAL_QUESTION ?
                    QuestionContext.DelayTime : TimeSpan.Zero;
                
                btnYourResponse.Visibility = Visibility.Hidden;
                btnEvaluate.Visibility = Visibility.Hidden;

                if (this.CurrentQuestionIndex == 0)
                {
                    btnPrevious.IsEnabled = false;
                }
                else if (this.CurrentQuestionIndex == this.TotalQuestionsCount - 1)
                {
                    btnNext.IsEnabled = false;
                    btnNext.Visibility = Visibility.Hidden;
                    btnSubmit.Visibility = Visibility.Visible;

                    if (this.CurrentMode == Mode.ANSWER_KEY || this.CurrentMode==Mode.TIME_OUT)
                    {
                        btnSubmit.IsEnabled = false; //Submit should be disabled when its an answer key or timeout mode
                    }
                }

                if (this.CurrentMode == Mode.ANSWER_KEY)
                {
                    btnSaveAndExit.IsEnabled = false; //Save&Exit should be disabled

                    if (QuestionContext.CurrentQuestionType != QuestionType.SPEAKING) //Your response button is not available
                        btnYourResponse.Visibility = Visibility.Visible;

                    ///var loginStatus = TPACache.GetItem(TPACache.LOGIN_KEY);
                    ///if (loginStatus != null && (CurrentQuestionType == QuestionType.SPEAKING || CurrentQuestionType == QuestionType.WRITING))
                        btnEvaluate.Visibility = Visibility.Visible;

                }

                if (this.CurrentMode == Mode.TIME_OUT)
                    IsTimeOut = true; // set is time out true if question mode is already timeout mode

                lblTimer.Content = "Time left : " + AttemptTime.ToString();
                
                if (this.AttemptTime != TimeSpan.Zero && CurrentMode == Mode.QUESTION) //Some value for timestamp exists
                {
                    delayTimer.Start();
                }
                else if (IsTimeOut)
                    lblTimer.Content = "Timeout";
                else
                    lblTimer.Visibility = Visibility.Hidden; //Don't show the timer at all

                lblItemCount.Content = string.Format("Question {0} of {1}", CurrentQuestionIndex + 1, TotalQuestionsCount);
            }

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            delayTimer.Stop();
            timer.Stop();
        }

        private void btnYourResponse_Click(object sender, RoutedEventArgs e)
        {
            YourResponseEventArgs eventArgs = new YourResponseEventArgs();

            if (btnYourResponse.Content.ToString().Equals("Your Response"))
            {
                btnYourResponse.Content = "Correct Answer";
                if(!QuestionContext.UserAnswers.Any())
                    WinForms.MessageBox.Show("Looks like you didn't attempt this question.");
                eventArgs.ShowYourAnswer = true;
            }
            else
            {
                btnYourResponse.Content = "Your Response";
                if (!QuestionContext.CorrectAnswers.Any() ||
                    (QuestionContext.CorrectAnswers.Count() == 1 &&
                    string.IsNullOrEmpty(QuestionContext.CorrectAnswers.First())))
                {
                    WinForms.MessageBox.Show("The answer key for this question is not available.");
                }

                eventArgs.ShowYourAnswer = false;
            }

            if (YourResponseClicked != null)
                YourResponseClicked(this, eventArgs);
        }

        private void btnEvaluate_Click(object sender, RoutedEventArgs e)
        {
            Evaluate evaluate = new Evaluate();
            evaluate.QuestionId = QuestionContext.Id;
            evaluate.Parameters = new List<TPACORE.Entities.EvaluationParameter>() {
                new TPACORE.Entities.EvaluationParameter() {
                    Max ="5",
                    Min ="0",
                    Type="int",
                    Name="Content"
                },
                new TPACORE.Entities.EvaluationParameter() {
                    Max ="5",
                    Min ="0",
                    Type="int",
                    Name="Form"
                },
                new TPACORE.Entities.EvaluationParameter() {
                    Max ="5",
                    Min ="0",
                    Type="int",
                    Name="Vocabulary"
                }
            };
            evaluate.ShowDialog();
        }
    }

    public class YourResponseEventArgs : EventArgs
    {
        public bool ShowYourAnswer { get; set; }
    }
}
