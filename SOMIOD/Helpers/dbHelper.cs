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

        public static Boolean isApplicationExist(string application_name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM application WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", application_name);

            NpgsqlDataReader reader = command.ExecuteReader();
            Boolean exists = reader.HasRows;

            conn.Close();
            return exists;
        }

        public static long GetApplicationId(string application_name)
        {
            Boolean exists = isApplicationExist(application_name);

            if (!exists)
            {
                throw new Exception("Application do not exists");
            }

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT id FROM application WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", application_name);

            NpgsqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                throw new Exception("Application with the requested ID does not exist!");

            reader.Read();

            long id = (long)reader["id"];

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

            long id = (long)reader["id"];
            string namesss = (string)reader["name"];
            DateTime creation_dt = (DateTime)reader["creation_dt"];

            Models.Application app = new Models.Application
            {
                Id = (long)reader["id"],
                Name = (string)reader["name"],
                Creation_dt = (DateTime)reader["creation_dt"],
            };

            reader.Close();
            conn.Close();
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
                    Id = (long)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                };
                list.Add(a);

            }
            reader.Close();
            conn.Close();
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

        public static Boolean isContainerExist(string application_name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM container WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", application_name);

            NpgsqlDataReader reader = command.ExecuteReader();
            Boolean exists = reader.HasRows;

            conn.Close();
            return exists;
        }

        public static long GetContainerId(string container_name)
        {
            Boolean exists = isContainerExist(container_name);

            if (!exists)
            {
                throw new Exception("Container do not exists");
            }

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT id FROM container WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", container_name);

            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();

            long id = (long)reader["id"];

            conn.Close();
            return id;
        }

        public static void createContainer(string container_name, string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetApplicationId(application);

            string str = "INSERT INTO container (name, parent) VALUES (@name, @parent)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", container_name);
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

            long parent = GetApplicationId(application);

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

            long parent = GetApplicationId(application);

            string str = "SELECT * FROM container WHERE name ILIKE @name AND parent = @parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();

            Models.Container container = new Models.Container
            {
                Id = (long)reader["id"],
                Name = (string)reader["name"],
                Creation_dt = (DateTime)reader["creation_dt"],
                Parent = (long)reader["parent"]
            };

            reader.Close();
            conn.Close();

            return container;
        }

        public static List<Models.Container> GetContainers(string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetApplicationId(application);

            string str = "SELECT * FROM container WHERE parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            List<Models.Container> list = new List<Models.Container>();

            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.Container container = new Models.Container
                {
                    Id = (long)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(container);
            }
            reader.Close();
            conn.Close();
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
                    Id = (long)reader["id"],
                    Name = (string)reader["name"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(container);
            }
            reader.Close();
            conn.Close();
            return list;
        }

        public static Models.Container updateContainer(string new_container, string application, string old_container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetApplicationId(application);

            string str = "UPDATE container SET name=@name WHERE name ILIKE @container AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", new_container);
            command.Parameters.AddWithValue("@container", old_container);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }

            Models.Container cont = dbHelper.getContainer(application, new_container);
            return cont; //enviar por XML
        }

        #endregion


        #region Data

        public static void sendData(string content, string app, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

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

            long parent = GetApplicationId(application);

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

            long parent = GetContainerId(container);

            string str = "SELECT * FROM data WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", dataId);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();

            Models.Data data = new Models.Data
            {
                Id = (long)reader["id"],
                Content = (string)reader["content"],
                Creation_dt = (DateTime)reader["creation_dt"],
                Parent = (long)reader["parent"]
            };

            reader.Close();
            conn.Close();
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
                    Id = (long)reader["id"],
                    Content = (string)reader["content"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(data);
            }
            reader.Close();
            conn.Close();
            return list;
        }

        public static List<Models.Data> GetDatas(string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetApplicationId(application);

            string str = "SELECT * FROM data WHERE parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            List<Models.Data> list = new List<Models.Data>();

            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.Data data = new Models.Data
                {
                    Id = (long)reader["id"],
                    Content = (string)reader["content"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(data);
            }
            reader.Close();
            conn.Close();
            return list;
        }

        public static Models.Data updateData(string application, string container, int dataId, string new_content)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

            string str = "UPDATE data SET content = @new_content WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@new_content", new_content);
            command.Parameters.AddWithValue("@id", dataId);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }

            Models.Data data = dbHelper.getData(application, container, dataId);
            return data; //enviar por XML
        }

        #endregion


        #region Subscription

        public static void createSubscription(string name, string eventType, string endPoint, string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

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

            long parent = GetContainerId(container);

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