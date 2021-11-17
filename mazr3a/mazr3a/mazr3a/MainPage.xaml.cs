using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mazr3a
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Username.Text == "Mohamed" & Password.Text == "123")
            {
                Navigation.PushModalAsync(new Source_Page());
            }
            else
            {
                DisplayAlert("Error", "Please Check Username Or Password", "OK");
            }
        }
    }
}
