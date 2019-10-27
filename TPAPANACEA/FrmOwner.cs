using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TPACORE.CoreFramework;

namespace TPA
{
    public partial class FrmOwner : Form
    {
        public FrmOwner()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBoxName.Text.Trim()))
            {
                var result = MessageBox.Show("Owner name has been set to " + txtBoxName.Text.Trim() + ". Press Yes to continue.", "Information set", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (UserManager.GetUserById("institute") == null)
                    {

                        UserManager.CreateUser(new User()
                        {
                            Email = "institute@institute.com",
                            ContactNo = "9999999990",
                            Firstname = txtBoxName.Text,
                            Lastname = "institute",
                            Password = "institute",
                            UserId = "institute",
                            Username = "institute",
                            DOB="01/01/1970"
                        });
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            
        }
    }
}
