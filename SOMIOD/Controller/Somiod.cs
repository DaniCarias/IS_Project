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


        #region SOMIOD DISCOVER

        [HttpGet]
        [Route("")] //Get all
        public IEnumerable GetAll()
        {
            try
            {
                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var contentType))
                {
                    switch (contentType.ToString())
                    {
                        case "application":
                            List<Application> applications = dbHelper.GetAllApplications();
                            return applications; //enviar por XML
                        case "container":
                            List<Container> containers = dbHelper.GetAllContainers();
                            return containers; //enviar por XML
                        case "data":
                            List<Data> datas = dbHelper.GetAllDatas();
                            return datas; //enviar por XML
                        default:
                            return null;
                    }
                }
                else
                {
                    throw new Exception("Header not found");
                }
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("{application}")] //Get from Application
        public IEnumerable GetFromApplication(string application)
        {
            try
            {
                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var contentType))
                {
                    switch (contentType.ToString())
                    {
                        case "application":
                            GetApplication(application); //app info
                            break;
                        case "container":
                            List<Container> containers = dbHelper.GetContainers(application);
                            return containers; //enviar por XML
                        case "data":
                            List<Data> data = dbHelper.GetDatas(application, null);
                            return data; //enviar por XML
                        default:
                            return null;
                    }
                }
                else
                {
                    GetApplication(application); //app info
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("{application}/{container}")] //Get from Application and Container
        public IEnumerable GetFromApplication(string application, string container)
        {
            try
            {
                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var contentType))
                {
                    switch (contentType.ToString())
                    {
                        case "application":
                            //NAO SEI
                            return null;
                        case "container":
                            GetContainer(container);
                            break;
                        case "data":
                            List<Data> data = dbHelper.GetDatas(application, container);
                            return data; //enviar por XML
                        default:
                            return null;
                    }
                }
                else
                {
                    GetContainer(container); //container info
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region Application

        [HttpPost]
        [Route("")] //Create Application
        public IHttpActionResult PostApplication([FromBody] Application a)
        {
            try
            {
                dbHelper.createApplication(a.Name);
                return Ok(a);//enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("{application}")] //Delete Application
        public IHttpActionResult DeleteApplication(string app)
        {
            try
            {
                dbHelper.deleteApplication(app);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("{application}")] //Get Application info
        public Application GetApplication(string application)
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

        #endregion

        
        #region Containers

        [HttpPost]
        [Route("{application}")] //Create Container
        public IHttpActionResult PostContainers(string application, [FromBody] Container c)
        {

            try
            {
                dbHelper.createContainer(c.Name, application);
                return Ok(c);//enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("{application}/{container}")] //Delete Container
        public IHttpActionResult DeleteContainer(string application, string container)
        {
            try
            {
                dbHelper.deleteContainer(application, container);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("{application}/{container}")] //Get Container
        public Container GetContainer(string container)
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
        [Route("{application}/containers")] //Get all Containers of an Application
        public IEnumerable<Container> GetContainers(string application)
        {
            try
            {
                List<Container> containers = dbHelper.GetContainers(application);
                return containers; //enviar por XML
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


        #region Data

        [HttpPost]
        [Route("{application}/{container}/data")] //Send Data to Broker
        public IHttpActionResult SendDataToBroker(string application, string container, [FromBody] Data d)
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

        [HttpDelete]
        [Route("{application}/{container}/data/{dataId}")] //Delete Data
        public IHttpActionResult DeleteData(string application, string container, int dataId)
        {
            try
            {
                dbHelper.deleteData(application, container, dataId);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("{application}/{container}/data/{dataId}")] //Get Data
        public Data GetData(string application, string container, int dataId)
        {
            try
            {
                Data data = dbHelper.getData(application, container, dataId);
                return data; //enviar por XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        //POST SUBSCRIPTION [Route("{application}/{container}/subscription")]
        //DELETE SUBSCRIPTION [Route("{application}/{container}/subscription")]

    }
}