using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mazr3a
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sector_Add_New : ContentPage
    {
        public Sector_Add_New()
        {
            InitializeComponent();

            //ArrayList user = new ArrayList();
            var user = new List<string>();
            
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"Username\" FROM public.\"Users\";";

                NpgsqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    user.Add(reader[0].ToString());
                    
                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }

            Owner.ItemsSource = user;
        }

        private void Add_New_Acticity_Clicked(object sender, EventArgs e)
        {
            string FarmN = FarmName.Text;
            string Sec = Sector.Text;
            string own = Owner.SelectedItem.ToString();

            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO public.\"Farm\"(\"Farm_Name\", \"Sector\", \"Owner\")VALUES(@FarmN, @Sec, @own); ";
                command.Parameters.AddWithValue("FarmN", FarmN);
                command.Parameters.AddWithValue("Sec", Sec);
                command.Parameters.AddWithValue("own", own);
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
                DisplayAlert("Success", "Inserted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }

            SectorInfo sector = ((Sector_Add_New_ViewModel)BindingContext).sector;
            MessagingCenter.Send(this, "Sector_Add_New", sector);
            

        }
    }
}