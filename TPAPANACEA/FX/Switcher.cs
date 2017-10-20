using System.Windows.Controls;

namespace TPA.CoreFramework
{
  	public static class Switcher
  	{
    	public static ActivitySwitcher activitySwitcher;

    	public static void Switch(UserControl newPage)
    	{
      		activitySwitcher.Navigate(newPage);
    	}

    	public static void Switch(UserControl newPage, object state)
    	{
      		activitySwitcher.Navigate(newPage, state);
    	}
  	}
}
