using Newtonsoft.Json.Linq;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Npgsql;
using System.Collections;

namespace SOMIOD.Helpers
{
    public class dbHelper
    {

        static string host = "localhost";
        static string database = "projeto_is";
        static string username = "projeto_is";
        static string password = "password";
        static int port = 5432;

        static string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";

        #region Applications

        public static int getApplicationId(string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT id FROM application WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            NpgsqlDataReader reader = command.ExecuteReader();
            //reader.Read();

            int id = (int)reader["id"];
            conn.Close();

            return id;
        }

        public static void createApplication (string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "INSERT INTO application (name) VALUES (@name)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }
        
        public static void deleteApplication(string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "DELETE FROM application WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            int rows = command.ExecuteNonQuery();
            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        public static Models.Application getApplication(String name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM Application WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            NpgsqlDataReader reader = command.ExecuteReader();

            reader.Read();
            Models.Application app = new Models.Application
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                Creation_dt = (DateTime)reader["creation_dt"],
            };

            reader.Close();
            return app;
        }

        public static List<Models.Application> GetAllApplications()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM Application";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            NpgsqlDataReader reader = command.ExecuteReader();

            List<Models.Application> list = new List<Models.Application>();
            while (reader.Read())
            {

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

        public static Models.Application updateApplication(string newName, string oldName) //REVER PAIR PROGRAMMING
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "UPDATE application SET name = @new_name WHERE name ILIKE @old_name"; //SE FOR HARD CODED FUNCIONA - PAIR_PROGRAMMING
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@new_name", newName);
            command.Parameters.AddWithValue("@old_name", oldName);

            int rows = command.ExecuteNonQuery();

            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
            return getApplication(newName);
        }

        #endregion


        #region Containers

        public static int getContainerId(string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT id FROM container WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();

            int id = (int)reader["id"];

            conn.Close();
            return id;
        }

        public static void createContainer(string container_name, string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "INSERT INTO container (id, name, creation_dt, parent) VALUES (@id, @name, @creation_dt, @parent)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", 2);
            command.Parameters.AddWithValue("@name", container_name);
            command.Parameters.AddWithValue("@creation_dt",DateTime.Now);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
               throw new Exception("Error");
            }

        } 
    
        public static void deleteContainer(string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "DELETE FROM container WHERE name ILIKE @container AND parent = @parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@container", container);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        public static Models.Container getContainer(string application, string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM container WHERE name ILIKE @name AND parent = @parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();

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

        public static List<Models.Container> GetContainers(string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM container WHERE parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            List<Models.Container> list = new List<Models.Container>();

            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.Container container = new Models.Container
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (int)reader["parent"]
                };
                list.Add(container);
            }
            reader.Close();
            return list;
        }

        public static List<Models.Container> GetAllContainers()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM container";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);

            List<Models.Container> list = new List<Models.Container>();

            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.Container container = new Models.Container
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (int)reader["parent"]
                };
                list.Add(container);
            }
            reader.Close();
            return list;
        }

        public static Models.Container updateContainer(string name, string application, string container) //REVER PAIR PROGRAMMING
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "UPDATE container SET name=@name WHERE name ILIKE @container AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@container", container);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }

            Models.Container cont = dbHelper.getContainer(application, container);
            return cont; //enviar por XML
        }

        #endregion


        #region Data

        public static void sendData(string content, string app, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getContainerId(container);

            string str = "INSERT INTO data (content, creation_dt, parent) VALUES (@content, @creation_dt, @parent)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
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

        public static void deleteData(string application, string container, int dataId)
       {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "DELETE FROM data WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", dataId);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        public static Models.Data getData(string application, string container, int dataId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM data WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", dataId);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();

            Models.Data data = new Models.Data
            {
                Id = (int)reader["id"],
                Content = (string)reader["content"],
                Creation_dt = (DateTime)reader["creation_dt"],
                Parent = (int)reader["parent"]
            };

            reader.Close();
            return data;
        }

        public static List<Models.Data> GetAllDatas()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM data";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);

            List<Models.Data> list = new List<Models.Data>();

            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.Data data = new Models.Data
                {
                    Id = (int)reader["id"],
                    Content = (string)reader["content"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (int)reader["parent"]
                };
                list.Add(data);
            }
            reader.Close();
            return list;
        }

        public static List<Models.Data> GetDatas(string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM data WHERE parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            List<Models.Data> list = new List<Models.Data>();

            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.Data data = new Models.Data
                {
                    Id = (int)reader["id"],
                    Content = (string)reader["content"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (int)reader["parent"]
                };
                list.Add(data);
            }
            reader.Close();
            return list;
        }

        #endregion


        #region Subscription

        public static void createSubscription(string name, string eventType, string endPoint, string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getContainerId(container);

            string str = "INSERT INTO subscription (name, event_type, end_point, creation_dt, parent) VALUES (@name, @event_type, @end_point, @creation_dt, @parent)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@event_type", eventType);
            command.Parameters.AddWithValue("@end_point", endPoint);
            command.Parameters.AddWithValue("@creation_dt", DateTime.Now);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        public static void deleteSubscription(string application, string container, int subscriptionId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            int parent = getContainerId(container);

            string str = "DELETE FROM subscription WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", subscriptionId);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        #endregion
        
    }
}