using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mazr3a
{
    class DistributionViewModel
    {
        public ObservableCollection<DistributionInfo> distributionitemlist { get; set; }
        public DistributionViewModel()
        {

            distributionitemlist = new ObservableCollection<DistributionInfo>();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"ID\", \"Date\" ,\"Farm_Name\",\"Sector\",\"Activity\" FROM public.\"Distribution\" ORDER BY \"Date\" DESC;";

                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    distributionitemlist.Add(new DistributionInfo() { DistributionId = reader[0].ToString(), DistributionDate = Convert.ToDateTime(reader[1].ToString()).ToShortDateString(), DistributionFarmName = reader[2].ToString(), DistributionSector = reader[3].ToString(), DistributionActivity = reader[4].ToString() }); ;

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }
            Xamarin.Forms.MessagingCenter.Subscribe<Distribution_Add_New, DistributionInfo>(this, "Distribution_Add_New", (page, distribution) =>
            {
                distributionitemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Date\" ,\"Farm_Name\",\"Sector\",\"Activity\" FROM public.\"Distribution\" ORDER BY \"Date\" DESC;";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        distributionitemlist.Add(new DistributionInfo() { DistributionId = reader[0].ToString(), DistributionDate = Convert.ToDateTime(reader[1].ToString()).ToShortDateString(), DistributionFarmName = reader[2].ToString(), DistributionSector = reader[3].ToString(), DistributionActivity = reader[4].ToString() }); ;

                    }
                    connection.Close();


                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                }


            });
            Xamarin.Forms.MessagingCenter.Subscribe<Distribution>(this, "UpdateDistribution", (sender) =>
            {
                distributionitemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Date\" ,\"Farm_Name\",\"Sector\",\"Activity\" FROM public.\"Distribution\" ORDER BY \"Date\" DESC;";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        distributionitemlist.Add(new DistributionInfo() { DistributionId = reader[0].ToString(), DistributionDate = Convert.ToDateTime(reader[1].ToString()).ToShortDateString(), DistributionFarmName = reader[2].ToString(), DistributionSector = reader[3].ToString(), DistributionActivity = reader[4].ToString() }); ;

                    }
                    connection.Close();


                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                }

            });

            Xamarin.Forms.MessagingCenter.Subscribe<Distribution_Update>(this, "UpdateDistribution", (sender) =>
            {
                distributionitemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Date\" ,\"Farm_Name\",\"Sector\",\"Activity\" FROM public.\"Distribution\" ORDER BY \"Date\" DESC;";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        distributionitemlist.Add(new DistributionInfo() { DistributionId = reader[0].ToString(), DistributionDate = Convert.ToDateTime(reader[1].ToString()).ToShortDateString(), DistributionFarmName = reader[2].ToString(), DistributionSector = reader[3].ToString(), DistributionActivity = reader[4].ToString() }); ;

                    }
                    connection.Close();


                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                }

            });


        }
    }
}