using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mazr3a
{
    public class Users_Add_New_ViewModel 
    {
        public UsersInfo users { get; set; }
        public Users_Add_New_ViewModel()
        {
            users = new UsersInfo();
        }
    }
}