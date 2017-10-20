using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPAPathAbroad.Templates.Common;
using System.Windows.Forms;
using System.Windows;

namespace TPAPathAbroad.FX
{
    public class MessageDialog
    {
        
        public static DialogResult Show(string title, string content,
            string okButtonText = "Ok", string cancelButtonText = "Cancel", bool disableCancelButton=false)
        {
            using (var d = new Dialog())
            {
                d.txtBlkTitle.Text = title;
                d.txtBlkContent.Text = content;
                d.btnOk.Content = okButtonText;
                d.btnClose.Content = cancelButtonText;
                d.btnClose.Visibility = disableCancelButton?Visibility.Collapsed:Visibility.Visible;

                if (disableCancelButton)
                {
                    d.btnOk.Margin = d.btnClose.Margin;
                }

                if (d.ShowDialog().Value)
                    return DialogResult.Yes;
                else
                    return DialogResult.Cancel;
            }

            
        }

    }
}
