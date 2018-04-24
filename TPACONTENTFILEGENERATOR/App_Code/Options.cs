using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Options
/// </summary>
[Serializable]
public class Option
{

    public int ID { get; set; }
    public string OptionText { get; set; }
    public bool Selected { get; set; }

}