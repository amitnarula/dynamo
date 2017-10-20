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

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for TranscriptBox.xaml
    /// </summary>
    public partial class TranscriptBox : Window
    {
        public string Transcript { get; set; }

        public TranscriptBox()
        {
            InitializeComponent();
            
            //txtBlkTranscript.Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtBlkTranscript.Text = Transcript;
            //this.Height = txtBlkTranscript.ActualHeight + 10;
        }
    }
}
