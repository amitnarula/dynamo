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
        private BitmapImage playImage = null;
        private BitmapImage stopImage = null;
        public Image imgLegend { get; set; }
        public TextBlock txtBlkLegend { get; set; }

        public delegate void PlayStopEventHandler(object sender, EventArgs e);
        public event PlayStopEventHandler PlayStopClicked;

        public SmallAudioPlayer()
        {
            InitializeComponent();
            playImage = new BitmapImage(new Uri("../../Templates/images/play.png",UriKind.Relative));
            stopImage = new BitmapImage(new Uri("../../Templates/images/stop.png", UriKind.Relative));
        }

        public void Play()
        {
            medAudio.Play();
            //btnPlayStop.Content = "Stop";
            imgLegend.Source = stopImage;
            //this.Image = stopImage;
        }
        public void Stop()
        {
            medAudio.Stop();
            //btnPlayStop.Content = "Play";
            imgLegend.Source = playImage;
            //this.Image = playImage;
        }

        private void btnPlayStop_Click(object sender, RoutedEventArgs e)
        {
            if (imgLegend.Source == playImage)
            //if (this.Image.Source == playImage)
            {
                medAudio.Play();
                //btnPlayStop.Content = "Stop";
                imgLegend.Source = stopImage;
                //this.Image = stopImage;
            }
            else
            {
                medAudio.Stop();
                //btnPlayStop.Content = "Play";
                imgLegend.Source = playImage;
                //this.Image = playImage;
            }
            if (PlayStopClicked != null)
                PlayStopClicked(sender, e);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            imgLegend = btnPlayStop.Template.FindName("imgLegend", btnPlayStop) as Image;
            txtBlkLegend = btnPlayStop.Template.FindName("txtBlkLegend", btnPlayStop) as TextBlock;
            if (imgLegend != null)
                imgLegend.Source = playImage;
            
            if (!string.IsNullOrEmpty(Media))
            {
                medAudio.Source = new Uri(Media, UriKind.RelativeOrAbsolute);

                //byte[] data = File.ReadAllBytes(Media);

                //System.IO.File.WriteAllBytes("temp.wav", data);
                //medAudio.Source = (new Uri("temp.wav", UriKind.Relative));
                //mediaPlayer.Play();
            }
            if (string.IsNullOrEmpty(Legend))
            {
                txtBlkLegend.Text = "Sample Response";
                lblLegend.Content = "Sample Response";
            }
            else
            {
                lblLegend.Content = this.Legend;
                if (txtBlkLegend != null)
                    txtBlkLegend.Text = this.Legend;
            }
        }

        private void medAudio_MediaEnded(object sender, RoutedEventArgs e)
        {
            medAudio.Stop();
            //btnPlayStop.Content = "Play";
            //txtBlkLegend.Text = "Play";
            imgLegend.Source = playImage;
        }
    }
}
