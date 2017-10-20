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
using System.IO;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for SmallAudioPlayer.xaml
    /// </summary>
    public partial class SmallAudioPlayer : UserControl
    {
        public string Media { get; set; }
        public string Legend { get; set; }

        public delegate void PlayStopEventHandler(object sender, EventArgs e);
        public event PlayStopEventHandler PlayStopClicked;

        public SmallAudioPlayer()
        {
            InitializeComponent();
        }

        public void Play()
        {
            medAudio.Play();
            btnPlayStop.Content = "Stop";
        }
        public void Stop()
        {
            medAudio.Stop();
            btnPlayStop.Content = "Play";
        }

        private void btnPlayStop_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(btnPlayStop.Content) == "Play")
            {
                medAudio.Play();
                btnPlayStop.Content = "Stop";
            }
            else
            {
                medAudio.Stop();
                btnPlayStop.Content = "Play";
            }
            if (PlayStopClicked != null)
                PlayStopClicked(sender, e);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Media))
            {
                medAudio.Source = new Uri(Media, UriKind.RelativeOrAbsolute);

                //byte[] data = File.ReadAllBytes(Media);

                //System.IO.File.WriteAllBytes("temp.wav", data);
                //medAudio.Source = (new Uri("temp.wav", UriKind.Relative));
                //mediaPlayer.Play();
            }
            if (string.IsNullOrEmpty(Legend))
                lblLegend.Content = "Sample Response";
            else
                lblLegend.Content = this.Legend;
        }

        private void medAudio_MediaEnded(object sender, RoutedEventArgs e)
        {
            medAudio.Stop();
            btnPlayStop.Content = "Play";
        }
    }
}
