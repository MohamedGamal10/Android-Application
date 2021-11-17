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
    public partial class Distribution_Update : ContentPage
    {
        public Distribution_Update(string DistributionId)
        {
            InitializeComponent();
            string DID = DistributionId;
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            var Activity_Code = new List<string>();
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT \"Activity_Code\"FROM public.\"Activity\";";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Activity_Code.Add(reader[0].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }
            DistributionActivityCode.ItemsSource = Activity_Code;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            var FarmName = new List<string>();

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT \"Farm_Name\" FROM public.\"Farm\";";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    FarmName.Add(reader[0].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }
             DistributionFarmName.ItemsSource = FarmName;
            //////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"Date\", \"Activity_Code\", \"Activity\", \"Farm_Name\", \"Sector\", \"No.of_labor\", \"Unit_Rate\", \"Total\", \"ID\" FROM public.\"Distribution\" WHERE \"ID\" = @DistributionId;";
                command.Parameters.AddWithValue("DistributionId", Convert.ToInt32(DistributionId));
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //date = reader[0].ToString();
                    DistributionDate.Date = Convert.ToDateTime(reader[0].ToString());
                    DistributionActivityCode.SelectedItem = reader[1].ToString();
                    DistributionActivity.SelectedItem = reader[2].ToString();
                    DistributionFarmName.SelectedItem = reader[3].ToString();
                    DistributionSector.SelectedItem = reader[4].ToString();
                    DistributionNoOfLabor.Text = reader[5].ToString();
                    DistributionUnitRate.Text = reader[6].ToString();
                    DistributionTotal.Text = reader[7].ToString();
                    DistributionID.Text = reader[8].ToString();

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();

            }


        }

        private void refresh_activity(object sender, EventArgs e)
        {
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";

            var Activity = new List<string>();

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT \"Activity\"FROM public.\"Activity\" WHERE \"Activity_Code\" = @DistributionActivityCode;";
                command.Parameters.AddWithValue("DistributionActivityCode", DistributionActivityCode.SelectedItem.ToString());
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Activity.Add(reader[0].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();

            }
            DistributionActivity.ItemsSource = Activity;


        }

        private void refresh_Sector(object sender, EventArgs e)
        {
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";

            var Sector = new List<string>();

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT \"Sector\"FROM public.\"Farm\" WHERE \"Farm_Name\" = @DistributionFarmName;";
                command.Parameters.AddWithValue("DistributionFarmName", DistributionFarmName.SelectedItem.ToString());
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Sector.Add(reader[0].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();

            }
            DistributionSector.ItemsSource = Sector;
        }

        private void refresh_Total(object sender, EventArgs e)
        {
            int Labor = Convert.ToInt32(DistributionNoOfLabor.Text.ToString());
            double unitrate = Convert.ToDouble(DistributionUnitRate.Text.ToString());
            double result = Labor * unitrate;
            DistributionTotal.Text = result.ToString();
        }

        private void Update_Distribution_Clicked(object sender, EventArgs e)
        {
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE public.\"Activity\" SET \"Activity_Code\" = @Act_code, \"Activity\" = @Act WHERE \"ID\" = @Act_ID; ";
                command.CommandText = "UPDATE public.\"Distribution\" SET \"Date\"=@Date, \"Activity_Code\"=@Activity_Code, \"Activity\"=@Activity, \"Farm_Name\"=@Farm_Name, \"Sector\"=@Sector, \"No.of_labor\"=@No.of_labor, \"Unit_Rate\"=@Unit_Rate, \"Total\"=@Total WHERE \"ID\"=@DistributionId;";
                command.Parameters.AddWithValue("Date", Convert.ToDateTime(DistributionDate.Date.ToShortDateString()));
                command.Parameters.AddWithValue("Activity_Code", DistributionActivityCode.SelectedItem.ToString());
                command.Parameters.AddWithValue("Activity", DistributionActivity.SelectedItem.ToString());
                command.Parameters.AddWithValue("Farm_Name", DistributionFarmName.SelectedItem.ToString());
                command.Parameters.AddWithValue("Sector", DistributionSector.SelectedItem.ToString());
                command.Parameters.AddWithValue("No.of_labor", Convert.ToInt32(DistributionNoOfLabor.Text.ToString()));
                command.Parameters.AddWithValue("Unit_Rate", Convert.ToDouble(DistributionUnitRate.Text.ToString()));
                command.Parameters.AddWithValue("Total", Convert.ToDouble(DistributionTotal.Text.ToString()));
                command.Parameters.AddWithValue("DistributionId", Convert.ToInt32(DistributionID.Text.ToString()));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();

                DisplayAlert("Success", "Updated Successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");

            }
            MessagingCenter.Send<Distribution_Update>(this, "UpdateDistribution");

        }

        private void DistributionActivityCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";

            var Activity = new List<string>();
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT \"Activity\"FROM public.\"Activity\" WHERE \"Activity_Code\" =@Activity_Code;";
                command.Parameters.AddWithValue("Activity_Code", DistributionActivityCode.SelectedItem.ToString());

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Activity.Add(reader[0].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
                
            }
            DistributionActivity.ItemsSource = Activity;
        }

        private void DistributionFarmName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";

            var Sector = new List<string>();
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT \"Sector\"FROM public.\"Farm\" WHERE \"Farm_Name\" = @DistributionFarmName;";
                command.Parameters.AddWithValue("DistributionFarmName", DistributionFarmName.SelectedItem.ToString());
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Sector.Add(reader[0].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();

            }
            DistributionSector.ItemsSource = Sector;

        }
    }
}