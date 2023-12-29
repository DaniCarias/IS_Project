using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lamp.Properties;

namespace Lamp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.ChangeLampImage(false);
            InitializeComponent();
        }
        
        private void ChangeLampImage(bool status)
        {
            Image img;
            if (status)
            {
                img = Resources.lightBulbOn;
            }
            else img = Resources.lightBulbOff;

            try
            {
                if (img != null)
                {
                    this.lamp_photo = Image.FromFile(img);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_create_app_cont_Click(object sender, EventArgs e)
        {
            //Verify if already exists (if yes disable the button, if no enable the button)


            //Request POST to create app


            //Request POST to create container


        }

        private void btn_subsc_Click(object sender, EventArgs e)
        {
            //Request POST to create subscribe


            //Subscrive to broker in MQTT


            //Listening if there is a message in MQTT chanel - here?????
        }
    }
}
