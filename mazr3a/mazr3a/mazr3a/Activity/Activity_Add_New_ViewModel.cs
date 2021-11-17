using System;
using System.Collections.Generic;
using System.Text;

namespace mazr3a
{
   public class Activity_Add_New_ViewModel
    {
        public ActivityInfo activity { get; set; }
        public Activity_Add_New_ViewModel()
        {
            activity = new ActivityInfo();
        }
    }
}
