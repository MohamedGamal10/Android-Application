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
    public partial class Sector : ContentPage
    {
        public Sector()
        {
            InitializeComponent();
        }

        private void New_Sector_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Sector_Add_New());
        }

        private void SearchBar_Sector_TextChanged(object sender, TextChangedEventArgs e)
        {
            var container = BindingContext as SectorViewModel;
            SectorList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                SectorList.ItemsSource = container.sectoritemlist;
            else
                SectorList.ItemsSource = container.sectoritemlist.Where(i => (i.Sector.Contains(e.NewTextValue)) | (i.Owner.Contains(e.NewTextValue)) | (i.FarmName.Contains(e.NewTextValue)));

            SectorList.EndRefresh();
        }

        private async void TapGestureRecognizer_Tapped_Edit(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string sectorID = tappedEventArgs.Parameter.ToString();
            string FarmName = await DisplayPromptAsync("Farm Name", "What's Farm Name ?");
            string Sector = await DisplayPromptAsync("Sector", "What's Sector ?");
            string Owner = await DisplayPromptAsync("Owner", "What's Owner ?");

            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE public.\"Farm\" SET \"Farm_Name\"=@FarmName, \"Sector\"=@Sector, \"Owner\"=@Owner WHERE \"ID\" = @sector_ID; ";
                command.Parameters.AddWithValue("FarmName", FarmName.ToString());
                command.Parameters.AddWithValue("Sector", Sector.ToString());
                command.Parameters.AddWithValue("Owner", Owner.ToString());
                command.Parameters.AddWithValue("sector_ID", int.Parse(sectorID));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                await DisplayAlert("Success", "Updated Successfully", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "OK");

            }
            MessagingCenter.Send<Sector>(this, "UpdateSector");


        }

        private void TapGestureRecognizer_Tapped_Delete(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string sectorID = tappedEventArgs.Parameter.ToString();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM public.\"Farm\" WHERE \"ID\"=@sector_ID;";
                command.Parameters.AddWithValue("sector_ID", int.Parse(sectorID));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                DisplayAlert("Success", "Deleted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }

            MessagingCenter.Send<Sector>(this, "UpdateSector");


        }
    }
}