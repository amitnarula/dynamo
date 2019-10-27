using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace TPAMED
{
    public partial class frmMediaEncrypter : Form
    {
        BackgroundWorker worker;
        public frmMediaEncrypter()
        {
            InitializeComponent();

            if (worker != null)
                worker.Dispose();

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProgress.Text = e.ProgressPercentage.ToString();
            lblProgress.Refresh();
        }

        private void btnEncryptMedia_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles("Data");
            string[] speakingFiles = Directory.GetFiles("Data\\SPK");

            int totalNumberOfFiles = files.Length + speakingFiles.Length;
            progressBar.Maximum = totalNumberOfFiles;
            progressBar.Minimum = 0;
            progressBar.Step = 1;
            int count = 0;
            foreach (var file in files)
            {
                byte[] data = File.ReadAllBytes(file);
                string output = Convert.ToBase64String(data);
                string fileName = Path.GetFileNameWithoutExtension(file);
                string extension = Path.GetExtension(file);
                string outputExtension = string.Empty;

                if (extension.Equals(".mp3",StringComparison.InvariantCultureIgnoreCase)
                    || extension.Equals(".wav", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".tpm";
                else if (extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
                    || extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".tpi";

                //File.Create("output\\" + fileName + ".tpa");

                // File.WriteAllText("output\\" + fileName.Replace("–", "-") + outputExtension, output);
                File.WriteAllText("output\\" + Regex.Replace(fileName,"[\u001e|\u2011|\u2013|\u2014]","-")  + outputExtension, output);
                progressBar.Value += 1;
                count++;
                worker.ReportProgress(count);
                
            }

            foreach (var file in speakingFiles)
            {
                byte[] data = File.ReadAllBytes(file);
                string output = Convert.ToBase64String(data);
                string fileName = Path.GetFileNameWithoutExtension(file);
                string extension = Path.GetExtension(file);
                string outputExtension = string.Empty;

                if (extension.Equals(".mp3", StringComparison.InvariantCultureIgnoreCase)
                    || extension.Equals(".wav", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".tpm";
                else if (extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
                    || extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".tpi";

                //File.Create("output\\" + fileName + ".tpa");

                File.WriteAllText("output\\spk\\" + Regex.Replace(fileName, "[\u001e|\u2011|\u2013|\u2014]", "-") + outputExtension, output);
                // File.WriteAllText("output\\spk\\" + fileName.Replace("–", "-") + outputExtension, output);
                progressBar.Value += 1;
                count++;
                worker.ReportProgress(count);
                
            }

            MessageBox.Show(totalNumberOfFiles + " files have been processed successfully");
            
        }

        private void btnDecrptMedia_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles("Output");

            foreach (var file in files)
            {
                string base64 = File.ReadAllText(file);
                byte[] data = Convert.FromBase64String(base64);
                string fileName = Path.GetFileNameWithoutExtension(file);

                string extension = Path.GetExtension(file);
                string outputExtension = string.Empty;

                if (extension.Equals(".tpm", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".mp3";
                else if (extension.Equals(".tpi", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".jpg";


                //File.Create("output\\" + fileName + ".tpa");

                File.WriteAllBytes("Original\\" + fileName + outputExtension, data);
                File.Delete(file);
            }

            string[] speakingFiles = Directory.GetFiles("Output\\SPK");
            foreach (var file in speakingFiles)
            {
                byte[] data = File.ReadAllBytes(file);
                string output = Convert.ToBase64String(data);
                string fileName = Path.GetFileNameWithoutExtension(file);
                string extension = Path.GetExtension(file);
                string outputExtension = string.Empty;

                if (extension.Equals(".tpm", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".mp3";
                else if (extension.Equals(".tpi", StringComparison.InvariantCultureIgnoreCase))
                    outputExtension = ".jpg";



                //File.Create("output\\" + fileName + ".tpa");

                File.WriteAllText("original\\spk\\" + fileName + outputExtension, output);
                File.Delete(file);
            }

            this.Close();
        }

        private void btnPlayShow_Click(object sender, EventArgs e)
        {
            ucMediaElement1.PlayShowRandomly();
        }

        private void btnRenameFiles_Click(object sender, EventArgs e)
        {
            //Audio
            var xDocument = XDocument.Load("Data\\B4AFE0DCEE464F9282BB52EE0FC03F02_QL.xml");
            var result = xDocument.Root.Elements("question").Select(x => new
            {

               ID = x.Element("id").Value,
               File = Convert.ToString(x.Element("template").Descendants("media").SingleOrDefault().Value)
            });

            var xDocument2 = XDocument.Load("Data\\B4AFE0DCEE464F9282BB52EE0FC03F02_QS.xml");
            var result2 = xDocument2.Root.Elements("question").Select(x => new
            {

                ID = x.Element("id").Value,
                File = Convert.ToString(x.Element("template").Descendants().Where(y=>y.Name=="audio").SingleOrDefault().Value)
            });

        }

        private void btnCheckMediaForErrors_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles("output");
            List<string> fileNames = new List<string>();
            files.ToList().ForEach(file => {
                file = file.Replace("output\\", string.Empty);
                if (!file.Contains("tpi"))
                    fileNames.Add(file);
            });
            
            string[] speakingFiles = Directory.GetFiles("output\\SPK");
            List<string> speakingFileNames = new List<string>();
            speakingFiles.ToList().ForEach(file => {
                file = file.Replace("output\\SPK\\", string.Empty);
                if (!file.Contains("tpi"))
                    speakingFileNames.Add(file);
                
            });

            string[] spFileNames = File.ReadAllLines("data\\s_file.txt");
            string[] lFileNames = File.ReadAllLines("data\\l_file.txt");

           
            //compare file names with actual files if there is mismatch
            //write it to new file
            var errornusSpeakingFiles = speakingFileNames.Except(spFileNames);
            var errornusListeningFiles = fileNames.Except(lFileNames);

            File.WriteAllLines("output\\errornusl.txt",errornusListeningFiles.ToArray());
            File.WriteAllLines("output\\errornuss.txt",errornusSpeakingFiles.ToArray());

            MessageBox.Show("Erros have been successfully propagated.");
        }
    }
}
