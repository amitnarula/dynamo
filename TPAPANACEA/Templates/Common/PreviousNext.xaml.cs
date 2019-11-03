using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TPA.Entities;
using TPA.CoreFramework;
using WinForms = System.Windows.Forms;
using System.Windows.Threading;
using TPAPanacea.Templates.Common;
using System.Data;
using TPACORE.CoreFramework;
using System.IO;
using Newtonsoft.Json;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for PreviousNext.xaml
    /// </summary>
    public partial class PreviousNext : UserControl
    {
        private static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";
        private List<string> ModuleSequence = new List<string> { "WRITING", "READING", "SPEAKING", "LISTENING" };

        private DispatcherTimer timer;
        private DispatcherTimer delayTimer;
        private DispatcherTimer screenTimer;
        private string CurrentPracticeSetId { get; set; }
        private QuestionType CurrentQuestionType { get; set; }
        private bool IsPreviousQuestionSelected { get; set; }
        private int CurrentQuestionIndex { get; set; }
        private int TotalQuestionsCount { get; set; }
        private Mode QuestionMode { get; set; }
        private Mode SetMode { get; set; }
        private TestMode CurrentTestMode { get; set; }

        private int TimeSpent { get; set; }

        public QuestionBase QuestionContext { get; set; }
        public string UserAnswer { get; set; }
        public TimeSpan DelayTimer { get; set; }
        private bool IsTimeOut { get; set; }


        private TimeSpan AttemptTime { get; set; }
        private AttemptTimeType AttemptTimeType { get; set; }

        public string QuestionTemplateKey { get; set; }

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

            screenTimer = new DispatcherTimer();
            screenTimer.Interval = TimeSpan.FromSeconds(1);
            screenTimer.Tick += ScreenTimer_Tick;
            TimeSpent = 0;
            screenTimer.Start();    
        }

        private void ScreenTimer_Tick(object sender, EventArgs e)
        {
            TimeSpent++;
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

                object currentSender = (btnSubmit.Visibility == Visibility.Hidden) ? btnNext : btnSubmit; //Taking care of next/submit scenarios
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
            questionState.QuestionMode = this.QuestionMode;
            questionState.SetMode = this.SetMode;
            questionState.TestMode = this.CurrentTestMode;
            timer.Stop();
            screenTimer.Stop();
            

            //Next/Previous button click question button click raise event
            OnPrevNextClicked(e);
            if (btnSender.Name == "btnNext" || btnSender.Name == "btnSubmit" || btnSender.Name == "btnSaveAndExit")
            {
                LogUserScreenSpentTime();
                if (btnSender.Name == "btnSubmit")
                {
                    WinForms.DialogResult result;
                    if (this.CurrentTestMode == TestMode.Mock)
                    {
                        var nextModule = ResolveNextModule(questionState);
                        if (nextModule != QuestionType.NONE)//todo:wait time for next module
                            result = WinForms.MessageBox.Show(string.Format("Thanks for your attempt, Your next module {0} will start now.", nextModule));
                        else
                        {
                            this.CurrentTestMode = questionState.TestMode = TestMode.Practice; //reset test mode to practice
                            //Log the submission of whole mock set
                            result = WinForms.MessageBox.Show(string.Format("Thanks for your attempt, Your mock test will end now."));
                        }
                    }
                    else
                    {
                        result = WinForms.MessageBox.Show("Thanks for your attempt, Your answers are now saved, Press OK to go back to Home");
                    }

                    if (result == WinForms.DialogResult.OK)
                    {

                        //log the submission of the practice set item
                        string baseOutDir = System.IO.Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetUserFolder());
                        File.Create(System.IO.Path.Combine(baseOutDir, questionState.PracticeSetId + "_SUB_" + questionState.QuestionType.ToString() + ".xml"));

                        if (this.CurrentTestMode == TestMode.Mock) //mock mode, go back to next module
                        {
                            QuestionType nextQuestionType = ResolveNextModule(questionState);

                            Switcher.Switch(new QuestionSwitcher(), new Question()
                            {
                                PracticeSetId = this.CurrentPracticeSetId,
                                QuestionMode = QuestionMode,
                                QuestionType = nextQuestionType,
                                TestMode = CurrentTestMode,
                                SetMode = SetMode
                            });
                        }
                        else //practice mode go back to home
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
                        if (CurrentTestMode == TestMode.Mock)
                        {
                            TPACache.SetItem("MOCK" + CurrentPracticeSetId, new CurrentState()
                            {
                                QuestionIndex = CurrentQuestionIndex,
                                PracticeSetId = CurrentPracticeSetId,
                                QuestionType = CurrentQuestionType
                            }, new TimeSpan(0, 0, 5, 0, 0));
                        }
                        else
                        {
                            TPACache.SetItem(CurrentPracticeSetId + CurrentQuestionType.ToString(), new CurrentState()
                            {
                                QuestionIndex = CurrentQuestionIndex,
                                PracticeSetId = CurrentPracticeSetId,
                                QuestionType = CurrentQuestionType
                            }, new TimeSpan(0, 0, 5, 0, 0));
                        }

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

        private void LogUserScreenSpentTime()
        {
            if (LoginManager.CheckIfStudentLoggedIn() && this.QuestionMode == Mode.QUESTION && (this.CurrentTestMode == TestMode.Mock || this.CurrentTestMode == TestMode.Practice))
            {
                //log time spent by user here

                string targetUserFolder = CommonUtilities.ResolveTargetUserFolder();
                if (!string.IsNullOrEmpty(targetUserFolder))
                {
                    targetUserFolder = Path.Combine(baseOutputDirectory, targetUserFolder); //full path
                    string screenTimeTrackingFile = Path.Combine(targetUserFolder, "st.json");
                    if (!File.Exists(screenTimeTrackingFile))
                        //create screen time tracking file
                        //write screen time
                        File.WriteAllText(screenTimeTrackingFile, string.Empty);


                    //read existing file
                    var screenTimeTrackingInfo = JsonConvert.DeserializeObject<List<ScreenTime>>(File.ReadAllText(screenTimeTrackingFile));
                    if (screenTimeTrackingInfo == null)
                    {
                        screenTimeTrackingInfo = new List<ScreenTime>();
                    }

                    var existingTrackingInfo = screenTimeTrackingInfo.SingleOrDefault(x => x.PracticeSetId == this.CurrentPracticeSetId &&
                    x.QuestionId == this.QuestionContext.Id &&
                    x.QuestionTemplate == this.QuestionContext.QuestionTemplate &&
                        x.QuestionType == this.CurrentQuestionType.ToString());

                    if (existingTrackingInfo != null) //update existing record
                    {
                        existingTrackingInfo.TimeSpent = TimeSpan.FromSeconds(this.TimeSpent).ToString();
                    }
                    else // add new record
                    {
                        int maxPoints = ResolveMaxPoints();
                        
                        screenTimeTrackingInfo.Add(new ScreenTime()
                        {
                            PracticeSetId = this.CurrentPracticeSetId,
                            QuestionId = this.QuestionContext.Id,
                            PracticeSetName = "Some Practice set",
                            QuestionTemplate = this.QuestionContext.QuestionTemplate,
                            QuestionType = this.CurrentQuestionType.ToString(),
                            TimeSpent = TimeSpan.FromSeconds(this.TimeSpent).ToString(),
                            MaxScore = maxPoints.ToString()
                        });
                    }

                    //append screen time //write back
                    File.WriteAllText(screenTimeTrackingFile, JsonConvert.SerializeObject(screenTimeTrackingInfo));

                }
            }
        }

        private int ResolveMaxPoints()
        {
            int maxPoints = 0;

            if (CurrentQuestionType == QuestionType.SPEAKING
                        || CurrentQuestionType == QuestionType.WRITING
                        || QuestionContext.QuestionTemplate == QuestionTemplates.LISTEN_AND_WRITE.ToString()
                        && QuestionContext.QuestionTemplate != QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION.ToString())
            {
                //multi parameter question
                DataSet dsEvalParams = FileReader.ReadFile(FileReader.FileType.EVALUATION_PARAMETER);
                var questionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), QuestionContext.QuestionTemplate, true);
                List<ParameterizedScore> lstParameterizedScore = new List<ParameterizedScore>();
                maxPoints = EvaluationManager.PointsByType(dsEvalParams, questionTemplate, ref lstParameterizedScore,0, "");
            }
            else
            {
                maxPoints = QuestionContext.CorrectAnswers.Count();
                //Write from dictation, listen and dictate has different point calculation mechanism

                if (QuestionContext.QuestionTemplate == QuestionTemplates.LISTEN_AND_DICTATE.ToString())
                {
                    var correctAnswser = QuestionContext.CorrectAnswers.SingleOrDefault();

                    if (correctAnswser != null)
                        maxPoints = correctAnswser.Split(' ').Count();
                }

                if (QuestionContext.QuestionTemplate == QuestionTemplates.REORDER.ToString())
                {
                    maxPoints = maxPoints - 1;//bug fix, its not the number of paragraphs, its about number of pairs
                    //number of correct pairs always 1 less than number of paragraphs in REORDER
                }
            }
            return maxPoints;
        }

        private QuestionType ResolveNextModule(Question questionState)
        {
            var nextQuestionType = QuestionType.SPEAKING;

            switch (questionState.QuestionType)
            {
                case QuestionType.READING:
                    nextQuestionType = QuestionType.LISTENING;
                    break;
                case QuestionType.SPEAKING:
                    nextQuestionType = QuestionType.WRITING;
                    break;
                case QuestionType.LISTENING:
                    nextQuestionType = QuestionType.NONE;
                    break;
                case QuestionType.WRITING:
                    nextQuestionType = QuestionType.READING;
                    break;
                default:
                    //break the sequence here
                    nextQuestionType = QuestionType.NONE;
                    break;
            }

            return nextQuestionType;
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
                this.QuestionMode = QuestionContext.Mode;
                this.SetMode = QuestionContext.SetMode;
                this.CurrentTestMode = QuestionContext.TestMode;
                this.AttemptTime = QuestionContext.AttemptTime;
                this.AttemptTimeType = QuestionContext.AttemptTimeType;
                this.DelayTimer = QuestionContext.AttemptTimeType == AttemptTimeType.INDIVIDUAL_QUESTION ?
                    QuestionContext.DelayTime : TimeSpan.Zero;

                btnYourResponse.Visibility = Visibility.Hidden;
                btnEvaluate.Visibility = Visibility.Hidden;
                if (CurrentTestMode == TestMode.Mock && this.QuestionMode == Mode.QUESTION)
                {
                    btnSaveAndExit.Margin = btnPrevious.Margin;
                    btnPrevious.Visibility = Visibility.Collapsed; //previous button visiblity and margin fix for save and exit button
                }

                if (this.CurrentQuestionIndex == 0)
                {
                    btnPrevious.IsEnabled = false;
                }
                else if (this.CurrentQuestionIndex == this.TotalQuestionsCount - 1)
                {
                    btnNext.IsEnabled = false;
                    btnNext.Visibility = Visibility.Hidden;
                    btnSubmit.Visibility = Visibility.Visible;

                    if (this.QuestionMode == Mode.ANSWER_KEY || this.QuestionMode == Mode.TIME_OUT)
                    {
                        btnSubmit.IsEnabled = false; //Submit should be disabled when its an answer key or timeout mode
                    }
                    if(CurrentTestMode == TestMode.Mock && QuestionMode == Mode.ANSWER_KEY)
                    {
                        btnSubmit.IsEnabled = true; // submit button enabled when test is running in mock mode, because next module load is required.
                    }
                }


                ////TIMEOUT CASES
                //if (CheckIfEvaluatorLoggedIn() && this.CurrentMode == Mode.TIME_OUT)
                //{
                //    btnEvaluate.Visibility = Visibility.Visible;
                //}
                //else
                //    btnEvaluate.Visibility = Visibility.Hidden;

                if (this.QuestionMode == Mode.ANSWER_KEY)
                {
                    btnSaveAndExit.IsEnabled = false; //Save&Exit should be disabled

                    if (QuestionContext.CurrentQuestionType != QuestionType.SPEAKING) //Your response button is not available
                        btnYourResponse.Visibility = Visibility.Visible;



                    if (CheckIfEvaluatorLoggedIn())
                        btnEvaluate.Visibility = Visibility.Visible;
                    else
                    {
                        btnEvaluate.Visibility = Visibility.Hidden;
                        BindObtainedPointData();

                    }

                }

                if (this.QuestionMode == Mode.TIME_OUT)
                {
                    btnYourResponse.Visibility = Visibility.Visible; // if timeout still user answer and correct answer should be shown
                    if (CheckIfEvaluatorLoggedIn())
                        btnEvaluate.Visibility = Visibility.Visible; // if timeout , still it should be evaluated as per teacher logged in status
                    IsTimeOut = true; // set is time out true if question mode is already timeout mode
                }
                
                
                lblTimer.Content = "Time left : " + AttemptTime.ToString();

                if (this.AttemptTime != TimeSpan.Zero && QuestionMode == Mode.QUESTION) //Some value for timestamp exists
                {
                    delayTimer.Start();
                }
                else if (IsTimeOut)
                    lblTimer.Content = "Timeout";
                else
                    lblTimer.Visibility = Visibility.Hidden; //Don't show the timer at all

                lblItemCount.Content = string.Format("Question {0} of {1}", CurrentQuestionIndex + 1, TotalQuestionsCount);


                //TODO : refactor in a better way with this.setMode / patch work
                if (this.QuestionMode == Mode.TIME_OUT && this.CurrentTestMode ==TestMode.Mock) {

                    if (this.SetMode == Mode.QUESTION) {
                        btnSaveAndExit.Margin = btnPrevious.Margin;
                        btnPrevious.Visibility = Visibility.Collapsed;
                        btnYourResponse.Visibility = Visibility.Collapsed;
                        btnSubmit.IsEnabled = true;


                    }
                    else if (this.SetMode == Mode.ANSWER_KEY) {
                        btnYourResponse.Visibility = Visibility.Visible;
                        btnPrevious.Visibility = Visibility.Visible;
                        btnSaveAndExit.IsEnabled = false;
                        btnSubmit.IsEnabled = true;
                        if (this.CheckIfEvaluatorLoggedIn()) {
                            btnEvaluate.Visibility = Visibility.Visible;
                        }
                    }
                }

                //TIMEOUT CASES with obtained and maximum points

                if (IsTimeOut)
                {
                    lblTimer.Content = string.Empty;
                    BindObtainedPointData();
                }
                
            }

        }

        private void BindObtainedPointData()
        {
            lblPoints.Visibility = Visibility.Visible;
            
            var result = EvaluationManager.GetResult(QuestionContext);

            //TODO : refactor in a better way with this.setMode / patch work
            if (result == null && IsTimeOut) {
                if (this.SetMode == Mode.QUESTION) {
                    lblPoints.Content = "Timeout";
                }
                else if (this.SetMode == Mode.ANSWER_KEY) {
                    lblPoints.Content = "Not evaluated (Timeout)";
                }
                return;
            }

            int maximumPoints = QuestionContext.CorrectAnswers.Count();
            //Write from dictation, listen and dictate has different point calculation mechanism

            if (QuestionContext.QuestionTemplate == QuestionTemplates.LISTEN_AND_DICTATE.ToString())
            {
                var correctAnswser = QuestionContext.CorrectAnswers.SingleOrDefault();

                if (correctAnswser != null)
                    maximumPoints = correctAnswser.Split(' ').Count();
            }

            if (QuestionContext.QuestionTemplate == QuestionTemplates.REORDER.ToString())
            {
                maximumPoints = maximumPoints - 1;//bug fix, its not the number of paragraphs, its about number of pairs
                //number of correct pairs always 1 less than number of paragraphs in REORDER
            }

            if(result!=null && result.Any(x=>x.ParamName =="SINGLE")){

            lblPoints.Content = string.Format("Maximum Points : {0}      Points Obtained: {1} {2}", maximumPoints,
                (result != null && result.Any() ? result.First().ParamScore : "0"), IsTimeOut ? "(Timeout)" : string.Empty);
            }
            else{ //multi parameter display of max and obtained points
                if (result != null)
                {
                    int max = result.Sum(x => Convert.ToInt16(x.ParamMaxScore));
                    int obt = result.Sum(x => Convert.ToInt16(x.ParamScore));

                    lblPoints.Content = string.Format("Maximum Points : {0}      Points Obtained: {1} {2}", max,
                    obt, IsTimeOut ? "(Timeout)" : string.Empty);
                }
                else {
                    lblPoints.Content = "Not evaluated";
                }
            }
            
        }

        private bool CheckIfEvaluatorLoggedIn()
        {
            var loginStatus = TPACache.GetItem(TPACache.LOGIN_KEY);

            return (CurrentQuestionType == QuestionType.SPEAKING
                                    || CurrentQuestionType == QuestionType.WRITING
                                    || QuestionContext.QuestionTemplate == QuestionTemplates.LISTEN_AND_WRITE.ToString()
                                    && QuestionContext.QuestionTemplate != QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION.ToString())
                                    && loginStatus != null;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            delayTimer.Stop();
            timer.Stop();
            screenTimer.Stop();
        }

        private void btnYourResponse_Click(object sender, RoutedEventArgs e)
        {
            bool isValidToClickYourResponseButton = false;

            if(LoginManager.CheckIfStudentLoggedIn())
            {
                isValidToClickYourResponseButton = true;
            }
            else
            {
                if (LoginManager.CheckIfTeacherLoggedIn())
                {
                    if (LoginManager.CheckIfStudentToEvaluateSet())
                    {
                        isValidToClickYourResponseButton = true;
                    }
                    else
                        isValidToClickYourResponseButton = false;
                }
                else
                {
                    isValidToClickYourResponseButton = false;
                }
            }

            if (!isValidToClickYourResponseButton)
            {
                WinForms.MessageBox.Show("You are not allowed to perform this action until unless you are logged in or set a student for evaluation");
                return;
            }

            YourResponseEventArgs eventArgs = new YourResponseEventArgs();

            if (btnYourResponse.Content.ToString().Equals("Your Response"))
            {
                btnYourResponse.Content = "Correct Answer";
                if (!QuestionContext.UserAnswers.Any())
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
            if (!LoginManager.CheckIfStudentToEvaluateSet())
            {
                EvaluatingStudent studentSetDialog = new EvaluatingStudent();
                if (!studentSetDialog.ShowDialog().Value)
                    return;
            }

            Evaluate evaluate = new Evaluate();
            evaluate.QuestionContext = QuestionContext;
            DataSet ds = FileReader.ReadFile(FileReader.FileType.EVALUATION_PARAMETER);
            DataRow[] parameters = ds.Tables["template"].Select("key='" + QuestionContext.QuestionTemplate + "'");

            DataRow drowParam = parameters[0];
            if (drowParam != null)
            {
                var evalParams = Convert.ToString(drowParam["params"]).Split('|');
                evaluate.Parameters = new List<TPACORE.Entities.EvaluationParameter>();
                foreach (var prm in evalParams)
                {
                    var evalParam = prm.Split(',');
                    evaluate.Parameters.Add(new TPACORE.Entities.EvaluationParameter() {
                        Max = evalParam[3],
                        Min = evalParam[2],
                        Name = evalParam[0],
                        Type = evalParam[1]
                    });
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
