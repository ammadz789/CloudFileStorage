using System;

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
            this.madePublicTextBox = new System.Windows.Forms.TextBox();
            this.madePublicButton = new System.Windows.Forms.Button();
            this.requestPublicListButton = new System.Windows.Forms.Button();
            this.downloadTextBox = new System.Windows.Forms.TextBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.requestPersonalButton = new System.Windows.Forms.Button();
            this.downloadSelfText = new System.Windows.Forms.TextBox();
            this.downloadSelfButton = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.download_location = new System.Windows.Forms.Button();
            this.delete_text = new System.Windows.Forms.TextBox();
            this.copy_text = new System.Windows.Forms.TextBox();
            this.copy_button = new System.Windows.Forms.Button();
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
            this.button_conn.Location = new System.Drawing.Point(16, 160);
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
            this.button_disconn.Location = new System.Drawing.Point(173, 160);
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
            this.button_upload.Location = new System.Drawing.Point(204, 205);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(99, 23);
            this.button_upload.TabIndex = 9;
            this.button_upload.Text = "Select to Upload";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.button_upload_Click);
            // 
            // text_fname
            // 
            this.text_fname.Enabled = false;
            this.text_fname.Location = new System.Drawing.Point(16, 208);
            this.text_fname.Name = "text_fname";
            this.text_fname.ReadOnly = true;
            this.text_fname.Size = new System.Drawing.Size(182, 20);
            this.text_fname.TabIndex = 10;
            // 
            // madePublicTextBox
            // 
            this.madePublicTextBox.Enabled = false;
            this.madePublicTextBox.Location = new System.Drawing.Point(15, 234);
            this.madePublicTextBox.Name = "madePublicTextBox";
            this.madePublicTextBox.Size = new System.Drawing.Size(184, 20);
            this.madePublicTextBox.TabIndex = 11;
            this.madePublicTextBox.TextChanged += new System.EventHandler(this.madePublicTextBox_TextChanged);
            // 
            // madePublicButton
            // 
            this.madePublicButton.Enabled = false;
            this.madePublicButton.Location = new System.Drawing.Point(205, 232);
            this.madePublicButton.Name = "madePublicButton";
            this.madePublicButton.Size = new System.Drawing.Size(98, 23);
            this.madePublicButton.TabIndex = 12;
            this.madePublicButton.Text = "Made Public";
            this.madePublicButton.UseVisualStyleBackColor = true;
            this.madePublicButton.Click += new System.EventHandler(this.madePublicButton_Click);
            // 
            // requestPublicListButton
            // 
            this.requestPublicListButton.Enabled = false;
            this.requestPublicListButton.Location = new System.Drawing.Point(16, 431);
            this.requestPublicListButton.Name = "requestPublicListButton";
            this.requestPublicListButton.Size = new System.Drawing.Size(136, 23);
            this.requestPublicListButton.TabIndex = 13;
            this.requestPublicListButton.Text = "Request Public Files";
            this.requestPublicListButton.UseVisualStyleBackColor = true;
            this.requestPublicListButton.Click += new System.EventHandler(this.requestPublicListButton_Click);
            // 
            // downloadTextBox
            // 
            this.downloadTextBox.Enabled = false;
            this.downloadTextBox.Location = new System.Drawing.Point(16, 379);
            this.downloadTextBox.Name = "downloadTextBox";
            this.downloadTextBox.Size = new System.Drawing.Size(183, 20);
            this.downloadTextBox.TabIndex = 14;
            this.downloadTextBox.TextChanged += new System.EventHandler(this.downloadTextBox_TextChanged);
            // 
            // downloadButton
            // 
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(206, 379);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(128, 23);
            this.downloadButton.TabIndex = 15;
            this.downloadButton.Text = "Download From Public";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // requestPersonalButton
            // 
            this.requestPersonalButton.Enabled = false;
            this.requestPersonalButton.Location = new System.Drawing.Point(158, 431);
            this.requestPersonalButton.Name = "requestPersonalButton";
            this.requestPersonalButton.Size = new System.Drawing.Size(136, 23);
            this.requestPersonalButton.TabIndex = 17;
            this.requestPersonalButton.Text = "Request Personal Files";
            this.requestPersonalButton.UseVisualStyleBackColor = true;
            this.requestPersonalButton.Click += new System.EventHandler(this.requestPersonalButton_Click);
            // 
            // downloadSelfText
            // 
            this.downloadSelfText.Enabled = false;
            this.downloadSelfText.Location = new System.Drawing.Point(16, 405);
            this.downloadSelfText.Name = "downloadSelfText";
            this.downloadSelfText.Size = new System.Drawing.Size(183, 20);
            this.downloadSelfText.TabIndex = 18;
            this.downloadSelfText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // downloadSelfButton
            // 
            this.downloadSelfButton.Enabled = false;
            this.downloadSelfButton.Location = new System.Drawing.Point(206, 405);
            this.downloadSelfButton.Name = "downloadSelfButton";
            this.downloadSelfButton.Size = new System.Drawing.Size(127, 23);
            this.downloadSelfButton.TabIndex = 19;
            this.downloadSelfButton.Text = "Download Personal";
            this.downloadSelfButton.UseVisualStyleBackColor = true;
            this.downloadSelfButton.Click += new System.EventHandler(this.downloadSelfButton_Click);
            // 
            // delete_button
            // 
            this.delete_button.Enabled = false;
            this.delete_button.Location = new System.Drawing.Point(206, 261);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(97, 23);
            this.delete_button.TabIndex = 21;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // download_location
            // 
            this.download_location.Enabled = false;
            this.download_location.Location = new System.Drawing.Point(16, 350);
            this.download_location.Name = "download_location";
            this.download_location.Size = new System.Drawing.Size(317, 23);
            this.download_location.TabIndex = 22;
            this.download_location.Text = "Choose Download Location";
            this.download_location.UseVisualStyleBackColor = true;
            this.download_location.Click += new System.EventHandler(this.download_location_Click);
            // 
            // delete_text
            // 
            this.delete_text.Enabled = false;
            this.delete_text.Location = new System.Drawing.Point(15, 265);
            this.delete_text.Name = "delete_text";
            this.delete_text.Size = new System.Drawing.Size(184, 20);
            this.delete_text.TabIndex = 23;
            this.delete_text.TextChanged += new System.EventHandler(this.delete_text_TextChanged);
            // 
            // copy_text
            // 
            this.copy_text.Enabled = false;
            this.copy_text.Location = new System.Drawing.Point(16, 292);
            this.copy_text.Name = "copy_text";
            this.copy_text.Size = new System.Drawing.Size(183, 20);
            this.copy_text.TabIndex = 24;
            // 
            // copy_button
            // 
            this.copy_button.Enabled = false;
            this.copy_button.Location = new System.Drawing.Point(206, 288);
            this.copy_button.Name = "copy_button";
            this.copy_button.Size = new System.Drawing.Size(97, 23);
            this.copy_button.TabIndex = 25;
            this.copy_button.Text = "Copy";
            this.copy_button.UseVisualStyleBackColor = true;
            this.copy_button.Click += new System.EventHandler(this.copy_button_Click);
            // 
            // F
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 466);
            this.Controls.Add(this.copy_button);
            this.Controls.Add(this.copy_text);
            this.Controls.Add(this.delete_text);
            this.Controls.Add(this.download_location);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.downloadSelfButton);
            this.Controls.Add(this.downloadSelfText);
            this.Controls.Add(this.requestPersonalButton);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.downloadTextBox);
            this.Controls.Add(this.requestPublicListButton);
            this.Controls.Add(this.madePublicButton);
            this.Controls.Add(this.madePublicTextBox);
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
            this.Load += new System.EventHandler(this.F_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void madePublicTextBox_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
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
        private System.Windows.Forms.TextBox madePublicTextBox;
        private System.Windows.Forms.Button madePublicButton;
        private System.Windows.Forms.Button requestPublicListButton;
        private System.Windows.Forms.TextBox downloadTextBox;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button requestPersonalButton;
        private System.Windows.Forms.TextBox downloadSelfText;
        private System.Windows.Forms.Button downloadSelfButton;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.Button download_location;
        private System.Windows.Forms.TextBox delete_text;
        private System.Windows.Forms.TextBox copy_text;
        private System.Windows.Forms.Button copy_button;
    }
}

