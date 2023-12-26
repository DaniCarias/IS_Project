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


namespace SOMIOD.Controller
{
    [RoutePrefix("api/somiod")]
    public class Somiod : ApiController
    {

        //string connectionString = SOMIOD.Properties.Settings.Default.ConStr;

        //Cria o BROKEN mosquito
        MqttClient mClient = new MqttClient(IPAddress.Parse("127.0.0.1")); //OR use the broker hostname
        string[] mStrTopicsInfo = { "news", "complaints" };

        [HttpPost]
        [Route("createApplication")]
        public IHttpActionResult PostApplication([FromBody] Application c)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                /*string str = "INSERT INTO Prods (name, category, price) VALUES (@name, @category, @price)";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@name", c.Name);*/


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


        [HttpGet]
        [Route("sendDataToBroker")]
        public IHttpActionResult sendDataToBroker([FromBody] Application c)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                /*string str = "INSERT INTO Prods (name, category, price) VALUES (@name, @category, @price)";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.AddWithValue("@name", c.Name);*/


                int rows = command.ExecuteNonQuery();
                conn.Close();
                if (rows <= 0)
                {
                    return BadRequest("Error");
                }
                else
                {

                }

                return Ok(c);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}