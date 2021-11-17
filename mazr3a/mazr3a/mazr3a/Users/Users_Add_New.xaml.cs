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
    public partial class Users_Add_New : ContentPage
    {
        public Users_Add_New()
        {
            InitializeComponent();
        }

        private void Add_New_Users_Clicked(object sender, EventArgs e)
        {
            string user = Username.Text;
            string pass = Password.Text;
            string ro = Role.SelectedItem.ToString();
            
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO public.\"Users\"(\"Username\",\"Password\",\"Role\")VALUES(@Username, @Password, @Role); ";
                command.Parameters.AddWithValue("Username", user);
                command.Parameters.AddWithValue("Password", pass);
                command.Parameters.AddWithValue("Role", ro);
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
                DisplayAlert("Success", "Inserted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }

            UsersInfo users = ((Users_Add_New_ViewModel)BindingContext).users;
            MessagingCenter.Send(this, "Users_Add_New", users);


        }
    }
}