using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace client_proj
{
    public partial class F : Form
    {

        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        string username;

        public F()
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(F_FormClosing);
            InitializeComponent();
        }

        private void F_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
            {
                string message_dc = (text_name.Text + " has been disconnected! \n");
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(message_dc);
                clientSocket.Send(buffer);
            }
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Title = "Select a txt File";
                file.InitialDirectory = @"c:\";
                file.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                file.FilterIndex = 2;
                file.RestoreDirectory = true;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    text_fname.Text = file.SafeFileName;
                    string fileName = file.FileName;

                    string[] fileLines = File.ReadAllLines(fileName);

                    string finalFileString = fileName + "\n";
                    foreach (string s in fileLines)
                    {
                        finalFileString += s + "\n";
                    }

                    finalFileString = "2" + finalFileString;
                    Console.WriteLine(finalFileString);

                    Byte[] buffer = new Byte[1024];
                    buffer = Encoding.Default.GetBytes(finalFileString);
                    clientSocket.Send(buffer);
                    logs.AppendText("File succesfully sent\n");

                }


            }
            // Dosya secilmez ise
            catch (System.ArgumentException)
            {
                logs.AppendText("File can not be null!!! \n");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button_conn_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = text_ıp.Text;

            int portNum;
            if (Int32.TryParse(text_port.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);


                    button_conn.Enabled = false;
                    button_upload.Enabled = true;
                    button_disconn.Enabled = true;
                    connected = true;

                    //Gets the username from the designated input
                    string message = text_name.Text;
                    username = message;
                 
                    //Checks if the username is valid
                    if (message != "" && message.Length <= 64)
                    {
                        message = "0" + message;
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                    }

                    logs.AppendText("Connected to the server!\n");

                    Thread receiveThread = new Thread(Receive);
                    receiveThread.Start();

                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }

        }



        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    Console.WriteLine(incomingMessage);
                    if(incomingMessage == "001")
                    {
                        logs.AppendText("Username exists in server. Please pick a different username \n");
                        disconnectFromServer();
                    }
                  
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        button_conn.Enabled = true;
                        button_upload.Enabled = false;
                        disconnectFromServer();
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }

        private void button_disconn_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                string message_dc = ("1" + username);
                Console.WriteLine(message_dc);

                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(message_dc);
                clientSocket.Send(buffer);

                disconnectFromServer();
                button_upload.Enabled = false;
            }
         

        }

        private void disconnectFromServer()
        {
            connected = false;
            terminating = true;
            button_conn.Enabled = true;
            button_disconn.Enabled = false;
            logs.AppendText("Disconnected from server \n");
        }

        private void text_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class Message
    {
        public String type { get; set; }
        public String content { get; set; }

    }
}
