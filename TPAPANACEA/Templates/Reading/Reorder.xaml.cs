using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TPA.Templates.Reading
{
    public class ReorderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Brush OptionBackgroundColor { get; set; } = new SolidColorBrush(Color.FromRgb(0,96,156));
    }

    /// <summary>
    /// Interaction logic for Reorder.xaml
    /// </summary>
    public partial class Reorder : UserControl, ISwitchable, IDropTarget
    {
        public ObservableCollection<ReorderItem> SourceItems { get; set; }
        public ObservableCollection<ReorderItem> TargetItems { get; set; }
        public ObservableCollection<ReorderItem> AnswerItems { get; set; }
        ReorderQuestion question;
        int itemCount = 0;
        public Reorder()
        {
            InitializeComponent();
            SourceItems = new ObservableCollection<ReorderItem>();
            TargetItems = new ObservableCollection<ReorderItem>();

            this.Loaded += Reorder_Loaded;

        }

        private void Reorder_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach (var item in lstSource.Items)
            //{
            //    var _Container = lstSource.ItemContainerGenerator
            //        .ContainerFromItem(item);
                
            //    var _Children = AllChildren(_Container);

            //    var grdSource = _Children
            //        // only interested in TextBoxes
            //        .OfType<Grid>()
            //        // only interested in FirstName
            //        .First(x => x.Name.Equals("grdSource"));

            //    grdSource.Background = Brushes.Aqua;
            //}
        }

        public void UtilizeState(object state)
        {
            question = (ReorderQuestion)state;
            txtBlkInstruction.Text = question.Instruction;
            if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
            {
                foreach (Option option in question.Options)
                {
                    ReorderItem reorderItem = new ReorderItem();
                    reorderItem.Id = Convert.ToInt32(option.Id);
                    reorderItem.Name = option.OptionText;
                    SourceItems.Add(reorderItem);
                }
            }
            if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            {
                string[] correctAnswers = question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    correctAnswers = question.UserAnswers;
                if (correctAnswers.Any())
                {
                    SourceItems = new ObservableCollection<ReorderItem>();
                    TargetItems = new ObservableCollection<ReorderItem>();
                    for (int count = 0; count < correctAnswers.Length; count++)
                    {
                        int itemId = 0;
                        if (int.TryParse(correctAnswers[count], out itemId))
                        {
                            ReorderItem reorderItem = new ReorderItem();
                            reorderItem.Id = Convert.ToInt32(itemId);
                            reorderItem.Name = question.Options.Where(_ => _.Id == correctAnswers[count]).Select(_ => _.OptionText).SingleOrDefault();
                            TargetItems.Add(reorderItem);
                        }

                    }
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                {
                    lstSource.IsEnabled = false;
                    lstTarget.IsEnabled = false;
                    btnToDown.IsEnabled = false;
                    btnToUp.IsEnabled = false;
                    btnToRight.IsEnabled = false;
                    btnToLeft.IsEnabled = false;
                }
            }
            lstSource.ItemsSource = SourceItems;
            lstTarget.ItemsSource = TargetItems;

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += prevNext_YourResponseClicked;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.READING;
            //itemCount = SourceItems.Count;
            itemCount = question.Options.Count;
            
        }

        private List<DependencyObject> GetAllParents(DependencyObject obj)
        {
            var lst = new List<DependencyObject>();
            var parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                lst.Add(parent);
                parent = VisualTreeHelper.GetParent(parent);
            }
            return lst;
        }

        private List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }

        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : null;
            string[] correctAnswers = question.CorrectAnswers;
            if (answers == null || !answers.Any())
            {
                lstSource.ItemsSource = null;
                lstSource.IsEnabled = false;
                lstSource.BorderThickness = new Thickness(1);
                return;
            }

            AnswerItems = new ObservableCollection<ReorderItem>();
            for (int count = 0; count < answers.Length; count++)
            {
                ReorderItem reorderItem = new ReorderItem();
                reorderItem.Id = Convert.ToInt32(answers[count]);
                reorderItem.Name = question.Options.Where(_ => _.Id == answers[count]).Select(_ => _.OptionText).SingleOrDefault();

                if (answers[count] != correctAnswers[count])
                    reorderItem.OptionBackgroundColor = new SolidColorBrush(Color.FromRgb(232,85,110)); //correct option 
                else if (answers[count] == correctAnswers[count])
                    reorderItem.OptionBackgroundColor = new SolidColorBrush(Color.FromRgb(35,155,57)); //wrong option

                AnswerItems.Add(reorderItem);

            }
            lstSource.IsEnabled = true;
            lstSource.BorderThickness = new Thickness(2);
            lstSource.ItemsSource = AnswerItems;
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            string answer = string.Empty;
            if (TargetItems.Count == itemCount) //If all the items are sorted out only then answer be logged
            {
                foreach (var item in TargetItems)
                {
                    answer += item.Id.ToString() + "|";
                }

                answer = answer.TrimEnd(new char[] { '|' });
                AnswerManager.LogAnswer(question, answer, prevNext.GetAttemptTimeLeft());
            }
        }
        void IDropTarget.DragOver(DropInfo dropInfo)
        {
            if (dropInfo.Data is ReorderItem)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(DropInfo dropInfo)
        {

            ReorderItem msp = (ReorderItem)dropInfo.Data;
            ((IList)dropInfo.DragInfo.SourceCollection).Remove(msp);

        }
        private void btnToRight_Click(object sender, RoutedEventArgs e)
        {
            if (lstSource.SelectedIndex >= 0)
            {
                var reorderItem = lstSource.Items[lstSource.SelectedIndex] as ReorderItem;
                TargetItems.Add(reorderItem);
                SourceItems.RemoveAt(lstSource.SelectedIndex);
            }
        }
        private void btnToLeft_Click(object sender, RoutedEventArgs e)
        {
            if (lstTarget.SelectedIndex >= 0)
            {
                SourceItems.Insert(0, ((ReorderItem)(lstTarget.Items[lstTarget.SelectedIndex])));
                TargetItems.RemoveAt(lstTarget.SelectedIndex);
            }
        }
        private void btnToUp_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lstTarget.SelectedIndex;
            if (selectedIndex > 0)
            {
                int targetIndex = selectedIndex - 1;
                TargetItems.Insert(targetIndex, ((ReorderItem)(lstTarget.Items[lstTarget.SelectedIndex])));
                TargetItems.RemoveAt(selectedIndex + 1);
            }
        }
        private void btnToDown_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lstTarget.SelectedIndex;
            if (selectedIndex >= 0)
            {
                if (selectedIndex < lstTarget.Items.Count)
                {
                    var item = lstTarget.Items[lstTarget.SelectedIndex] as ReorderItem;
                    int targetIndex = selectedIndex + 1;
                    if (TargetItems.Any() && TargetItems.Count > 1)
                    {
                        TargetItems.RemoveAt(selectedIndex);
                        TargetItems.Insert(targetIndex, item);
                        //SourceTwo.RemoveAt(selectedIndex);
                    }
                }
            }
        }
    }
}
