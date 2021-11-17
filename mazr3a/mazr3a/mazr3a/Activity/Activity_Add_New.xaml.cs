using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Npgsql;

namespace mazr3a
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Activity_Add_New : ContentPage
    {
        public Activity_Add_New()
        {
            InitializeComponent();
        }

        private void Add_New_Acticity_Clicked(object sender, EventArgs e)
        {
            
            string Act_code = Activity_Code.Text;
            string Act = Activity.Text;

            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO public.\"Activity\"(\"Activity_Code\",\"Activity\")VALUES(@Act_code, @Act); ";
                command.Parameters.AddWithValue("Act_code", Act_code);
                command.Parameters.AddWithValue("Act", Act);
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
                DisplayAlert("Success", "Inserted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
                
            }

            ActivityInfo activity = ((Activity_Add_New_ViewModel)BindingContext).activity;
            MessagingCenter.Send(this, "Activity_Add_New", activity);
            //Navigation.PushModalAsync(new Activity());
        }
    }
}