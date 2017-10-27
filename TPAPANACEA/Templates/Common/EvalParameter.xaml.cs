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
        public string ParamName { get; set; }
        public string ParamMax { get; set; }
        public string ParamMin { get; set; }
        public string Type { get; set; }

        public string ParamScore { get; set; }

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
    }
}
