namespace client_proj
{
    partial class F
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
            this.label_IP = new System.Windows.Forms.Label();
            this.label_Port = new System.Windows.Forms.Label();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.label_name = new System.Windows.Forms.Label();
            this.button_conn = new System.Windows.Forms.Button();
            this.button_disconn = new System.Windows.Forms.Button();
            this.text_ıp = new System.Windows.Forms.TextBox();
            this.text_port = new System.Windows.Forms.TextBox();
            this.text_name = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_upload = new System.Windows.Forms.Button();
            this.text_fname = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(13, 37);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(20, 13);
            this.label_IP.TabIndex = 0;
            this.label_IP.Text = "IP:";
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(12, 70);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(40, 13);
            this.label_Port.TabIndex = 1;
            this.label_Port.Text = "PORT:";
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(382, 12);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(253, 261);
            this.logs.TabIndex = 2;
            this.logs.Text = "";
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(12, 105);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(63, 13);
            this.label_name.TabIndex = 3;
            this.label_name.Text = "User Name:";
            // 
            // button_conn
            // 
            this.button_conn.Location = new System.Drawing.Point(22, 209);
            this.button_conn.Name = "button_conn";
            this.button_conn.Size = new System.Drawing.Size(75, 23);
            this.button_conn.TabIndex = 4;
            this.button_conn.Text = "Connect";
            this.button_conn.UseVisualStyleBackColor = true;
            this.button_conn.Click += new System.EventHandler(this.button_conn_Click);
            // 
            // button_disconn
            // 
            this.button_disconn.Enabled = false;
            this.button_disconn.Location = new System.Drawing.Point(173, 209);
            this.button_disconn.Name = "button_disconn";
            this.button_disconn.Size = new System.Drawing.Size(75, 23);
            this.button_disconn.TabIndex = 5;
            this.button_disconn.Text = "Disconnect";
            this.button_disconn.UseVisualStyleBackColor = true;
            this.button_disconn.Click += new System.EventHandler(this.button_disconn_Click);
            // 
            // text_ıp
            // 
            this.text_ıp.Location = new System.Drawing.Point(123, 37);
            this.text_ıp.Name = "text_ıp";
            this.text_ıp.Size = new System.Drawing.Size(125, 20);
            this.text_ıp.TabIndex = 6;
            this.text_ıp.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // text_port
            // 
            this.text_port.Location = new System.Drawing.Point(123, 70);
            this.text_port.Name = "text_port";
            this.text_port.Size = new System.Drawing.Size(125, 20);
            this.text_port.TabIndex = 7;
            // 
            // text_name
            // 
            this.text_name.Location = new System.Drawing.Point(123, 105);
            this.text_name.Name = "text_name";
            this.text_name.Size = new System.Drawing.Size(125, 20);
            this.text_name.TabIndex = 8;
            this.text_name.TextChanged += new System.EventHandler(this.text_name_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // button_upload
            // 
            this.button_upload.Enabled = false;
            this.button_upload.Location = new System.Drawing.Point(204, 287);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(99, 23);
            this.button_upload.TabIndex = 9;
            this.button_upload.Text = "Select to Upload";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.button_upload_Click);
            // 
            // text_fname
            // 
            this.text_fname.Location = new System.Drawing.Point(16, 287);
            this.text_fname.Name = "text_fname";
            this.text_fname.ReadOnly = true;
            this.text_fname.Size = new System.Drawing.Size(182, 20);
            this.text_fname.TabIndex = 10;
            // 
            // F
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 389);
            this.Controls.Add(this.text_fname);
            this.Controls.Add(this.button_upload);
            this.Controls.Add(this.text_name);
            this.Controls.Add(this.text_port);
            this.Controls.Add(this.text_ıp);
            this.Controls.Add(this.button_disconn);
            this.Controls.Add(this.button_conn);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.label_IP);
            this.Name = "F";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Button button_conn;
        private System.Windows.Forms.Button button_disconn;
        private System.Windows.Forms.TextBox text_ıp;
        private System.Windows.Forms.TextBox text_port;
        private System.Windows.Forms.TextBox text_name;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_upload;
        private System.Windows.Forms.TextBox text_fname;
    }
}

