using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControls_ucBlank : BaseUserControl
{

    public delegate void GenerateAnswer(object sender, Dictionary<string,List<Option>> blanks);
    public event GenerateAnswer OnGenerateAnswerClick;

    public Dictionary<string, List<Option>> Blanks
    {
        get;
        set;
    }

    private void BindOptions()
    {
        
        rptBlank.DataSource = Blanks;
        rptBlank.DataBind();
    }

    protected void Page_OnInit(object sender, EventArgs e)
    {
        
    }

    protected void btnGenerateAnswers_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptBlank.Items)
        {
            TextBox txtCorrectOption = item.FindControl("txtCorrectOption") as TextBox;
            Literal litBlank = item.FindControl("litBlankName") as Literal;

            if (!string.IsNullOrEmpty(txtCorrectOption.Text))
            {
                var correctIndex = Convert.ToInt32(txtCorrectOption.Text);

                Blanks = ViewState["blanks"] as Dictionary<string, List<Option>>;

                var blank = Blanks[litBlank.Text];

                blank[correctIndex].Selected = true;

                //Blanks[litBlank.Text] = blank;

                
            }
        }

        if (OnGenerateAnswerClick != null)
            OnGenerateAnswerClick(sender, Blanks);
    }

    protected void rptBlank_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rptOptions = e.Item.FindControl("rptOptions") as Repeater;

            var dataItem = (KeyValuePair<string, List<Option>>)e.Item.DataItem;

            rptOptions.DataSource = Blanks[dataItem.Key];
            rptOptions.DataBind();
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Blanks = ViewState["blanks"] as Dictionary<string,List<Option>>;

        if (Blanks == null)
            Blanks = new Dictionary<string, List<Option>>();

        var blank = Blanks.Where(x => x.Key == ddlBlankGeneral.SelectedValue).SingleOrDefault().Value;

        if (blank == null)
            blank = new List<Option>();

        blank.Add(new Option() { 
            ID = blank.Count,
            OptionText = txtOptionText.Text,
            Selected = false
        });

        Blanks[ddlBlankGeneral.SelectedValue] = blank;

        ViewState["blanks"] = Blanks;


        txtOptionText.Text = string.Empty;

        BindOptions();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            ViewState["blanks"] = null;
    }
}