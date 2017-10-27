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

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for EvalParameter.xaml
    /// </summary>
    public partial class EvalParameter : UserControl
    {
        public string ParamName { get { return lblParamName.Content.ToString(); } set { lblParamName.Content = value; } }
        public string ParamMax { get; set; }
        public string ParamMin { get; set; }
        public string Type { get; set; }

        public string ParamScore { get { return txtEvaluatedMarks.Text; } set { txtEvaluatedMarks.Text = value; } }

        public EvalParameter()
        {
            InitializeComponent();
            this.Loaded += EvalParameter_Loaded;
        }

        private void EvalParameter_Loaded(object sender, RoutedEventArgs e)
        {
            lblOutOf.Content = "/" + ParamMax;
            lblParamName.Content = ParamName;
            txtEvaluatedMarks.Text = ParamScore;
        }

        public bool ValidateParameterInput()
        {
            //validate type and values
            int valueResult = 0;
            decimal decimalValueResult = 0;
            switch (Type)
            {
                case "int":
                    if (!int.TryParse(ParamScore, out valueResult) || valueResult > Convert.ToInt32(ParamMax))
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid value for parameter :"+ParamName);
                        return false;
                    }
                    break;
                case "decimal":
                    if (!decimal.TryParse(ParamScore, out decimalValueResult) || decimalValueResult > Convert.ToDecimal(ParamMax))
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid value for parameter:"+ParamName);
                        return false;
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid type of parameter");
            }
            return true;
        }
    }
}
