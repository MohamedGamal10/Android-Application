using Npgsql;
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
    public partial class Activity : ContentPage
    {
        public Activity()
        {
            InitializeComponent();
            BindingContext = new ActivityViewModel();
            

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var container = BindingContext as ActivityViewModel;
            ActivityList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ActivityList.ItemsSource = container.itemlist;
            else
                ActivityList.ItemsSource = container.itemlist.Where(i => (i.ActivityCode.Contains(e.NewTextValue))|(i.ActivityAct.Contains(e.NewTextValue)));

            ActivityList.EndRefresh();
        }

        private void New_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Activity_Add_New());
        }

        private async void TapGestureRecognizer_Tapped_Edit(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string activityID = tappedEventArgs.Parameter.ToString();
            string ActivityCode = await DisplayPromptAsync("ActivityCode", "What's ActivityCode ?");
            string Activity = await DisplayPromptAsync("Activity", "What's Activity ?");

            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE public.\"Activity\" SET \"Activity_Code\" = @Act_code, \"Activity\" = @Act WHERE \"ID\" = @Act_ID; ";
                command.Parameters.AddWithValue("Act_code", ActivityCode.ToString());
                command.Parameters.AddWithValue("Act", Activity.ToString());
                command.Parameters.AddWithValue("Act_ID", int.Parse(activityID));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
             
                await DisplayAlert("Success", "Updated Successfully", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "OK");

            }
            MessagingCenter.Send<Activity>(this, "UpdateActivity");

        }

        private void TapGestureRecognizer_Tapped_Delete(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string activityID = tappedEventArgs.Parameter.ToString();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM public.\"Activity\" WHERE \"ID\"=@Act_ID;";
                command.Parameters.AddWithValue("Act_ID", int.Parse(activityID));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                DisplayAlert("Success", "Deleted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }
            
            MessagingCenter.Send<Activity>(this, "UpdateActivity");

        }
    }
}
