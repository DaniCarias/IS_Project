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
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
using System.Runtime.InteropServices.ComTypes;

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
                            return Ok(applications); 
                        case "container":
                            List<Container> containers = dbHelper.GetAllContainers();
                            return Ok(containers); 
                        case "data":
                            List<Data> datas = dbHelper.GetAllDatas();
                            return Ok(datas); 
                        case "subscription":
                            List<Subscription> subscriptions = dbHelper.GetAllSubscriptions();
                            return Ok(subscriptions);
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
        [Route("{application}")] //Get from Application - DONE
        public IHttpActionResult GetFromApplication(string application)
        {
            try
            {
                if (dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var contentType))
                {
                    switch (contentType.FirstOrDefault().ToString())
                    {
                        case "application":
                            Application app = dbHelper.GetApplication(application);
                            return Ok(app);
                        case "container":
                            List<Container> containers = dbHelper.GetContainers(application);
                            return Ok(containers);
                        case "data":
                            List<Data> data = dbHelper.GetDatas(application, null);
                            return Ok(data);
                        case "subscription":
                            List<Subscription> subscriptions = dbHelper.GetSubscriptions(application);
                            return Ok(subscriptions);
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
        [Route("{application}/{container}")] //Get from Application and Container - DONE
        public IHttpActionResult GetFromApplication(string application, string container)
        {
            try
            {
                if (dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                var headers = Request.Headers;

                if (headers.TryGetValues("somiod-discover", out var contentType))
                {
                    switch (contentType.FirstOrDefault().ToString())
                    {
                        case "application":
                            return Ok("Header application not Suported");
                        case "container":
                            Container cont = dbHelper.GetContainer(container);
                            return Ok(cont);
                        case "data":
                            List<Data> data = dbHelper.GetDatas(application, container);
                            return Ok(data);
                        case "subscription":
                            List<Subscription> subscriptions = dbHelper.GetSubscriptions(application, container);
                            return Ok(subscriptions);
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
        public IHttpActionResult PostApplication([FromBody] Application app)
        {
            try
            {
                if (string.IsNullOrEmpty(app.Name))
                    return BadRequest("Invalid name");

                if (dbHelper.IsApplicationExists(app.Name))
                    return Conflict();

                Boolean res = dbHelper.CreateApplication(app.Name);

                if (res)
                    return Ok("Application created successfully");
                else
                    return BadRequest("Application not created");

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
                    return BadRequest("Invalid name");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");
               
                Boolean res = dbHelper.DeleteApplication(application);

                if (res)
                    return Ok("Application deleted successfully");
                else
                    return BadRequest("Application not deleted");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpPatch]
        [Route("{application}")] //Update Application - DONE
        public IHttpActionResult UpdateApplication(string application, string name) //name in query string on the URL
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Application not found");

                if (string.IsNullOrEmpty(name))
                    return BadRequest("Invalid Name");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                Boolean res = dbHelper.UpdateApplication(name, application);

                if (res)
                    return Ok("Application updated");
                else
                    return BadRequest("Application not updated");
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
        public IHttpActionResult PostContainers(string application, [FromBody] Container container)
        {

            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container.Name))
                return BadRequest("Invalid container");

                if(!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (dbHelper.IsContainerExists(container.Name))
                    return Conflict();

                Boolean res = dbHelper.CreateContainer(container.Name, application);

                if (res)
                    return Ok("Container created successfully");
                else
                    return BadRequest("Container not created");

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
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid Container");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return NotFound();

                if (!dbHelper.VerifyParent("Container", container, application))
                    return BadRequest("Container does not belong to this Application");

                Boolean res = dbHelper.DeleteContainer(application, container);

                if (res)
                    return Ok("Container deleted successfully");
                else
                    return BadRequest("Container not deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch]
        [Route("{application}/{container}")] //Update Container - DONE
        public IHttpActionResult UpdateContainer(string application, string container, string name) //name in query string on the URL
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (string.IsNullOrEmpty(name))
                    return BadRequest("Invalid Name");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                if (!dbHelper.VerifyParent("Container", container, application))
                    return BadRequest("Container does not belong to this Application");

                Boolean res = dbHelper.UpdateContainer(name, application, container);

                if (res)
                    return Ok("Container updated");
                else
                    return BadRequest("Container not updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion


        #region Data

        [HttpPost]
        [Route("{application}/{container}/data")] //Send Data to Broker - DONE
        public IHttpActionResult SendDataToBroker(string application, string container, [FromBody] Data data)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (string.IsNullOrEmpty(data.Content))
                    return BadRequest("Invalid data");

                if(!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                if (!dbHelper.VerifyParent("Data", container, application))
                    return BadRequest("Data does not belong to this Container");

                Boolean res = dbHelper.CreateData(data.Content, application, container);

                if (!res)
                    return BadRequest("Data not created");

                if(mClient.IsConnected)
                {
                    //envia a mensagem para o broker com o topico que foi selecionado e a mensagem que foi escrita
                    mClient.Publish(application, Encoding.UTF8.GetBytes(data.Content));
                }

                return Ok("Data created successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{application}/{container}/data/{dataId}")] //Delete Data - DONE
        public IHttpActionResult DeleteData(string application, string container, int dataId)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                if (!dbHelper.IsDataExists(dataId))
                    return BadRequest("Data does not exists");

                if (!dbHelper.VerifyParent("Data", dataId.ToString(), container))
                    return BadRequest("Data does not belong to this Container");

                Boolean res = dbHelper.DeleteData(application, container, dataId);

                if(res)
                    return Ok("Data deleted successfully");
                else
                    return BadRequest("Data not deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch]
        [Route("{application}/{container}/data/{dataId}")] //Update Data content - DONE
        public IHttpActionResult UpdateData(string application, string container, int dataId, string content) //content in query string on the URL
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (string.IsNullOrEmpty(content))
                    return BadRequest("Invalid content");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                if (!dbHelper.IsDataExists(dataId))
                    return BadRequest("Data does not exists");

                if (!dbHelper.VerifyParent("Data", dataId.ToString(), container))
                    return BadRequest("Data does not belong to this Container");

                Boolean res = dbHelper.UpdateData(application, container, dataId, content);

                if (res)
                    return Ok("Data updated");
                else
                    return BadRequest("Data not updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion


        #region Subscription

        public class SubscriptionData
        {
            public string Name { get; set; }
            public string Endpoint { get; set; }
        }

        [HttpPost]
        [Route("{application}/{container}/subscription")] //Create Subscription - DONE
        public IHttpActionResult PostSubscription(string application, string container, [FromBody] SubscriptionData data)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (string.IsNullOrEmpty(data.Name))
                    return BadRequest("Invalid subscription name");

                if (string.IsNullOrEmpty(data.Endpoint))
                    return BadRequest("Invalid subscription endpoint");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                Boolean res = dbHelper.CreateSubscription(data.Name, data.Endpoint, application, container);

                if (res)
                    return Ok("Subscription created successfully");
                else
                    return BadRequest("Subscription not created");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{application}/{container}/subscription/{subscriptionName}")] //Delete Subscription- DONE
        public IHttpActionResult DeleteSubscription(string application, string container, string subscriptionName)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (string.IsNullOrEmpty(subscriptionName))
                    return BadRequest("Invalid subscription name");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                if (!dbHelper.IsSubscriptionExists(subscriptionName))
                    return BadRequest("Subscription does not exists");

                if (!dbHelper.VerifyParent("Subscription", subscriptionName, container))
                    return BadRequest("Data does not belong to this Container");

                Boolean res = dbHelper.DeleteSubscription(application, container, subscriptionName);

                if (res)
                    return Ok("Subscription deleted successfully");
                else
                    return BadRequest("Subscription not deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        [Route("{application}/{container}/subscription/{subscriptionName}")] //Update Subscription - DONE
        public IHttpActionResult UpdateSubscription(string application, string container, string subscriptionName, [FromBody] SubscriptionData data)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                    return BadRequest("Invalid application");

                if (string.IsNullOrEmpty(container))
                    return BadRequest("Invalid container");

                if (string.IsNullOrEmpty(subscriptionName))
                    return BadRequest("Invalid subscription name");

                if (!dbHelper.IsApplicationExists(application))
                    return BadRequest("Application does not exists");

                if (!dbHelper.IsContainerExists(container))
                    return BadRequest("Container does not exists");

                if (!dbHelper.IsSubscriptionExists(subscriptionName))
                    return BadRequest("Subscription does not exists");

                if (string.IsNullOrEmpty(data.Name))
                    data.Name = null;

                if (string.IsNullOrEmpty(data.Endpoint))
                    data.Endpoint = null;

                if (!dbHelper.VerifyParent("Subscription", subscriptionName, container))
                    return BadRequest("Data does not belong to this Container");

                Boolean res = dbHelper.UpdateSubscription(application, container, subscriptionName, data.Name, data.Endpoint);

                if (res)
                    return Ok("Subscription updated");
                else
                    return BadRequest("Subscription not updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion

    }
}