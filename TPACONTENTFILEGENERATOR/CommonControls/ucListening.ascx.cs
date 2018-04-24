using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucListening : BaseUserControl
{
    public bool ListenAndWrite { get; set; }
    public bool ListenMultiSelect { get; set; }
    public bool ListenMultiChoice { get; set; }
    int maxOptions = 10;
    //public string MaxWordCount { get { return txtMaxWordCount.Text; } }
    //public string Answer { get { return txtAnswer.Text; } }
    public string AudioDelay { get { return new TimeSpan(0, 0, Convert.ToInt32(txtDelay.Text)).ToString(); } }
    public string MediaFileName { get { return txtMedia.Text; } }

    class Option
    {
        public int ID { get; set; }
        public string OptionText { get; set; }
    }

    public enum Mode
    {
        SingleSelect,
        MultiSelect
    }

    public Mode OptionMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ListenMultiChoice || ListenMultiSelect)
        {
            trTemplateListenWithSelectionOptions.Visible = true;
            List<Option> Options = new List<Option>();
            for (int count = 0; count < maxOptions; count++)
            {
                Options.Add(new Option()
                {
                    ID = count,
                    OptionText = string.Empty
                });
            }

            rptOptions.DataSource = Options;
            rptOptions.DataBind();
        }

    }

    protected void rptOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            if (ListenMultiSelect)
            {
                CheckBox chkOption = e.Item.FindControl("chkSelectOption") as CheckBox;
                chkOption.Visible = false;
            }
            else if(ListenMultiChoice)
            {
                RadioButton radBxOption = e.Item.FindControl("radSelectOption") as RadioButton;
                radBxOption.Visible = false;
            }
        }
    }
}