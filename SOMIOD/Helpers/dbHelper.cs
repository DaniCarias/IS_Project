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

        //VERIFY IF APPLICATION EXISTS
        public static Boolean IsApplicationExist(string application_name)
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

        //GET APPLICATION ID
        public static long GetApplicationId(string application_name)
        {
            Boolean exists = IsApplicationExist(application_name);

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

        //CREATE APPLICATION
        public static Boolean CreateApplication (string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "INSERT INTO application (name) VALUES (@name)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
                return false;

            return true;
        }
        
        //DELETE APPLICATION
        public static Boolean DeleteApplication(string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "DELETE FROM application WHERE name ILIKE @name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
                return false;

            return true;
        }

        //UPDATE APPLICATION NAME
        public static Models.Application UpdateApplication(string newName, string oldName)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "UPDATE application SET name = @new_name WHERE name ILIKE @old_name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@new_name", newName);
            command.Parameters.AddWithValue("@old_name", oldName);

            int rows = command.ExecuteNonQuery();

            conn.Close();
            if (rows <= 0)
                return null;

            return GetApplication(newName);
        }

        //APPLICATION INFO
        public static Models.Application GetApplication(String name)
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

        //ALL APPLICATIONS
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

        #endregion


        #region Containers

        //VERIFY IF CONTAINER EXISTS
        public static Boolean IsContainerExist(string application_name)
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

        //GET CONTAINER ID
        public static long GetContainerId(string container_name)
        {
            Boolean exists = IsContainerExist(container_name);

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

        //CREATE CONTAINER
        public static void CreateContainer(string container_name, string application)
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
    
        //DELETE CONTAINER
        public static void DeleteContainer(string application, string container)
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

        //UPDATE CONTAINER NAME
        public static Models.Container UpdateContainer(string new_name, string application, string old_name)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetApplicationId(application);

            string str = "UPDATE container SET name=@new_name WHERE name ILIKE @old_name AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@new_name", new_name);
            command.Parameters.AddWithValue("@old_name", old_name);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }

            Models.Container cont = dbHelper.GetContainer(application, new_name);
            return cont;
        }

        //CONTAINER INFO
        public static Models.Container GetContainer(string application, string name)
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

        //CONTAINERS FROM AN APPLICATION
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

        //ALL CONTAINERS
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

        #endregion


        #region Data

        //CREATE DATA
        public static void CreateData(string content, string app, string container)
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

        //DELETE DATA
        public static void DeleteData(string application, string container, int dataId)
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

        //UPDATE DATA CONTENT
        public static Models.Data UpdateData(string application, string container, int dataId, string new_content)
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

            Models.Data data = dbHelper.GetData(application, container, dataId);
            return data;
        }

        //DATA INFO
        public static Models.Data GetData(string application, string container, int dataId)
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

        //ALL DATA
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

        //ALL DATA FROM APPLICATION
        public static List<Models.Data> GetDatas(string application, string container) 
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            List<Models.Container> containers_list = GetContainers(application);
            List<long> uniqueParentIds = containers_list.Select(c => c.Id).Distinct().ToList();

            string parentIdsString = string.Join(",", uniqueParentIds);

            string str = $"SELECT * FROM data WHERE parent IN ({parentIdsString})";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parentIdsString); ;

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

        //ALL DATA FROM CONTAINER
        public static List<Models.Data> GetDatas(string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

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

        #endregion


        #region Subscription

        //VERIFY IF SUBSCRIPTION EXISTS
        public static Boolean IsSubscriptionExist(string subscriptionName)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM subscription WHERE name=@name";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", subscriptionName);

            NpgsqlDataReader reader = command.ExecuteReader();
            Boolean exists = reader.HasRows;

            reader.Close();
            conn.Close();
            return exists;
        }

        //GET SUBSCRIPTION ID
        public static long GetSubscriptionId(string application, string container, string subscriptionName)
        {
            Boolean exists = IsSubscriptionExist(subscriptionName);

            if (!exists)
            {
                throw new Exception("Subscription do not exists");
            }

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

            string str = "SELECT id FROM subscription WHERE name=@name AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", subscriptionName);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();

            long id = (long)reader["id"];

            reader.Close();
            conn.Close();
            return id;
        }

        //CREATE SUBSCRIPTION
        public static void CreateSubscription(string name, string endPoint, string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

            string str = "INSERT INTO subscription (name, event, endpoint, parent) VALUES (@name, @event_type, @end_point, @parent)";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@event_type", "creation");
            command.Parameters.AddWithValue("@end_point", endPoint);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();
            if (rows <= 0)
            {
                throw new Exception("Error");
            }
        }

        //DELETE SUBSCRIPTION
        public static void DeleteSubscription(string application, string container, string subscriptionName)
        {
            long subscriptionId = GetSubscriptionId(application, container, subscriptionName);

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

        //UPDATE SUBSCRIPTION
        public static Models.Subscription UpdateSubscription(string application, string container, string subscriptionName, string new_name, string new_endPoint)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long subscriptionId = GetSubscriptionId(application, container, subscriptionName);
            long parent = GetContainerId(container);

            string str = "UPDATE subscription SET name = @new_name, endpoint = @new_endPoint WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@new_name", new_name);
            command.Parameters.AddWithValue("@new_endPoint", new_endPoint);
            command.Parameters.AddWithValue("@id", subscriptionId);
            command.Parameters.AddWithValue("@parent", parent);

            int rows = command.ExecuteNonQuery();
            conn.Close();

            if (rows <= 0)
            {
                throw new Exception("Error");
            }

            Models.Subscription subscription = dbHelper.GetSubscription(application, container, subscriptionId);
            return subscription;
        }

        //SUBSCRIPTION INFO
        public static Models.Subscription GetSubscription(string application, string container, long subscriptionId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

            string str = "SELECT * FROM subscription WHERE id=@id AND parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@id", subscriptionId);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();

            Models.Subscription subscription = new Models.Subscription
            {
                Id = (long)reader["id"],
                Name = (string)reader["name"],
                EventType = (string)reader["event"],
                Endpoint = (string)reader["endpoint"],
                Creation_dt = (DateTime)reader["creation_dt"],
                Parent = (long)reader["parent"]
            };

            reader.Close();
            conn.Close();
            return subscription;
        }

        //GET ALL SUBSCRIPTIONS
        public static List<Models.Subscription> GetAllSubscriptions()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string str = "SELECT * FROM subscription";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);

            NpgsqlDataReader reader = command.ExecuteReader();
            List<Models.Subscription> list = new List<Models.Subscription>();

            while (reader.Read())
            {
                Models.Subscription subscription = new Models.Subscription
                {
                    Id = (long)reader["id"],
                    Name = (string)reader["name"],
                    EventType = (string)reader["event"],
                    Endpoint = (string)reader["endpoint"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(subscription);
            }

            reader.Close();
            conn.Close();
            return list;
        }

        //GET SUBSCRIPTIONS OF A CONTAINER
        public static List<Models.Subscription> GetSubscriptions(string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            List<Models.Container> containers_list = GetContainers(application);
            List<long> uniqueParentIds = containers_list.Select(c => c.Id).Distinct().ToList();

            string parentIdsString = string.Join(",", uniqueParentIds);

            string str = $"SELECT * FROM subscription WHERE parent IN ({parentIdsString})";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parentIdsString);

            NpgsqlDataReader reader = command.ExecuteReader();
            List<Models.Subscription> list = new List<Models.Subscription>();
            while (reader.Read())
            {
                Models.Subscription subscription = new Models.Subscription
                {
                    Id = (long)reader["id"],
                    Name = (string)reader["name"],
                    EventType = (string)reader["event"],
                    Endpoint = (string)reader["endpoint"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(subscription);
            }

            reader.Close();
            conn.Close();
            return list;
        }

        //GET SUBSCRIPTIONS OF A CONTAINER
        public static List<Models.Subscription> GetSubscriptions(string application, string container)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            long parent = GetContainerId(container);

            string str = "SELECT * FROM subscription WHERE parent=@parent";
            NpgsqlCommand command = new NpgsqlCommand(str, conn);
            command.Parameters.AddWithValue("@parent", parent);

            NpgsqlDataReader reader = command.ExecuteReader();
            List<Models.Subscription> list = new List<Models.Subscription>();

            while (reader.Read())
            {
                Models.Subscription subscription = new Models.Subscription
                {
                    Id = (long)reader["id"],
                    Name = (string)reader["name"],
                    EventType = (string)reader["event"],
                    Endpoint = (string)reader["endpoint"],
                    Creation_dt = (DateTime)reader["creation_dt"],
                    Parent = (long)reader["parent"]
                };
                list.Add(subscription);
            }

            reader.Close();
            conn.Close();
            return list;
        }

        #endregion

    }
}