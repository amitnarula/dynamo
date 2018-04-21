using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucQuestionCommon : BaseUserControl
{
    public string DelayTime
    {
        get
        {
            return new TimeSpan(0,Convert.ToInt32(ddlDelayTimeMinutes.SelectedValue)
                ,Convert.ToInt32(ddlDelayTimeSeconds.SelectedValue)).ToString();
        }
    }

    public string AttemptTime
    {
        get {
            return new TimeSpan(0,Convert.ToInt32(ddlAttemptTimeMinutes.SelectedValue),
                Convert.ToInt32(ddlAttemptTimeSeconds.SelectedValue)).ToString();
        }
    }

    public string Description { get { return txtDescription.Text; } }
    public string Title { get { return txtTitle.Text; } }
    public string Instruction { get { return txtInstruction.Text; } }

    public new void Reset()
    {
        txtDescription.Text = string.Empty;
        txtInstruction.Text = string.Empty;
        txtTitle.Text = string.Empty;
        ddlAttemptTimeMinutes.SelectedIndex = ddlAttemptTimeSeconds.SelectedIndex = ddlDelayTimeMinutes.SelectedIndex =
            ddlDelayTimeSeconds.SelectedIndex = 0;
    }

    private void BindDropDowns()
    {
        for (int count = 0; count < 60; count++)
        {
            ddlAttemptTimeMinutes.Items.Add(count.ToString());
            ddlAttemptTimeSeconds.Items.Add(count.ToString());
            ddlDelayTimeMinutes.Items.Add(count.ToString());
            ddlDelayTimeSeconds.Items.Add(count.ToString());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDowns();
        }
    }
}