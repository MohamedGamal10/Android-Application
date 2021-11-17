using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace mazr3a
{
    class ActivityViewModel
    {
        public ObservableCollection <ActivityInfo> itemlist { get; set; }
        public ActivityViewModel()
        {
            
            itemlist = new ObservableCollection<ActivityInfo>();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"ID\", \"Activity_Code\", \"Activity\"FROM public.\"Activity\";";

                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    itemlist.Add(new ActivityInfo() { ActivityId = reader[0].ToString(), ActivityAct = reader[1].ToString(), ActivityCode = reader[2].ToString() });
       
                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }
            Xamarin.Forms.MessagingCenter.Subscribe<Activity_Add_New, ActivityInfo>(this, "Activity_Add_New", (page, activity) =>
              {
                  itemlist.Clear();
                  try
                  {
                      NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                      connection.Open();
                      NpgsqlCommand command = connection.CreateCommand();
                      command.CommandText = "SELECT \"ID\", \"Activity_Code\", \"Activity\"FROM public.\"Activity\";";

                      NpgsqlDataReader reader = command.ExecuteReader();
                      while (reader.Read())
                      {
                          itemlist.Add(new ActivityInfo() { ActivityId = reader[0].ToString(), ActivityAct = reader[1].ToString(), ActivityCode = reader[2].ToString() });

                      }
                      connection.Close();


                  }
                  catch (Exception ex)
                  {
                      string Error = ex.ToString();
                  }


              });
                Xamarin.Forms.MessagingCenter.Subscribe<Activity>(this, "UpdateActivity", (sender) =>
            {
                itemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Activity_Code\", \"Activity\"FROM public.\"Activity\";";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        itemlist.Add(new ActivityInfo() { ActivityId = reader[0].ToString(), ActivityAct = reader[1].ToString(), ActivityCode = reader[2].ToString() });

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
