/************************************************************************
 * Copyright: Hans Wolff
 *
 * License:  This software abides by the LGPL license terms. For further
 *           licensing information please see the top level LICENSE.txt 
 *           file found in the root directory of CodeReason Reports.
 *
 * Author:   Hans Wolff
 *
 ************************************************************************/

using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Application's main window
    /// </summary>
    public partial class ReportViewer : Window
    {
        private bool _firstActivated = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportViewer()
        {
            InitializeComponent();
        }

        public DataTable ReportData { get; set; }
        public DataTable ReportHeader { get; set; }

        public DataTable ReportGraph { get; set; }
        public string ReportTitle { get; set; }

        public string TemplateType { get; set; }

        public void GenerateReport()
        {
            this.Activate();
        }

        /// <summary>
        /// Window has been activated
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event details</param>
        //private void Window_Activated(object sender, EventArgs e)
        //{
        //    if (!_firstActivated) return;

        //    _firstActivated = false;

        //    Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(delegate
        //    {
        //        try
        //        {
        //            ReportDocument reportDocument = new ReportDocument();
        //            StreamReader reader = new StreamReader(new FileStream(@"Templates\Common\ReportTemplates\SimpleReport.xaml", FileMode.Open, FileAccess.Read));
        //            reportDocument.XamlData = reader.ReadToEnd();
        //            reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
        //            reader.Close();

        //            ReportData data = new ReportData();
        //            if (ReportData != null)
        //                data.DataTables.Add(ReportData);
        //            if (ReportHeader != null)
        //                data.DataTables.Add(ReportHeader);

        //            XpsDocument xps = reportDocument.CreateXpsDocument(data);
        //            documentViewer.Document = xps.GetFixedDocumentSequence();
        //        }
        //        catch (Exception ex)
        //        {
        //            // show exception
        //            MessageBox.Show(ex.Message + "\r\n\r\n" + ex.GetType() + "\r\n" + ex.StackTrace, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
        //        }
        //        finally
        //        {
        //            busyDecorator.IsBusyIndicatorHidden = true;
        //        }
        //    }));
        //}

        private void Window_Activated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(delegate
            {
                try
                {
                    ReportDocument reportDocument = new ReportDocument();
                    StreamReader reader = new StreamReader(new FileStream(@"Templates\Common\Reports\"+ TemplateType + ".xaml", FileMode.Open, FileAccess.Read));
                    reportDocument.XamlData = reader.ReadToEnd();
                    reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                    reader.Close();

                    ReportData data = new ReportData();
                    
                    data.ReportDocumentValues.Add("ReportTitle", ReportTitle);
                    data.ReportDocumentValues.Add("PrintDate", DateTime.Now);

                    if (ReportData != null)
                        data.DataTables.Add(ReportData);
                    if (ReportHeader != null)
                        data.DataTables.Add(ReportHeader);
                    if (ReportGraph != null)
                        data.DataTables.Add(ReportGraph);

                    XpsDocument xps = reportDocument.CreateXpsDocument(data);
                    documentViewer.Document = xps.GetFixedDocumentSequence();
                }
                catch (Exception ex)
                {
                    // show exception
                    MessageBox.Show(ex.Message + "\r\n\r\n" + ex.GetType() + "\r\n" + ex.StackTrace, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                finally
                {
                    busyDecorator.IsBusyIndicatorHidden = true;
                }
            }));
        }
    }
}
