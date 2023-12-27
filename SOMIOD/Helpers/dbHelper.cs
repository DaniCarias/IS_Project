using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SOMIOD.Helpers
{
    public class dbHelper
    {

        //Fazer connectionString para a bd postgres
        string connectionString = SOMIOD.Properties.Settings.Default.ConStr;

        public static void createApp (string name)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "INSERT INTO application (name, creation_dt) VALUES (@id, @name, @creation_dt)";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@creation_dt",DateTime.Now);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        public static void createContainer(String name, int parent)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "INSERT INTO container (name, creation_dt, parent) VALUES (@name, @creation_dt, @parent)";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@creation_dt",DateTime.Now);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
               throw new Exception("Error");
            }

        }
    
    
        public static void sendData(String content, string app, string container)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = ;

            string str = "INSERT INTO data (content, creation_dt, parent) VALUES (@content, @creation_dt, @parent)";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@content", content);
            command.Parameters.AddWithValue("@creation_dt", DateTime.Now);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }

        }
    
    
        public static Models.Application getApplication(String name)
        {
            Models.Application app = new Models.Application();
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM Application (id, name, creation_dt, parent) WHERE name=@name";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            Models.Application a = new Models.Application
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                };

            reader.Close();
            return app;
        }


        public static Models.Container getContainer(String name)
        {
            Models.Application app = new Models.Application();
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM container (id, name, creation_dt, parent) WHERE name=@name";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = command.ExecuteReader();

            Models.Container container = new Models.Container
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                Creation_dt = (DateTime)reader["creation_dt"],
                Parent = (int)reader["parent"]
            };
            
            reader.Close();
            return container;
        }


        public static List<Models.Application> GetApplications()
        {
            Models.Application app = new Models.Application();
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM Application (id, name, creation_dt, parent)";
            SqlCommand command = new SqlCommand(str, conn);
            SqlDataReader reader = command.ExecuteReader();

            List<Models.Application> list = new List<Models.Application>();
            while (reader.Read()) { 

                Models.Application a = new Models.Application
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                };
                list.Add(a);

            }
            reader.Close();
            return list;
        }
    }
}