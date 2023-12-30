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
using System.Xml.Serialization;
using RestSharp;
using Switch_lamp.Models;
using Switch_lamp.Properties;
using Application = Switch_lamp.Models.Application;

namespace Switch_lamp
{
    public partial class SwitchForm : Form
    {
        private static readonly string apiUrl = Settings.Default.ApiUrl;
        private static readonly string appName = Settings.Default.AppName;
        private static readonly string containerName = Settings.Default.ContainerName;
        
        private readonly RestClient restClient = new RestClient(apiUrl);
        public SwitchForm()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            CreateApplication(appName);
        }

        private void btn_on_Click(object sender, EventArgs e)
        {
            PostData("ON");
        }

        private void btn_off_Click(object sender, EventArgs e)
        {
           PostData("OFF");
        }

        private void btn_create_app_Click(object sender, EventArgs e)
        {
            //Verify if already exists (if yes disable the button, if no enable the button)


            //Request POST to create an application
            //[Route("")] - Create Application
            
        }
        
        private void CreateApplication(string applicationName)
        {
            var app = new Application(applicationName);
            
            var request = new RestRequest(apiUrl + $"/api/somiod/", Method.Post);
            request.AddObject(app);
        
            var response = restClient.Execute<RestRequest>(request);
            if (CheckEntityExists(response))
                return;

            if (response.StatusCode == 0 ){
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private bool CheckEntityExists(RestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
                return true;

            return false;
        }

        private void PostData(string content)
        {
            RestRequest request = new RestRequest(apiUrl + $"/api/somiod/{appName}/{containerName}/data", Method.Post);
            request.RequestFormat = DataFormat.Xml;
            request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            request.AddXmlBody(data);
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(Data));
            serializer.Serialize(stringwriter, new Data(content, containerName));
            request.AddParameter("application/xml", stringwriter.ToString() , ParameterType.RequestBody);

            var response = restClient.Execute<RestRequest>(request);
            if (CheckEntityExists(response))
                return;

            if (response.StatusCode == 0 ){
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while posting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
