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
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Amazon.Auth.AccessControlPolicy;
using static System.Net.WebRequestMethods;

namespace SOMIOD.Controller
{
    [RoutePrefix("api/somiod")]
    public class SomiodController : ApiController
    {

        //Cria o BROKEN mosquito
        MqttClient mClient = new MqttClient(IPAddress.Parse("127.0.0.1")); //OR use the broker hostname
        
        #region SOMIOD DISCOVER

        [HttpGet]
        [Route("")] //Get all - DONE
        public IHttpActionResult GetAll()
        {
            try
            {
                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var dataType))
                {
                    switch (dataType.FirstOrDefault().ToString())
                    {
                        case "application":
                            List<Application> applications = dbHelper.GetAllApplications();
                            return Ok(applications); //enviar por XML
                        case "container":
                            List<Container> containers = dbHelper.GetAllContainers();
                            return Ok(containers); //enviar por XML
                        case "data":
                            List<Data> datas = dbHelper.GetAllDatas();
                            return Ok(datas); //enviar por XML
                        //case subscription:
                        default:
                            return BadRequest("Wrong Header");
                    }
                }
                else
                {
                    return BadRequest("Header not found");
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("{application}")] //Get from Application
        public IHttpActionResult GetFromApplication(string application)
        {
            try
            {
                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var contentType))
                {
                    switch (contentType.FirstOrDefault().ToString())
                    {
                        case "application":
                            Application app = dbHelper.getApplication(application); //PAIR PROGRAMMING
                            return Ok(app); //enviar por XML
                        case "container":
                            List<Container> containers = dbHelper.GetContainers(application); //PAIR PROGRAMMING
                            return Ok(containers); //enviar por XML
                        case "data":
                            List<Data> data = dbHelper.GetDatas(application, null); //PAIR PROGRAMMING
                            return Ok(data); //enviar por XML
                        //case subscription:
                        default:
                            return BadRequest("Wrong Header");
                    }
                }
                else
                {
                    return BadRequest("Header not found");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("{application}/{container}")] //Get from Application and Container
        public IHttpActionResult GetFromApplication(string application, string container)
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
                            Container cont = dbHelper.getContainer(application, container);
                            return Ok(cont); //enviar por XML
                        case "data":
                            List<Data> data = dbHelper.GetDatas(application, container);
                            return Ok(data); //enviar por XML
                        //case subscription:
                        default:
                            return BadRequest("Wrong Header");
                    }
                }
                else
                {
                    return BadRequest("Header not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion
        

        #region Application

        [HttpPost]
        [Route("")] //Create Application - DONE
        public IHttpActionResult PostApplication([FromBody] Application a)
        {
            try
            {
                if (a == null)
                {
                    return BadRequest("Invalid application");
                }

                dbHelper.createApplication(a.Name);
                return Ok("Application created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpDelete]
        [Route("{application}")] //Delete Application - DONE
        public IHttpActionResult DeleteApplication(string application)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                dbHelper.deleteApplication(application);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpPatch]
        [Route("{application}")] //Update Application ---- PAIR PROGRAMMING
        public IHttpActionResult UpdateApplication(string application, string name) //name in query string on the URL
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Application not found");
                }

                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Invalid Name");
                }

                Application app = dbHelper.updateApplication(name, application);
                return Ok(app);//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion


        #region Containers

        [HttpPost]
        [Route("{application}")] //Create Container - DONE
        public IHttpActionResult PostContainers(string application, [FromBody] Container c)
        {

            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (c == null)
                {
                   return BadRequest("Invalid container");
                }

                dbHelper.createContainer(c.Name, application);
                return Ok(c);//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{application}/{container}")] //Delete Container - DONE
        public IHttpActionResult DeleteContainer(string application, string container)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid Container");
                }

                dbHelper.deleteContainer(application, container);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch]
        [Route("{application}/{container}")] //Update Container ---- PAIR PROGRAMMING
        public IHttpActionResult UpdateContainer(string application, string container, string name) //name in query string on the URL
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid container");
                }

                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Invalid Name");
                }

                Container c = dbHelper.updateContainer(name, application, container);
                return Ok(c);//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion


        #region Data

        [HttpPost]
        [Route("{application}/{container}/data")] //Send Data to Broker
        public IHttpActionResult SendDataToBroker(string application, string container, [FromBody] Data data)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid container");
                }

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }

                dbHelper.sendData(data.Content, application, container);

                if(mClient.IsConnected)
                {
                    
                    //envia a mensagem para o broker com o topico que foi selecionado e a mensagem que foi escrita
                    mClient.Publish(application, Encoding.UTF8.GetBytes(data.Content)); //o que é o canal???
                }

                return Ok(data); //enviar por XML

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{application}/{container}/data/{dataId}")] //Delete Data
        public IHttpActionResult DeleteData(string application, string container, int dataId)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid container");
                }

                if (dataId == null)
                {
                    return BadRequest("Invalid dataId");
                }

                dbHelper.deleteData(application, container, dataId);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("{application}/{container}/data/{dataId}")] //Get Data
        public IHttpActionResult GetData(string application, string container, int dataId)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid container");
                }

                if (dataId == null)
                {
                    return BadRequest("Invalid dataId");
                }

                Data data = dbHelper.getData(application, container, dataId);
                return Ok(data); //enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion


        #region Subscription

        [HttpPost]
        [Route("{application}/{container}/subscription")] //Create Subscription
        public IHttpActionResult PostSubscription(string application, string container, [FromBody] Subscription subs)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid container");
                }

                if (subs == null)
                {
                    return BadRequest("Invalid subscription");
                }

                dbHelper.createSubscription(subs.Name, subs.EventType, subs.Endpoint, application, container);
                return Ok(subs);//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{application}/{container}/subscription/{subscriptionId}")] //Delete Subscription
        public IHttpActionResult DeleteSubscription(string application, string container, int subscriptionId)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    return BadRequest("Invalid application");
                }

                if (string.IsNullOrEmpty(container))
                {
                    return BadRequest("Invalid container");
                }

                if (subscriptionId == null)
                {
                    return BadRequest("Invalid subscriptionId");
                }

                dbHelper.deleteSubscription(application, container, subscriptionId);
                return Ok();//enviar por XML
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion

    }
}