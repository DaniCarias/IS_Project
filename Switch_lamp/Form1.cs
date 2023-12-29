using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Switch_lamp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_on_Click(object sender, EventArgs e)
        {
            //Request POST to create a data to turn on the lamp on - publish in SOMIOD controller
            //[Route("{application}/{container}/data")] - Send Data to Broker


        }

        private void btn_off_Click(object sender, EventArgs e)
        {
            //Request POST to create a data to turn on the lamp off - publish in SOMIOD controller
            //[Route("{application}/{container}/data")] - Send Data to Broker


            //Publish a msg to turn on the lamp off


        }

        private void btn_create_app_Click(object sender, EventArgs e)
        {
            //Verify if already exists (if yes disable the button, if no enable the button)


            //Request POST to create an application
            //[Route("")] - Create Application


        }
    }
}
