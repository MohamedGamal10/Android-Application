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
    public partial class Users : ContentPage
    {
        public Users()
        {
            InitializeComponent();
        }

        private void New_Users_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Users_Add_New());
        }

        private void SearchBar_Users_TextChanged(object sender, TextChangedEventArgs e)
        {
            var container = BindingContext as UsersViewModel;
            UsersList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                UsersList.ItemsSource = container.usersitemlist;
            else
                UsersList.ItemsSource = container.usersitemlist.Where(i => (i.Username.Contains(e.NewTextValue)));

            UsersList.EndRefresh();
        }

        private async void TapGestureRecognizer_Tapped_Edit(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string UserId = tappedEventArgs.Parameter.ToString();
            string Username = await DisplayPromptAsync("Username", "What's Username ?");
            string Password = await DisplayPromptAsync("Password", "What's Password ?");
            string Role = await DisplayPromptAsync("Role", "What's Role ?");

            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE public.\"Users\" SET \"Username\" = @Username, \"Password\" = @Password , \"Role\" = @Role WHERE \"ID\" = @UserId; ";
                command.Parameters.AddWithValue("Username", Username.ToString());
                command.Parameters.AddWithValue("Password", Password.ToString());
                command.Parameters.AddWithValue("Role", Role.ToString());
                command.Parameters.AddWithValue("UserId", int.Parse(UserId));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                await DisplayAlert("Success", "Updated Successfully", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "OK");

            }
            MessagingCenter.Send<Users>(this, "UpdateUsers");

        }

        private void TapGestureRecognizer_Tapped_Delete(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string UserId = tappedEventArgs.Parameter.ToString();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM public.\"Users\" WHERE \"ID\"=@UserId;";
                command.Parameters.AddWithValue("UserId", int.Parse(UserId));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                DisplayAlert("Success", "Deleted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }

            MessagingCenter.Send<Users>(this, "UpdateUsers");


        }
    }
}