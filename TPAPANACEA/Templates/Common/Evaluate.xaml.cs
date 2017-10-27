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
using TPACORE.CoreFramework;
using TPACORE.Entities;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for Evaluate.xaml
    /// </summary>
    public partial class Evaluate : Window
    {
        public string QuestionId { get; set; }

        public List<EvaluationParameter> Parameters { get; set; }

        public Evaluate()
        {
            InitializeComponent();
            this.Loaded += Evaluate_Loaded;
        }

        private void Evaluate_Loaded(object sender, RoutedEventArgs e)
        {
            var savedResults = EvaluationManager.GetResult(QuestionId);

            foreach (var item in Parameters)
            {
                EvalParameter ep = new EvalParameter();
                ep.ParamName = item.Name;
                ep.ParamMax = item.Max;
                ep.ParamMin = item.Min;
                ep.Type = item.Type;
                ep.ParamScore = savedResults != null ? savedResults.Where(x => x.ParamName.Equals(ep.ParamName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault().ParamScore : string.Empty;
                stkParams.Children.Add(ep);

            }

        }

        private void btnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            List<EvaluationResult> result = new List<EvaluationResult>();
            
            foreach (var item in Parameters)
            {
                result.Add(new EvaluationResult()
                {
                    ParamMaxScore = item.Max,
                    ParamName = item.Name,
                    ParamScore = "3" //default value is a must here
                });
            }

            EvaluationManager.Evaluate(QuestionId, result);
        }
    }
}
