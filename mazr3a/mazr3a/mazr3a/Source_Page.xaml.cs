using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mazr3a
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Source_Page : ContentPage
    {
        public Source_Page()
        {
            InitializeComponent();
            
        }

        private void Activity_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Activity());
            
        }

        private void Sector_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Sector());
        }

        private void Users_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Users());
        }

        private void Labor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Distribution());
        }

        private void Reports_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Reports());
        }
    }
}