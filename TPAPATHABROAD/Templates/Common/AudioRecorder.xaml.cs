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
using NAudio;
using NAudio.Wave;
using TPA.CoreFramework;
using System.Windows.Threading;
using System.Media;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for AudioRecorder.xaml
    /// </summary>
    public partial class AudioRecorder : UserControl
    {
        private WaveIn waveIn;
        private WaveFileWriter writer;
        DispatcherTimer recordingTimer;
        DispatcherTimer delayTimer;
        int delayTickCount = 0;
        int recordingTickCount = 0;

        public string Media { get; set; }
        public string Picture { get; set; }
        public int Delay { get; set; }
        public int RecordingTime { get; set; }
        public string OutputFile { get; set; }
        public bool PlayBeepSound { get; set; }
        

        public AudioRecorder()
        {
            InitializeComponent();
            recordingTimer = new DispatcherTimer();
            recordingTimer.Tick += new EventHandler(recordingTimer_Tick);
            recordingTimer.Interval = TimeSpan.FromSeconds(1);

            delayTimer = new DispatcherTimer();
            delayTimer.Tick += new EventHandler(delayTimer_Tick);
            delayTimer.Interval = TimeSpan.FromSeconds(1);
            PlayBeepSound = true;

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
                    lblRecordingStatus.Content = string.Format("Recording starts in {0} seconds", (Delay - delayTickCount));
                }
                else
                {
                    lblRecordingStatus.Content = "Recording..";
                    delayTimer.Stop();

                    if (PlayBeepSound)
                        new SoundPlayer(MediaReader.GetResourcePath("beep.wav")).Play();

                    try
                    {
                        string outputFilename = MediaReader.GetOutputFileName(OutputFile, "wav");
                        int sampleRate = 22000;
                        int channels = 1;
                        waveIn = new WaveIn();
                        waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                        waveIn.DeviceNumber = 0;
                        waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(
                            waveIn_DataAvailable);
                        writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);

                        prgsBarRecording.Maximum = RecordingTime;
                        prgsBarRecording.SmallChange = 1;
                        prgsBarRecording.LargeChange = Math.Min(10, RecordingTime / 10);

                        waveIn.StartRecording();
                        recordingTimer.Start();
                    }
                    catch (Exception)
                    {
                        //TODO:Audit Logging
                    }
            
                }
            }
        }

        void recordingTimer_Tick(object sender, EventArgs e)
        {
            if (RecordingTime == 0)
            {
                recordingTimer.Stop();
                lblRecordingStatus.Content = "Completed";
                waveIn.StopRecording();
                waveIn.Dispose();
                writer.Close();
            }
            else
            {
                RecordingTime--;
                prgsBarRecording.Value = prgsBarRecording.Maximum - RecordingTime;
                lblRecordingStatus.Content = string.Format("Time left {0} seconds", RecordingTime);
            }
        }

        private void ucAudioPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void InitializeRecorder()
        {
            delayTimer.Start();
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            writer.WriteData(e.Buffer, 0, e.BytesRecorded);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (waveIn!=null)
                waveIn.Dispose();
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
            if (delayTimer != null)
                delayTimer.Stop();
            if (recordingTimer != null)
                recordingTimer.Stop();
        }

    }
}
