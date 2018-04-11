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
using TPA.Entities;
using TPACORE.CoreFramework;
using TPACORE.Entities;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for Evaluate.xaml
    /// </summary>
    public partial class Evaluate : Window
    {
        public QuestionBase QuestionContext { get; set; }

        public List<EvaluationParameter> Parameters { get; set; }

        public Evaluate()
        {
            InitializeComponent();
            this.Loaded += Evaluate_Loaded;
        }

        private void Evaluate_Loaded(object sender, RoutedEventArgs e)
        {
            var savedResults = EvaluationManager.GetResult(QuestionContext);

            foreach (var item in Parameters)
            {
                EvalParameter ep = new EvalParameter();
                ep.ParamName = item.Name;
                ep.ParamMax = item.Max;
                ep.ParamMin = item.Min;
                ep.Type = item.Type;
                var parameterScore = savedResults != null ? savedResults.Where(x => x.ParamName.Equals(ep.ParamName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault() : null;

                //it should be blank when opened by the teacher as suggested by the change
                //ep.ParamScore = parameterScore != null ? parameterScore.ParamScore.ToString() : null;

                stkParams.Children.Add(ep);

            }

        }

        private void btnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            List<EvaluationResult> result = new List<EvaluationResult>();

            foreach (UIElement item in stkParams.Children)
            {
                EvalParameter param = item as EvalParameter;
                if (param.ValidateParameterInput())
                {
                    result.Add(new EvaluationResult()
                    {
                        ParamMaxScore = param.ParamMax,
                        ParamName = param.ParamName,
                        ParamScore = param.ParamScore
                    });
                }
                else
                    break;

            }

            if (result.Count == stkParams.Children.Count) //if all the params are there successfully
            {
                //if content parameter is 0, rest parameter score is also 0
                if (result.Any(x => (x.ParamName.Equals("content", StringComparison.InvariantCultureIgnoreCase)
                    || (x.ParamName.Equals("form", StringComparison.InvariantCultureIgnoreCase))) && x.ParamScore == "0"))
                {
                    result.ForEach((x) =>
                    {
                        x.ParamScore = "0";
                    });
                }

                EvaluationManager.Evaluate(QuestionContext, result);
                System.Windows.Forms.MessageBox.Show("Evaluation successful");
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
