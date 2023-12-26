using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Text;
using Container = SOMIOD.Models.Container;
using Application = SOMIOD.Models.Application;
using System.Windows.Input;
using System.Web.UI.WebControls.WebParts;
using System.Collections;



namespace SOMIOD.Controller
{
    [RoutePrefix("api/somiod")]
    public class Somiod : ApiController
    {

        string connectionString = SOMIOD.Properties.Settings.Default.ConStr;

        //Cria o BROKEN mosquito
        MqttClient mClient = new MqttClient(IPAddress.Parse("127.0.0.1")); //OR use the broker hostname

        [HttpPost]
        [Route("createApplication")]
        public IHttpActionResult PostApplication([FromBody] Application a)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string str = "INSERT INTO application (id, name, creation_dt) VALUES (@id, @name, @creation_dt)";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@id", a.Id);
                command.Parameters.AddWithValue("@name", a.Name);
                command.Parameters.AddWithValue("@creation_dt", a.Creation_dt);


                int rows = command.ExecuteNonQuery();
                conn.Close();
                if (rows <= 0)
                {
                    return BadRequest("Error");
                }

                return Ok(a);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("createContainers")]
        public IHttpActionResult PostContainers([FromBody] Container c)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string str = "INSERT INTO container (id ,name, creation_dt, parent) VALUES (@id, @name, @creation_dt, @parent)";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@id", c.Id);
                command.Parameters.AddWithValue("@name", c.Name);
                command.Parameters.AddWithValue("@creation_dt", c.Creation_dt);
                command.Parameters.AddWithValue("@parent", c.Parent);

                int rows = command.ExecuteNonQuery();
                conn.Close();
                if (rows <= 0)
                {
                    return BadRequest("Error");
                }

                return Ok(c);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("sendDataToBroker")]
        public IHttpActionResult sendDataToBroker([FromBody] Application c)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                /*
                string str = "INSERT INTO data (id, name, creation_dt, parent) VALUES (@id, @name, @creation_dt, @parent)";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@id", c.Id);
                command.Parameters.AddWithValue("@name", c.Name);
                command.Parameters.AddWithValue("@creation_dt", c.Creation_dt);
                command.Parameters.AddWithValue("@parent", c.Parent);
                */

                int rows = command.ExecuteNonQuery();
                conn.Close();

                if (rows <= 0)
                {
                    return BadRequest("Error");
                }
                else if(mClient.IsConnected)
                {
                    //string selectedTopic = comboBox1.SelectedItem.ToString();
                    //string textFromTextBox = textBox1.Text;
                    //envia a mensagem para o broker com o topico que foi selecionado e a mensagem que foi escrita
                    mClient.Publish(/* canal */"canal1", /* MSG */ Encoding.UTF8.GetBytes("123"));
                }

                return Ok(c);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("{application}/{container}")]
        public IEnumerable<Container> getContainer( string app, string container)
        {
            List<Container> containers = new List<Container>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string str = "SELECT * FROM container (id, name, creation_dt, parent) WHERE name=@container";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@container", container);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Container c = new Container
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["name"],
                        Creation_dt = (DateTime)reader["creation_dt"],
                        Parent = (int)reader["parent"]
                    };
                    containers.Add(c);
                }

                reader.Close();
                return containers;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        [HttpGet]
        [Route("{application}")]
        public IEnumerable<Application> getApplication(string app)
        {
            List<Application> applications = new List<Application>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string str = "SELECT * FROM Application (id, name, creation_dt, parent) WHERE name=@app";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@app", app);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Application a = new Application
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["name"],
                        Creation_dt = (DateTime)reader["creation_dt"],
                    };
                    applications.Add(a);
                }
                reader.Close();
                return applications;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}