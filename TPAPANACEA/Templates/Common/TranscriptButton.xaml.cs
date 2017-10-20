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

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for TranscriptButton.xaml
    /// </summary>
    public partial class TranscriptButton : UserControl
    {
        public string Transcript { get; set; }

        public TranscriptButton()
        {
            InitializeComponent();
        }

        private void btnTranscript_Click(object sender, RoutedEventArgs e)
        {
            TranscriptBox transcriptBox = new TranscriptBox();
            transcriptBox.Transcript = Transcript;
            transcriptBox.ShowDialog();
        }
    }
}
