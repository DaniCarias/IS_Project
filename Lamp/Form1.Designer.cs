namespace Lamp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lamp_photo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_create_app_cont = new System.Windows.Forms.Button();
            this.btn_subsc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lamp_photo)).BeginInit();
            this.SuspendLayout();
            // 
            // lamp_photo
            // 
            this.lamp_photo.Image = ((System.Drawing.Image)(resources.GetObject("lamp_photo.Image")));
            this.lamp_photo.Location = new System.Drawing.Point(107, 57);
            this.lamp_photo.Name = "lamp_photo";
            this.lamp_photo.Size = new System.Drawing.Size(343, 332);
            this.lamp_photo.TabIndex = 0;
            this.lamp_photo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(261, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lamp";
            // 
            // btn_create_app_cont
            // 
            this.btn_create_app_cont.Location = new System.Drawing.Point(152, 439);
            this.btn_create_app_cont.Name = "btn_create_app_cont";
            this.btn_create_app_cont.Size = new System.Drawing.Size(249, 23);
            this.btn_create_app_cont.TabIndex = 2;
            this.btn_create_app_cont.Text = "Create Application and Container";
            this.btn_create_app_cont.UseVisualStyleBackColor = true;
            this.btn_create_app_cont.Click += new System.EventHandler(this.btn_create_app_cont_Click);
            // 
            // btn_subsc
            // 
            this.btn_subsc.Location = new System.Drawing.Point(230, 477);
            this.btn_subsc.Name = "btn_subsc";
            this.btn_subsc.Size = new System.Drawing.Size(91, 23);
            this.btn_subsc.TabIndex = 3;
            this.btn_subsc.Text = "Subscrive";
            this.btn_subsc.UseVisualStyleBackColor = true;
            this.btn_subsc.Click += new System.EventHandler(this.btn_subsc_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 512);
            this.Controls.Add(this.btn_subsc);
            this.Controls.Add(this.btn_create_app_cont);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lamp_photo);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.lamp_photo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox lamp_photo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_create_app_cont;
        private System.Windows.Forms.Button btn_subsc;
    }
}

