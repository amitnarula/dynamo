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
using TPA.Templates.Reading;
using System.Data.Linq;
using TPA.Templates.Writing;
using TPA.Templates.Listening;
using TPA.Templates.Speaking;


namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for Question.xaml
    /// </summary>
    public partial class QuestionSwitcher : UserControl,ISwitchable
    {

        private void ProcessQuestion(DataSet dsQuestions, string practiceSetId, bool isPreviousQuestionSelected, bool isNextQuestionSelected, Mode questionMode, QuestionType questionType, TestMode testMode, Mode setMode)
        {
            object currentQuestion = TPACache.GetItem("currentQuestionIndex");
            CurrentState currentState = TPACache.GetItem(practiceSetId+Convert.ToString(questionType)) as CurrentState;
            int currentQuestionIndex = 0;
            if (currentQuestion == null)
            {
                currentQuestionIndex = 0;
            }
            else
            {
                currentQuestionIndex = Convert.ToInt32(currentQuestion);
            }

            if (isPreviousQuestionSelected && currentQuestionIndex >= 0)
                currentQuestionIndex = currentQuestionIndex - 1;
            else if (isNextQuestionSelected)
                currentQuestionIndex = currentQuestionIndex + 1;
            else //When no one is selected, this case arises, if user chooses to continue the test from previous stored state
            {
                if (currentState != null && questionMode != Mode.ANSWER_KEY)
                    currentQuestionIndex = currentState.QuestionIndex;
                else
                    currentQuestionIndex = 0; //If nothing is found, restart the test from beginning
            }

            if(testMode == TestMode.Mock)
            {
                //check save and exit state
                var testModeStateForMockTest = TPACache.GetItem("MOCK" + practiceSetId) as CurrentState;
                if (testModeStateForMockTest != null)
                {
                    currentQuestionIndex = testModeStateForMockTest.QuestionIndex;
                    //now remove the state as it has already been consumed to navigate to required question
                    //so no longer needed
                    TPACache.RemoveItem("MOCK" + practiceSetId);
                }
            }

            TPACache.SetItem("currentQuestionIndex", currentQuestionIndex, new TimeSpan(1,0,0));

            DataRow[] questionsDataRows = dsQuestions.Tables["question"].Select("practiceSet='" + practiceSetId+"'");

            if (questionsDataRows == null || !questionsDataRows.Any())
            {
                System.Windows.Forms.MessageBox.Show("No questions found in this practice set");
                return;
            }

            DataRow questionDataRow = questionsDataRows[currentQuestionIndex];
            int tbl_question_Id = Convert.ToInt32(questionDataRow["question_Id"]);
            DataRow questionTemplateRow = dsQuestions.Tables["template"].Select("question_Id=" + tbl_question_Id).FirstOrDefault();
            int tbl_template_Id = questionTemplateRow.Table.Columns.Contains("template_Id") ?
                Convert.ToInt32(questionTemplateRow["template_Id"]) : 0;
            

            string title = Convert.ToString(questionTemplateRow["title"]);
            string description = Convert.ToString(questionTemplateRow["description"]);
            string[] correctAnswers = Convert.ToString(questionTemplateRow["answer"]).Split(new char[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
            string questionId = Convert.ToString(questionDataRow["id"]);
            string questionInstruction = Convert.ToString(questionDataRow["instruction"]);
            TimeSpan attemptTime = TimeSpan.Parse(Convert.ToString(questionDataRow["attemptTime"]));
            AttemptTimeType attemptTimeType = AttemptTimeType.INDIVIDUAL_QUESTION;

            Answer userAnswer = AnswerManager.ReadAnswer(practiceSetId, questionId);

            var practiceSetAttemptTime = FileReader.ResolvePracticeSetAttemptTimeLeft(practiceSetId,questionType.ToString(),questionsDataRows.Count());

            string[] userAnswers = new string[] { };
            if (userAnswer != null)
            {
                userAnswers = userAnswer.Answers;
                //if attempt time left for the question, then set attempt time from the answer
                if (userAnswer.AttemptTime == "TIME_OUT") //if time out
                {
                    questionMode = Mode.TIME_OUT;
                }
                else
                {
                    //Reset the left attempt time here from the answer where user left it
                    attemptTime = TimeSpan.Parse(userAnswer.AttemptTime);

                    if (questionMode == Mode.TIME_OUT)
                        questionMode = Mode.QUESTION; //Reset the mode to question mode
                }
            }

            if (practiceSetAttemptTime != null) //Overall timer
            {
                attemptTimeType = AttemptTimeType.PRACTICE_SET_ITEM;

                if (practiceSetAttemptTime.AttemptTime == "TIME_OUT")
                {
                    questionMode = Mode.TIME_OUT;
                }
                else
                {
                    //overall attempt time should not be applied in case of listen and write type of questions
                    //as they have individual time and rest of the listening module has overall time
                    if (!Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_AND_WRITE.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        attemptTime = TimeSpan.Parse(practiceSetAttemptTime.AttemptTime); //Attempt time left

                    if (questionMode == Mode.TIME_OUT)
                        questionMode = Mode.QUESTION; //Reset the mode to question mode
                }
            }

            TimeSpan delayTime = TimeSpan.FromSeconds(0); //Delay time before attempt time timer starts

            if (questionDataRow.Table.Columns.Contains("delayTime"))
            {
                delayTime = TimeSpan.Parse(Convert.ToString(questionDataRow["delayTime"]));
            }

            string questionTemplate = Convert.ToString(questionDataRow["type"]);

            if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER.ToString(), StringComparison.InvariantCultureIgnoreCase))
            { 
                //Multiple Choice Single Answer question
                MultiChoiceSingleAnswerQuestion question = new MultiChoiceSingleAnswerQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Picture = Convert.ToString(questionTemplateRow["picture"]);
                question.QuestionTemplate = questionTemplate;

                DataRow questionOptionRow = dsQuestions.Tables["options"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();

                List<Option> lstOptions = new List<Option>();
                for (int count = 0; count < questionOptionRow.ItemArray.Count()-1; count++)
                {
                    if (!string.IsNullOrEmpty(questionOptionRow.ItemArray[count].ToString()))
                    {
                        Option option = new Option();
                        option.Id = count.ToString();
                        option.OptionText = questionOptionRow.ItemArray[count].ToString();
                        lstOptions.Add(option);
                    }
                }

                question.Options = lstOptions;
                question.CurrentQuestionType = QuestionType.READING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new MultiChoiceSingleAnswer(), question);
            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Multiple Choice Single Answer question
                MultiChoiceMultiAnswerQuestion question = new MultiChoiceMultiAnswerQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Picture = Convert.ToString(questionTemplateRow["picture"]);
                question.QuestionTemplate = questionTemplate;


                DataRow questionOptionRow = dsQuestions.Tables["options"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();

                List<Option> lstOptions = new List<Option>();
                for (int count = 0; count < questionOptionRow.ItemArray.Count() - 1; count++)
                {
                    if (!string.IsNullOrEmpty(questionOptionRow.ItemArray[count].ToString()))
                    {
                        Option option = new Option();
                        option.Id = count.ToString();
                        option.OptionText = questionOptionRow.ItemArray[count].ToString();
                        lstOptions.Add(option);
                    }
                }

                question.Options = lstOptions;
                question.CurrentQuestionType = QuestionType.READING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new MultiChoiceMultiAnswer(), question);
            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.REORDER.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Multiple Choice Single Answer question
                ReorderQuestion question = new ReorderQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.QuestionTemplate = questionTemplate;


                DataRow questionOptionRow = dsQuestions.Tables["options"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();

                List<Option> lstOptions = new List<Option>();
                for (int count = 0; count < questionOptionRow.ItemArray.Count() - 1; count++)
                {
                    if (!string.IsNullOrEmpty(questionOptionRow.ItemArray[count].ToString()))
                    {
                        Option option = new Option();
                        option.Id = count.ToString();
                        option.OptionText = questionOptionRow.ItemArray[count].ToString();
                        lstOptions.Add(option);
                    }
                }

                question.Options = lstOptions;
                question.CurrentQuestionType = QuestionType.READING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                
                Switcher.Switch(new Reorder(),question);
            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                FillInTheBlanksWithOptionsQuestion question = new FillInTheBlanksWithOptionsQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.QuestionTemplate = questionTemplate;

                
                DataRow questionOptionRow = dsQuestions.Tables["blanks"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();
                int blanks_Id = Convert.ToInt32(questionOptionRow["blanks_Id"]);

                DataRow[] blanksDataOptionRows = dsQuestions.Tables["blank"].Select("blanks_Id=" + blanks_Id);

                List<List<Option>> listOfBlanksAndOptions = new List<List<Option>>();
                for (int count = 0; count < blanksDataOptionRows.Count(); count++)
                {
                    List<Option> lstOptions = new List<Option>();
                    for (int innerCount = 0; innerCount < blanksDataOptionRows[count].ItemArray.Count()-1; innerCount++)
                    {
                        if (!string.IsNullOrEmpty(blanksDataOptionRows[count].ItemArray[innerCount].ToString()))
                        {
                            Option option = new Option();
                            option.Id = innerCount.ToString();
                            option.OptionText = blanksDataOptionRows[count].ItemArray[innerCount].ToString();
                            lstOptions.Add(option);
                        }   
                    }
                    listOfBlanksAndOptions.Add(lstOptions);

                    
                }

                question.Options = listOfBlanksAndOptions;
                question.CurrentQuestionType = QuestionType.READING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new FillInBlanksWithOptions(), question);
            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.FILL_IN_BLANKS.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                FillInTheBlanksQuestion question = new FillInTheBlanksQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.QuestionTemplate = questionTemplate;


                DataRow questionOptionRow = dsQuestions.Tables["blanks"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();
                int blanks_Id = Convert.ToInt32(questionOptionRow["blanks_Id"]);

                DataRow[] blanksDataOptionRows = dsQuestions.Tables["blank"].Select("blanks_Id=" + blanks_Id);

                List<Option> lstOptions = new List<Option>();
                for (int count = 0; count < blanksDataOptionRows.Count(); count++)
                {
                    for (int innerCount = 0; innerCount < blanksDataOptionRows[count].ItemArray.Count() - 1; innerCount++)
                    {
                        if (!string.IsNullOrEmpty(blanksDataOptionRows[count].ItemArray[innerCount].ToString()))
                        {
                            Option option = new Option();
                            option.Id = innerCount.ToString();
                            option.OptionText = blanksDataOptionRows[count].ItemArray[innerCount].ToString();
                            lstOptions.Add(option);
                        }
                    }
                    
                }

                question.Options = lstOptions;
                question.CurrentQuestionType = QuestionType.READING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new FillInBlanks(), question);
            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.SUMMARIZE_TEXT.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                SummarizeTextQuestion question = new SummarizeTextQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.CurrentQuestionType = QuestionType.WRITING;
                question.CurrentPracticeSetId = practiceSetId;
                question.MaxWordCount = Convert.ToInt32(questionTemplateRow["maxWordCount"]);
                question.DelayTime = delayTime;
                question.QuestionTemplate = questionTemplate;



                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new SummarizeText(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.WRITE_ESSAY.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                WriteEssayQuestion question = new WriteEssayQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.CurrentQuestionType = QuestionType.WRITING;
                question.CurrentPracticeSetId = practiceSetId;
                question.MaxWordCount = Convert.ToInt32(questionTemplateRow["maxWordCount"]);
                question.DelayTime = delayTime;
                question.QuestionTemplate = questionTemplate;


                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new WriteEssay(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_MULTI_CHOICE.ToString(), StringComparison.InvariantCultureIgnoreCase)
                || Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY.ToString(), StringComparison.InvariantCultureIgnoreCase)
                || Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_SELECT_MISSING_WORD.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                ListenMultiChoiceQuestion question = new ListenMultiChoiceQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["media"]));
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.QuestionTemplate = questionTemplate;


                DataRow questionOptionRow = dsQuestions.Tables["options"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();

                List<Option> lstOptions = new List<Option>();
                for (int count = 0; count < questionOptionRow.ItemArray.Count() - 1; count++)
                {
                    if (!string.IsNullOrEmpty(questionOptionRow.ItemArray[count].ToString()))
                    {
                        Option option = new Option();
                        option.Id = count.ToString();
                        option.OptionText = questionOptionRow.ItemArray[count].ToString();
                        lstOptions.Add(option);
                    }
                }

                question.Options = lstOptions;
                question.CurrentQuestionType = QuestionType.LISTENING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ListenAndMultiChoice(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_MULTI_SELECT.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                ListenMultiSelectQuestion question = new ListenMultiSelectQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["media"]));
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.QuestionTemplate = questionTemplate;

                DataRow questionOptionRow = dsQuestions.Tables["options"].Select("template_Id=" + tbl_template_Id).FirstOrDefault();

                List<Option> lstOptions = new List<Option>();
                for (int count = 0; count < questionOptionRow.ItemArray.Count() - 1; count++)
                {
                    if (!string.IsNullOrEmpty(questionOptionRow.ItemArray[count].ToString()))
                    {
                        Option option = new Option();
                        option.Id = count.ToString();
                        option.OptionText = questionOptionRow.ItemArray[count].ToString();
                        lstOptions.Add(option);
                    }
                }

                question.Options = lstOptions;

                question.CurrentQuestionType = QuestionType.LISTENING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ListenAndMultiSelect(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_AND_WRITE.ToString(), StringComparison.InvariantCultureIgnoreCase)
                || Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_AND_DICTATE.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                ListenAndWriteQuestion question = new ListenAndWriteQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.MaxWordCount = Convert.ToInt32(questionTemplateRow["maxWordCount"]);
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["media"]));
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.QuestionTemplate = questionTemplate;

                question.CurrentQuestionType = QuestionType.LISTENING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ListenAndWrite(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_AND_HIGHLIGHT.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                ListenAndHighlightQuestion question = new ListenAndHighlightQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["media"]));
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.QuestionTemplate = questionTemplate;

                question.CurrentQuestionType = QuestionType.LISTENING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ListenAndHighlight(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LISTEN_AND_FILL_BLANKS.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                ListenAndFillInBlanksQuestion question = new ListenAndFillInBlanksQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["media"]));
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.QuestionTemplate = questionTemplate;

                question.CurrentQuestionType = QuestionType.LISTENING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ListenAndFillBlanks(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.SPEAK_READ.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                SpeakReadQuestion question = new SpeakReadQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.QuestionTemplate = questionTemplate;

                question.RecordingTime = Convert.ToInt32(questionTemplateRow["recordingTime"]);
                question.PlayBeep = Convert.ToBoolean(questionTemplateRow["beep"]);
                
                question.OutputFile = Convert.ToString(questionTemplateRow["output"]);
                question.CurrentQuestionType = QuestionType.SPEAKING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ReadAndSpeak(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.SPEAK_LOOK.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                SpeakLookQuestion question = new SpeakLookQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.RecordingTime = Convert.ToInt32(questionTemplateRow["recordingTime"]);
                question.PlayBeep = Convert.ToBoolean(questionTemplateRow["beep"]);
                question.OutputFile = Convert.ToString(questionTemplateRow["output"]);
                question.Picture = Convert.ToString(questionTemplateRow["picture"]);
                question.QuestionTemplate = questionTemplate;

                question.CurrentQuestionType = QuestionType.SPEAKING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new LookAndSpeak(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.SPEAK_LISTEN.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                SpeakListenQuestion question = new SpeakListenQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.RecordingTime = Convert.ToInt32(questionTemplateRow["recordingTime"]);
                question.PlayBeep = Convert.ToBoolean(questionTemplateRow["beep"]);
                question.OutputFile = Convert.ToString(questionTemplateRow["output"]);
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["audio"]),"SPK");//TODO Temporary
                question.AudioDelay = Convert.ToInt32(questionTemplateRow["audioDelay"]);
                question.QuestionTemplate = questionTemplate;

                string transcript = Convert.ToString(questionTemplateRow["transcript"]);

                question.ShowTranscript = !string.IsNullOrEmpty(transcript);
                question.SampleTranscript = transcript;


                question.CurrentQuestionType = QuestionType.SPEAKING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new ListenAndSpeak(), question);

            }
            else if (Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.LOOK_SPEAK_LISTEN.ToString(), StringComparison.InvariantCultureIgnoreCase)
                || Convert.ToString(questionDataRow["type"]).Equals(QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //Summarize text answer question
                LookSpeakListenQuestion question = new LookSpeakListenQuestion();
                question.Id = questionId;
                question.Title = title;
                question.Description = description;
                question.CorrectAnswers = correctAnswers;
                question.Mode = questionMode;
                question.TestMode = testMode;
                question.SetMode = setMode;
                question.UserAnswers = userAnswers;
                question.Instruction = questionInstruction;
                question.AttemptTime = attemptTime;
                question.AttemptTimeType = attemptTimeType;
                question.DelayTime = delayTime;
                question.Delay = Convert.ToInt32(questionTemplateRow["delay"]);
                question.RecordingTime = Convert.ToInt32(questionTemplateRow["recordingTime"]);
                question.PlayBeep = Convert.ToBoolean(questionTemplateRow["beep"]);
                question.OutputFile = Convert.ToString(questionTemplateRow["output"]);
                question.Media = MediaReader.GetMediaPath(Convert.ToString(questionTemplateRow["audio"]),"SPK");
                question.AudioDelay = Convert.ToInt32(questionTemplateRow["audioDelay"]);
                question.Picture = Convert.ToString(questionTemplateRow["picture"]);
                question.QuestionTemplate = questionTemplate;

                string transcript = Convert.ToString(questionTemplateRow["transcript"]);

                question.ShowTranscript = !string.IsNullOrEmpty(transcript);
                question.SampleTranscript = transcript;

                question.CurrentQuestionType = QuestionType.SPEAKING;
                question.CurrentPracticeSetId = practiceSetId;

                question.QuestionIndex = currentQuestionIndex;
                question.TotalQuestionsCount = questionsDataRows.Count();
                Switcher.Switch(new LookListenAndSpeak(), question);

            }
        }
        private void LoadReadingQuestions(string practiceSetId, bool isPreviousQuestionSelected, bool isNextQuestionSelected, Mode questionMode, TestMode testMode, Mode setMode)
        {
            DataSet dsReadingQuestions = FileReader.ReadFile(FileReader.FileType.QUESTION_READING);
            ProcessQuestion(dsReadingQuestions, practiceSetId, isPreviousQuestionSelected, isNextQuestionSelected,questionMode,QuestionType.READING,testMode,  setMode);
        }

        private void LoadWritingQuestions(string practiceSetId, bool isPreviousQuestionSelected, bool isNextQuestionSelected, Mode questionMode, TestMode testMode, Mode setMode)
        {
            DataSet dsWritingQuestions = FileReader.ReadFile(FileReader.FileType.QUESTION_WRITING);
            ProcessQuestion(dsWritingQuestions, practiceSetId, isPreviousQuestionSelected, isNextQuestionSelected,questionMode,QuestionType.WRITING, testMode,  setMode);
        }

        private void LoadListeningQuestions(string practiceSetId, bool isPreviousQuestionSelected, bool isNextQuestionSelected, Mode questionMode, TestMode testMode, Mode setMode)
        {
            DataSet dsWritingQuestions = FileReader.ReadFile(FileReader.FileType.QUESTION_LISTENING);
            ProcessQuestion(dsWritingQuestions, practiceSetId, isPreviousQuestionSelected, isNextQuestionSelected,questionMode,QuestionType.LISTENING, testMode,setMode);
        }

        private void LoadSpeakingQuestions(string practiceSetId, bool isPreviousQuestionSelected, bool isNextQuestionSelected, Mode questionMode, TestMode testMode, Mode setMode)
        {
            DataSet dsWritingQuestions = FileReader.ReadFile(FileReader.FileType.QUESTION_SPEAKING);
            ProcessQuestion(dsWritingQuestions, practiceSetId, isPreviousQuestionSelected, isNextQuestionSelected, questionMode, QuestionType.SPEAKING, testMode,setMode);
        }

        private void LoadQuestion(object state)
        {
            Entities.Question question = (Entities.Question)state;
            switch (question.QuestionType)
            {
                case QuestionType.READING:
                    LoadReadingQuestions(question.PracticeSetId, question.IsPreviousQuestionSelected, question.IsNextQuestionSelected,question.QuestionMode, question.TestMode,question.SetMode);
                    break;
                case QuestionType.WRITING:
                    LoadWritingQuestions(question.PracticeSetId, question.IsPreviousQuestionSelected, question.IsNextQuestionSelected, question.QuestionMode, question.TestMode,question.SetMode);
                    break;
                case QuestionType.SPEAKING:
                    LoadSpeakingQuestions(question.PracticeSetId, question.IsPreviousQuestionSelected, question.IsNextQuestionSelected, question.QuestionMode, question.TestMode,question.SetMode);
                    break;
                case QuestionType.LISTENING:
                    LoadListeningQuestions(question.PracticeSetId, question.IsPreviousQuestionSelected, question.IsNextQuestionSelected, question.QuestionMode, question.TestMode,question.SetMode);
                    break;
                default:
                    break;
            }
        }

        public QuestionSwitcher()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            LoadQuestion(state);
        }
    }
}
