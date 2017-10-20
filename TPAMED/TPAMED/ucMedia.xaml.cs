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

namespace TPAMED
{
    /// <summary>
    /// Interaction logic for ucMediaElement.xaml
    /// </summary>
    public partial class ucMediaElement : UserControl
    {
        public ucMediaElement()
        {
            InitializeComponent();
        }

        public void PlayShowRandomly()
        {
            try
            {

                //Image
                string[] files = Directory.GetFiles("output").Where(x => x.IndexOf(".tpi") > 0).ToArray();
                int randomNumber = new Random().Next(0,files.Length-1);

                string base64 = File.ReadAllText("output\\"+System.IO.Path.GetFileName(files[randomNumber]));
                byte[] data = Convert.FromBase64String(base64);
                 
                BitmapImage bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.StreamSource = new MemoryStream(data);
                bmpImage.EndInit();

                imgRandom.Source = bmpImage;

                //Media file
                string[] audioFiles = Directory.GetFiles("output").Where(x => x.IndexOf(".tpm") > 0).ToArray();
                int randomFileNumber = new Random().Next(0, audioFiles.Length);

                string base64Audio = File.ReadAllText("output\\" + System.IO.Path.GetFileName(audioFiles[randomFileNumber]));
                byte[] audioData = Convert.FromBase64String(base64Audio);

                string outputFileName = "original\\"+System.IO.Path.GetFileNameWithoutExtension(audioFiles[randomFileNumber])+".mp3";

                File.WriteAllBytes(outputFileName,audioData);

                medEleRandom.LoadedBehavior = MediaState.Manual;
                medEleRandom.Source = new Uri(outputFileName, UriKind.Relative);
                medEleRandom.Play();


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
