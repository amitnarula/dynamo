using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TPA.Entities
{

    public class Option
    {
        public string Id { get; set; }
        public string OptionText { get; set; }
        public bool IsSelected { get; set; }
        //public string SelectedColor { get { return this.IsSelected ? Colors.Yellow.ToString() : null; } }
        public string SelectedColor { get { return this.IsSelected ? string.Empty : null; } } //Panacea
    }

}
