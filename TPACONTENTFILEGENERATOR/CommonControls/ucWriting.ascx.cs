using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucWriting : BaseUserControl
{
    public string CorrectAnswer { get { return txtCorrectAnswer.Text; } }

    public string MaxWordCount { get { return txtMaxWordCount.Text.ToString(); } }

    public override void Reset()
    {
        txtCorrectAnswer.Text = string.Empty;
        txtMaxWordCount.Text = string.Empty;
        base.Reset();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}