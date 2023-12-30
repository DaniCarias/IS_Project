namespace Switch_lamp
{
    partial class SwitchForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_on = new System.Windows.Forms.Button();
            this.btn_off = new System.Windows.Forms.Button();
            this.btn_create_app = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Switch Lamp";
            // 
            // btn_on
            // 
            this.btn_on.Location = new System.Drawing.Point(109, 116);
            this.btn_on.Name = "btn_on";
            this.btn_on.Size = new System.Drawing.Size(75, 23);
            this.btn_on.TabIndex = 1;
            this.btn_on.Text = "On";
            this.btn_on.UseVisualStyleBackColor = true;
            this.btn_on.Click += new System.EventHandler(this.btn_on_Click);
            // 
            // btn_off
            // 
            this.btn_off.Location = new System.Drawing.Point(228, 116);
            this.btn_off.Name = "btn_off";
            this.btn_off.Size = new System.Drawing.Size(75, 23);
            this.btn_off.TabIndex = 2;
            this.btn_off.Text = "Off";
            this.btn_off.UseVisualStyleBackColor = true;
            this.btn_off.Click += new System.EventHandler(this.btn_off_Click);
            // 
            // btn_create_app
            // 
            this.btn_create_app.Location = new System.Drawing.Point(125, 162);
            this.btn_create_app.Name = "btn_create_app";
            this.btn_create_app.Size = new System.Drawing.Size(147, 23);
            this.btn_create_app.TabIndex = 3;
            this.btn_create_app.Text = "Create Application";
            this.btn_create_app.UseVisualStyleBackColor = true;
            this.btn_create_app.Click += new System.EventHandler(this.btn_create_app_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 197);
            this.Controls.Add(this.btn_create_app);
            this.Controls.Add(this.btn_off);
            this.Controls.Add(this.btn_on);
            this.Controls.Add(this.label1);
            this.Name = "SwitchForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_on;
        private System.Windows.Forms.Button btn_off;
        private System.Windows.Forms.Button btn_create_app;
    }
}

