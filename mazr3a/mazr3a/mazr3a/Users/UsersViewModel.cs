using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mazr3a
{
    class UsersViewModel
    {
        public ObservableCollection<UsersInfo> usersitemlist { get; set; }
        public UsersViewModel()
        {
            usersitemlist = new ObservableCollection<UsersInfo>();
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"ID\", \"Username\", \"Password\", \"Role\" FROM public.\"Users\";";

                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usersitemlist.Add(new UsersInfo() { UserId = reader[0].ToString(), Username = reader[1].ToString(), Password = reader[2].ToString(), Role = reader[3].ToString() });

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
            }
            Xamarin.Forms.MessagingCenter.Subscribe<Users_Add_New, UsersInfo>(this, "Users_Add_New", (page, users) =>
            {
                usersitemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Username\", \"Password\", \"Role\" FROM public.\"Users\";";

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        usersitemlist.Add(new UsersInfo() { UserId = reader[0].ToString(), Username = reader[1].ToString(), Password = reader[2].ToString(), Role = reader[3].ToString() });

                    }
                    connection.Close();


                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                }


            });
            Xamarin.Forms.MessagingCenter.Subscribe<Users>(this, "UpdateUsers", (sender) =>
            {
                usersitemlist.Clear();
                try
                {
                    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT \"ID\", \"Username\", \"Password\", \"Role\" FROM public.\"Users\";"; ;

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        usersitemlist.Add(new UsersInfo() { UserId = reader[0].ToString(), Username = reader[1].ToString(), Password = reader[2].ToString(), Role = reader[3].ToString() });

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