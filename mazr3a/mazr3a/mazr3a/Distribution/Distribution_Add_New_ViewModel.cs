using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mazr3a
{
    public class Distribution_Add_New_ViewModel
    {
        public DistributionInfo distribution { get; set; }
        public Distribution_Add_New_ViewModel()
        {
            distribution = new DistributionInfo();
        }
    }
}