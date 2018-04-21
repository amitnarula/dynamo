using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for BaseUserControl
/// </summary>
public class BaseUserControl : UserControl
{
    public int Position { get; set; }

    public virtual void Reset() { }

	public BaseUserControl()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}

public class PublisherSubscriberEventArgs : EventArgs
{ 

}