using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mazr3a
{
    public class Sector_Add_New_ViewModel 
    {
        public SectorInfo sector { get; set; }
        public Sector_Add_New_ViewModel()
        {
            sector = new SectorInfo();
        }
    }
}