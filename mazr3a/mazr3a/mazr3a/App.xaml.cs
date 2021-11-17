using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mazr3a
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Source_Page();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
