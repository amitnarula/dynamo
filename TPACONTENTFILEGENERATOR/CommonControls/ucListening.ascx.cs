using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucListening : BaseUserControl
{
    public bool ListenAndWrite { get; set; }
    public string MaxWordCount { get { return txtMaxWordCount.Text; } }
    public string Answer { get { return txtAnswer.Text; } }
    public string AudioDelay { get { return new TimeSpan(0, 0, Convert.ToInt32(txtDelay.Text)).ToString(); } }
    public string MediaFileName { get { return txtMedia.Text; } }
    

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        trTemplateListenWrite.Visible = ListenAndWrite;
    }
}