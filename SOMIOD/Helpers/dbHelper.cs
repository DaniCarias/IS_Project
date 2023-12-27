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

namespace SOMIOD.Helpers
{
    public class dbHelper
    {

        //Fazer connectionString para a bd postgres
        string connectionString = SOMIOD.Properties.Settings.Default.ConStr;

        #region Applications

        public static int getApplicationId(string name)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT id FROM application WHERE name=@name";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int id = (int)reader["id"];
            conn.Close();

            return id;
        }

        public static void createApplication (string name)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "INSERT INTO application (name, creation_dt) VALUES (@name, @creation_dt)";
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

        public static void deleteApplication(string name)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "DELETE FROM application WHERE name=@name";
            SqlCommand command = new SqlCommand(str, conn);
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

        public static List<Models.Application> GetAllApplications()
        {
            Models.Application app = new Models.Application();
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM Application (id, name, creation_dt, parent)";
            SqlCommand command = new SqlCommand(str, conn);
            SqlDataReader reader = command.ExecuteReader();

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

        

        #endregion


        #region Containers

        public static int getContainerId(string name)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT id FROM container WHERE name=@name";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            int id = (int)reader["id"];

            conn.Close();
            return id;
        }

        public static void createContainer(string container_name, string application)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "INSERT INTO container (name, creation_dt, parent) VALUES (@name, @creation_dt, @parent)";
            SqlCommand command = new SqlCommand(str, conn);
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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "DELETE FROM container WHERE name=@container && parent=@parent";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@container", container);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        public static Models.Container getContainer(string name)
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

        public static List<Models.Container> GetContainers(string application)
        {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM container WHERE parent=@parent";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            List<Models.Container> list = new List<Models.Container>();

            SqlDataReader reader = command.ExecuteReader();
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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM container (id, name, creation_dt, parent)";
            SqlCommand command = new SqlCommand(str, conn);

            List<Models.Container> list = new List<Models.Container>();

            SqlDataReader reader = command.ExecuteReader();
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

        #endregion


        #region Data

        public static void sendData(string content, string app, string container)
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

        public static void deleteData(string application, string container, int dataId)
       {
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "DELETE FROM data WHERE id=@id && parent=@parent";
            SqlCommand command = new SqlCommand(str, conn);
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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM data WHERE id=@id && parent=@parent";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", dataId);
            command.Parameters.AddWithValue("@parent", parent);

            SqlDataReader reader = command.ExecuteReader();

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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM data (id, content, creation_dt, parent)";
            SqlCommand command = new SqlCommand(str, conn);

            List<Models.Data> list = new List<Models.Data>();

            SqlDataReader reader = command.ExecuteReader();
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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getApplicationId(application);

            string str = "SELECT * FROM data WHERE parent=@parent";
            SqlCommand command = new SqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            List<Models.Data> list = new List<Models.Data>();

            SqlDataReader reader = command.ExecuteReader();
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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getContainerId(container);

            string str = "INSERT INTO subscription (name, event_type, end_point, creation_dt, parent) VALUES (@name, @event_type, @end_point, @creation_dt, @parent)";
            SqlCommand command = new SqlCommand(str, conn);
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
            SqlConnection conn = null;

            conn = new SqlConnection(connectionString);
            conn.Open();

            int parent = getContainerId(container);

            string str = "DELETE FROM subscription WHERE id=@id && parent=@parent";
            SqlCommand command = new SqlCommand(str, conn);
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