﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Lamp.Models;
using Lamp.Properties;
using RestSharp;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Application = Lamp.Models.Application;
using Container = Lamp.Models.Container;

namespace Lamp
{
    public partial class Form1 : Form
    {
        private static readonly string brokerIp = Settings.Default.BrokerIp;
        private static readonly string[] topic = { Settings.Default.Topic };
        private static readonly string apiUrl = Settings.Default.ApiUrl;
        private static readonly string appName = Settings.Default.AppName;
        private static readonly string containerName = Settings.Default.ContainerName;
        private static readonly string subscriptionName = Settings.Default.SubscriptionName;
        private static readonly string eventType = Settings.Default.EventType;
        private static readonly string containerEventType = "CREATE";
        private static readonly string endpoint = Settings.Default.Endpoint;

        private MqttClient mClient;
        private readonly RestClient restClient = new RestClient(apiUrl);
        private bool isOn = false;
        private bool apiConnected = true;


        public Form1()
        {
            
            InitializeComponent();
            this.Shown += Form1_Shown;
            ChangeLampImage(isOn);
            
            //this.FormClosing += Form1_FormClosing;
        }

        private void ChangeLampImage(bool status)
        {
            Bitmap img;
            if (status)
            {
                img = Resources.lightBulbOn;
            }
            else img = Resources.lightBulbOff;

            try
            {
                if (img != null)
                {
                    if (this.lamp_photo.InvokeRequired)
                    {
                        this.lamp_photo.Invoke(new MethodInvoker(delegate { this.lamp_photo.Image = img; }));
                    }
                    else { 

                        this.lamp_photo.Image = img;
                        this.lamp_photo.SizeMode = PictureBoxSizeMode.Zoom;
                        this.lamp_photo.Size = new Size(400, 400);
                        this.lamp_photo.Refresh();
                        this.lamp_photo.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        
        private void Form1_Shown(object sender, EventArgs e)
        {
            ConnectToBroker();
            bool itSubbed = SubscribeToTopics();
            CreateApplication(appName);
            CreateContainer(containerName, appName);
            if (itSubbed)
            {
                CreateSubscription(subscriptionName, containerName, appName, eventType, endpoint);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mClient.IsConnected)
            {
                mClient.Unsubscribe(topic);
                mClient.Disconnect();
            }
        }


        private void ConnectToBroker()
        {
            mClient = new MqttClient(brokerIp);
            try
            {
                mClient.Connect(Guid.NewGuid().ToString());

                if (!mClient.IsConnected)
                {
                    MessageBox.Show("Broker couldnt be reached", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"Error connecting to the broker: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        
        private bool SubscribeToTopics()
        {
            try
            {
                mClient.MqttMsgPublishReceived += clientPublishReceived;
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
                mClient.Subscribe(topic, qosLevels);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error subscribing to the topic: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void clientPublishReceived(object sender, MqttMsgPublishEventArgs args)
        {

            string message = Encoding.UTF8.GetString(args.Message);
            if(message == null)
                return; 
            XmlReader reader = XmlReader.Create(new StringReader(message));
            reader.ReadToFollowing("Content");
            string content = reader.ReadElementContentAsString();
            if (content == null)
                return;
       
                switch (content.ToUpper())
                {
                    case "ON":
                        isOn = true;
                        break;
                    case "OFF":
                        isOn = false;
                        break;
                }
                ChangeLampImage(isOn);
            

            
            
        }



        private void CreateApplication(string applicationName)
        {
            var app = new Application(applicationName);
            
            var request = new RestRequest(apiUrl + $"/api/somiod/", Method.Post);
            request.AddObject(app);
        
            var response = restClient.Execute<RestRequest>(request);
            if (CheckEntityExists(response))
                return;

            if (response.StatusCode == 0 && apiConnected) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                apiConnected = false;
                return;
            }else apiConnected = true;

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private void CreateContainer(string containerName, string applicationName)
        {
            var container = new Container(containerName, applicationName);

            RestRequest request = new RestRequest(apiUrl + $"/api/somiod/{applicationName}", Method.Post);
            //request.AddHeader("somiod-discover", "container");
            request.AddObject(container);
            
            var response = restClient.Execute<RestRequest>(request);

            if (CheckEntityExists(response))
                return;

            if (response.StatusCode == 0 && apiConnected) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                apiConnected = false;
                return;
            }else apiConnected = true;

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating the container", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private bool CheckEntityExists(RestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
                return true;

            return false;
        }
        
        private void CreateSubscription(string subscriptionName, string containerName, string applicationName, string eventType, string endpoint)
        {
            var sub = new Subscription(subscriptionName, containerName, eventType, endpoint);

            RestRequest request = new RestRequest(apiUrl + $"/api/somiod/{applicationName}/{containerName}/subscription", Method.Post);
            request.AddObject(sub);

            var response = restClient.Execute<RestRequest>(request);

            if (CheckEntityExists(response))
                return;

            if (response.StatusCode == 0 && apiConnected) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                apiConnected = false;
                return;
            }else apiConnected = true;

        }

    }

}
