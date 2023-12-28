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
                    this.lamp_photo.Image = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
