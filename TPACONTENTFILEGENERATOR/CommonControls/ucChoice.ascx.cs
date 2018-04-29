using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucChoice : BaseUserControl
{
    public List<Option> Options
    {
        get { return (List<Option>)(ViewState["options"]); }
        set { ViewState["options"] = value; }
    }

    public event GenerateOptionsClick OnGenerateOptionsClick;

    public delegate void GenerateOptionsClick(object sender, List<Option> options);
    

    public string Mode { get; set; }
    int maxOptions = 8;
    
    protected void Page_OnInit(object sender, EventArgs e)
    {
        
    }

    protected void btnGenerateOptions_Click(object sender, EventArgs e)
    {
        if (OnGenerateOptionsClick != null)
        {
            List<Option> optionsOutput = new List<Option>();
            foreach (RepeaterItem item in rptOptions.Items)
            {
                if (item.ItemType == ListItemType.Item ||
                    item.ItemType == ListItemType.AlternatingItem)
                {

                    CheckBox chkSelect=null;
                    RadioButton radSelect = null;
                    bool optionSelected = false;
                    if (Mode == "MultiSelect")
                    {
                        chkSelect = (CheckBox)item.FindControl("chkSelectOption");
                        optionSelected = chkSelect.Checked;


                    }
                    else if (Mode == "MultiChoice")
                    {
                        radSelect = (RadioButton)item.FindControl("radBtnOption");
                        optionSelected = radSelect.Checked;
                    }

                    TextBox txtOption = (TextBox)item.FindControl("txtOption");

                    if (!string.IsNullOrEmpty(txtOption.Text))
                    {
                        Option opt = new Option();
                        opt.ID = optionsOutput.Count;
                        opt.OptionText = txtOption.Text.Trim();
                        opt.Selected = optionSelected;
                        optionsOutput.Add(opt);
                    }
                }
            }

            for (int cont = optionsOutput.Count; cont < maxOptions; cont++)
			{
                optionsOutput.Add(new Option() { 
                    ID = cont,
                    OptionText= string.Empty,
                    Selected = false
                });
			} //Fill upto 8 for rest of the options

            this.Options = optionsOutput;

            OnGenerateOptionsClick(sender, optionsOutput);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindOptions();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Mode == "MultiChoice" ||  Mode=="MultiSelect")
        {
            pnlMultiChoiceOrSelect.Visible = true;

        }
    }

    private void BindOptions()
    {
        if (this.Options == null)
        {
            this.Options = new List<Option>();
            for (int count = 0; count < maxOptions; count++)
            {
                Options.Add(new Option()
                {
                    ID = count,
                    OptionText = string.Empty
                });
            }
        }

        rptOptions.DataSource = this.Options;
        rptOptions.DataBind();
    }

    protected void rptOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            if (Mode == "MultiChoice")
            {
                CheckBox chkOption = e.Item.FindControl("chkSelectOption") as CheckBox;
                chkOption.Visible = false;
            }
            else if (Mode == "MultiSelect")
            {
                RadioButton radBxOption = e.Item.FindControl("radBtnOption") as RadioButton;
                radBxOption.Visible = false;
            }
        }
    }
}