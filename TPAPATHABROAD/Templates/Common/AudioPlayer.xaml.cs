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
using System.Windows.Threading;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for AudioPlayer.xaml
    /// </summary>
    public partial class AudioPlayer : UserControl
    {
        public string Media { get; set; }
        public int Delay { get; set; }

        DispatcherTimer timer;
        DispatcherTimer delayTimer;
        int delayTickCount = 0;

        public delegate void AudioEndedEventHandler(object sender, EventArgs e);
        public event AudioEndedEventHandler AudioEnded;

        public AudioPlayer()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(200);

            delayTimer = new DispatcherTimer();
            delayTimer.Tick += new EventHandler(delayTimer_Tick);
            delayTimer.Interval = TimeSpan.FromSeconds(1);
            
        }

        protected virtual void OnAudioEnded(EventArgs e)
        {
            if (AudioEnded != null)
                AudioEnded(this, e);
        }

        void delayTimer_Tick(object sender, EventArgs e)
        {
            if (Delay == 0)
            {
                delayTimer.Stop();
            }
            else
            {
                if (delayTickCount != (Delay - 1))
                {
                    delayTickCount++;
                    lblStatus.Content = string.Format("Status : Playing in {0} seconds", (Delay - delayTickCount));
                }
                else
                {
                    //lblStatus.Visibility = Visibility.Hidden;
                    delayTimer.Stop();
                    medAudio.Play();
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblStatus.Content = "Status : Playing..";
            playProgress.Value = medAudio.Position.TotalSeconds;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Media))
            {
                medAudio.LoadedBehavior = MediaState.Manual;
                medAudio.Source = new Uri(Media, UriKind.RelativeOrAbsolute);
                delayTimer.Start();
            }
        }

        private void medAudio_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (medAudio.NaturalDuration.HasTimeSpan)
            {
                TimeSpan time = medAudio.NaturalDuration.TimeSpan;
                playProgress.Maximum = time.TotalSeconds;
                playProgress.SmallChange = 1;
                playProgress.LargeChange = Math.Min(10, time.Seconds / 10);
            }
            timer.Start();
        }

        private void slideVolumeControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            medAudio.Volume = e.NewValue/10;
        }

        private void medAudio_MediaEnded(object sender, RoutedEventArgs e)
        {
            OnAudioEnded(e);
            timer.Stop();
            lblStatus.Content = "Status : Completed";
            playProgress.Value += 1;
            //Adjustment so that it may show as complete because timer stops before play progress is filled completely
        }
    }
}
