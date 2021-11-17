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
    public partial class Distribution : ContentPage
    {
        public Distribution()
        {
            InitializeComponent();
        }

        private void SearchBar_Distribution_TextChanged(object sender, TextChangedEventArgs e)
        {
            var container = BindingContext as DistributionViewModel;
            DistributionList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                DistributionList.ItemsSource = container.distributionitemlist;
            else
                DistributionList.ItemsSource = container.distributionitemlist.Where(i => (i.DistributionDate.Contains(e.NewTextValue)));

            DistributionList.EndRefresh();
        }

        private void TapGestureRecognizer_Tapped_Edit(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string DistributionId = tappedEventArgs.Parameter.ToString();
            Navigation.PushModalAsync(new Distribution_Update(DistributionId));
        }

        private void TapGestureRecognizer_Tapped_Delete(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            string DistributionId = tappedEventArgs.Parameter.ToString();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM public.\"Distribution\" WHERE \"ID\"=@DistributionId;";
                command.Parameters.AddWithValue("DistributionId", int.Parse(DistributionId));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                DisplayAlert("Success", "Deleted Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }

            MessagingCenter.Send<Distribution>(this, "UpdateDistribution");

        }

        private void New_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Distribution_Add_New());
        }
    }
}