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
using SOMIOD.Helpers;



namespace SOMIOD.Controller
{
    [RoutePrefix("api/somiod")]
    public class Somiod : ApiController
    {

        //Cria o BROKEN mosquito
        MqttClient mClient = new MqttClient(IPAddress.Parse("127.0.0.1")); //OR use the broker hostname

        [HttpPost]
        [Route("")] //Create Application
        public IHttpActionResult PostApplication([FromBody] Application a)
        {
            
            try
            {
                dbHelper.createApp(a.Name);

                return Ok(a);//enviar por XML

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //DELETE APPLICATION [Route("{application}")]
        //DELETE CONTAINER [Route("{application}/{container}")]
        //GET CONTAINERS  [Route("{application}/containers")]
        //POST SUBSCRIPTION [Route("{application}/{container}/subscription")]
        //DELETE SUBSCRIPTION [Route("{application}/{container}/subscription")]
        //GET CONTAINERS [Route("{application}/containers")]
        //GET DATA [Route("{application}/{container}/data")]
        //DELETE DATA [Route("{application}/{container}/data")]

        [HttpPost]
        [Route("{application}")] //Create Container
        public IHttpActionResult PostContainers([FromBody] Container c)
        {

            try
            {
               dbHelper.createContainer(c.Name, c.Parent); //enviar o application

                return Ok(c);//enviar por XML

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("{application}/{container}/data")] //Send Data to Broker
        public IHttpActionResult sendDataToBroker(string application, string container, [FromBody] Data d)
        {
            try
            {
                dbHelper.sendData(d.Content, application, container);

                if(mClient.IsConnected)
                {
                    //string selectedTopic = comboBox1.SelectedItem.ToString();
                    //string textFromTextBox = textBox1.Text;
                    //envia a mensagem para o broker com o topico que foi selecionado e a mensagem que foi escrita
                    mClient.Publish(application, d.Content);
                }

                return Ok(d); //enviar por XML

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("{application}/{container}")] //Get Container
        public Container getContainer( string container)
        {
            try
            {
                Container cont = dbHelper.getContainer(container); //enviar o application
                return cont; //enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        [HttpGet]
        [Route("{application}")] //Get Application
        public Application getApplication(string application)
        {
            try
            {
                Application app = dbHelper.getApplication(application);
                return app; //enviar por XML

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("")] //Get Applications
        public IEnumerable<Application> GetApplications()
        {
            try
            {
                List<Application> applications = dbHelper.GetApplications();
                return applications; //enviar por XML
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}