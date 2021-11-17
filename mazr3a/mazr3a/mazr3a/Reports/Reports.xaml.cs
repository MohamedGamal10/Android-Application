using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mazr3a
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reports : ContentPage
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void Labor_Reports_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Labor_Reports());
        }
    }
}