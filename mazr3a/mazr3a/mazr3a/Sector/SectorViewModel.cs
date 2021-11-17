using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mazr3a
{
    class SectorViewModel
    {
        public ObservableCollection<SectorInfo> sectoritemlist { get; set; }
        public SectorViewModel()
        {

            sectoritemlist = new ObservableCollection<SectorInfo>();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"ID\", \"Farm_Name\", \"Sector\", \"Owner\"FROM public.\"Farm\";";

                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sectoritemlist.Add(new SectorInfo() { SectorId = reader[0].ToString(), FarmName = reader[1].ToString(), Sector = reader[2].ToString(), Owner = reader[3].ToString() });

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }
            Xamarin.Forms.MessagingCenter.Subscribe<Sector_Add_New, SectorInfo>(this, "Sector_Add_New", (page, activity) =>
            {
                sectoritemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Farm_Name\", \"Sector\", \"Owner\"FROM public.\"Farm\";";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sectoritemlist.Add(new SectorInfo() { SectorId = reader[0].ToString(), FarmName = reader[1].ToString(), Sector = reader[2].ToString(), Owner = reader[3].ToString() });

                    }
                    connection.Close();


                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                }


            });
            Xamarin.Forms.MessagingCenter.Subscribe<Sector>(this, "UpdateSector", (sender) =>
            {
                sectoritemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Farm_Name\", \"Sector\", \"Owner\"FROM public.\"Farm\";";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sectoritemlist.Add(new SectorInfo() { SectorId = reader[0].ToString(), FarmName = reader[1].ToString(), Sector = reader[2].ToString(), Owner = reader[3].ToString() });

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
