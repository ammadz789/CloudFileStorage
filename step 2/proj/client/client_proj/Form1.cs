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
        string downloadDirectory;

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
                text_fname.Enabled = true;
                byte[] success = new byte[16];
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
                    Byte[] buffer = new Byte[1024];

                    //First send the information about the connection
                    string firstSend = "2" + fileName + "\n";
                    buffer = Encoding.Default.GetBytes(firstSend);
                    clientSocket.Send(buffer);
                    
                    //size info sending
                    byte[] bytes = System.IO.File.ReadAllBytes(fileName);
                    byte[] size = BitConverter.GetBytes(bytes.Length);
                    clientSocket.Send(size, size.Length, SocketFlags.None);
                    Thread.Sleep(1000);
                    
                    //continous sending
                   using (var file2 = System.IO.File.OpenRead(fileName))
                    {
                        var sendBuffer = new byte[2048];
                        var fileSize = BitConverter.GetBytes(file2.Length);
                        //clientSocket.Send(fileSize, fileSize.Length, SocketFlags.None);

                        var bytesLeftToTransmit = BitConverter.ToInt32(fileSize,0);
                        while (bytesLeftToTransmit > 0)
                        {
                            var dataToSend = file2.Read(sendBuffer, 0, sendBuffer.Length);
                            bytesLeftToTransmit -= dataToSend;

                            //loop until the socket have sent everything in the buffer.
                            var offset = 0;
                            while (dataToSend > 0)
                            {
                                var bytesSent = clientSocket.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                                dataToSend -= bytesSent;
                                offset += bytesSent;
                            }
                            //var bytesSent = clientSocket.Send(sendBuffer,sendBuffer.Length,SocketFlags.None);                                                          
                        }
                    }



                    //Console.WriteLine(fileName);
                    //clientSocket.SendFile(fileName);
                    // Send the "end" keyword to inform server that transmission is finished
                    logs.AppendText("File succesfully sent\n");

                }


            }
            // Dosya secilmez ise
            catch (System.ArgumentException)
            {
                logs.AppendText("File can not be null!!! \n");
            }
            catch (Exception Ex)
            {
                logs.AppendText("Exception Occured" + Ex.ToString() + "\n");
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

                    madePublicTextBox.Enabled = true;
                    madePublicButton.Enabled = true;
                    button_conn.Enabled = false;
                    button_upload.Enabled = true;
                    button_disconn.Enabled = true;
                    requestPublicListButton.Enabled = true;
                    downloadButton.Enabled = true;
                    downloadTextBox.Enabled = true;
                    requestPersonalButton.Enabled = true;
                    downloadSelfButton.Enabled = true;
                    downloadSelfText.Enabled = true;
                    delete_button.Enabled = true;
                    delete_text.Enabled = true;
                    download_location.Enabled = true;
                    copy_button.Enabled = true;
                    copy_text.Enabled = true;
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
                    Byte[] buffer = new Byte[4096];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    string incomingMessage2 = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    Console.WriteLine(incomingMessage);
                    if (incomingMessage[0] == '1')
                    {
                        logs.AppendText("Username exists in server. Please pick a different username \n");
                        disconnectFromServer();
                    }
                    if (incomingMessage[0] == '2')
                    {
                        if (incomingMessage[1] == '0')
                        {
                            logs.AppendText("|------------------------|\nFormat: Owner|FileName|Size|Upload Time\nCurrent Public File list is:\n" + incomingMessage.Substring(2, incomingMessage.Length - 2));
                            logs.AppendText("|------------------------|\n");
                        }
                        if (incomingMessage[1] == '1')
                        {
                            logs.AppendText("No public file is available!\n");
                        }
                    }
                    if (incomingMessage[0] == '3')
                    {
                        try
                        {
                            if (incomingMessage[1] == '0')
                            {
                                logs.AppendText("\nRequested file: " + downloadTextBox.Text + " is not in the server!\n");
                            }
                            else if (incomingMessage[1] == '1')
                            {
                                logs.AppendText("\nRequested file: " + downloadTextBox.Text + " is not public!\n");
                            }
                            else
                            {
                                byte[] bsize = new byte[512];
                                clientSocket.Receive(bsize);
                                int size = BitConverter.ToInt32(bsize, 0);
                                Thread.Sleep(1000);
                                //if (Directory.Exists("C:Temp\\Downloads\\" + text_name.Text) == false)
                                if (Directory.Exists(downloadDirectory) == false)
                                {
                                    Directory.CreateDirectory(downloadDirectory);
                                }
                                using (var filewriteStream = System.IO.File.Create(downloadDirectory + "\\" + downloadTextBox.Text))
                                {

                                    var receiveBuffer = new byte[2048];
                                    var bytesLeftToReceive = size;

                                    while (bytesLeftToReceive > 0)
                                    {
                                        //receive
                                        var bytesRead = clientSocket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
                                        if (bytesRead == 0)
                                        {
                                            logs.AppendText("Server has disconnected\n");
                                            break;
                                        }

                                        //if the socket is used for other things after the file transfer
                                        //we need to make sure that we do not copy that data
                                        //to the file
                                        int bytesToCopy = Math.Min(bytesRead, bytesLeftToReceive);

                                        // write to file
                                        filewriteStream.Write(receiveBuffer, 0, bytesToCopy);

                                        //update our tracker.
                                        bytesLeftToReceive -= bytesToCopy;
                                    }
                                }
                                //alerting the changes to db on rich text box
                                logs.AppendText("File: " + downloadTextBox.Text + " has been downloaded\n");

                            }
                        }
                        catch (Exception Ex)
                        {
                            logs.AppendText("Exception occured " + Ex + "\n");
                        }
                    }
                    if (incomingMessage[0] == '4')
                    {
                        try
                        {
                            if (incomingMessage[1] == '1')
                            {
                                logs.AppendText("File is already made public\n");
                            }
                            if (incomingMessage[1] == '0') { logs.AppendText("File not found in the Database check file name!\n"); }
                            if (incomingMessage[1] == '2') { logs.AppendText("File is successfully made public!\n"); }
                        }
                        catch (Exception Ex)
                        {
                            logs.AppendText("Exception occured " + Ex + "\n");
                        }

                    }
                    if (incomingMessage[0] == '5')
                    {
                        logs.AppendText("Format: fileName|uploadTime|File Size\n");
                        logs.AppendText(incomingMessage.Substring(1, incomingMessage.Length - 1));
                    }
                    if (incomingMessage[0] == '6')
                    {
                        if (incomingMessage[1] == '0') { logs.AppendText("cant download the file!"); }
                        else
                        {
                            Thread.Sleep(500);
                            byte[] bsize = new byte[512];
                            clientSocket.Receive(bsize);
                            int size = BitConverter.ToInt32(bsize, 0);
                            Thread.Sleep(1000);
                            if (Directory.Exists(downloadDirectory) == false)
                            {
                                Directory.CreateDirectory(downloadDirectory);
                            }
                            using (var filewriteStream = System.IO.File.Create(downloadDirectory + "\\" + downloadSelfText.Text))
                            {

                                var receiveBuffer = new byte[2048];
                                var bytesLeftToReceive = size;
                                while (bytesLeftToReceive > 0)
                                {
                                    //receive
                                    var bytesRead = clientSocket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
                                    if (bytesRead == 0)
                                    {
                                        logs.AppendText("Server has disconnected\n");
                                        break;
                                    }

                                    //if the socket is used for other things after the file transfer
                                    //we need to make sure that we do not copy that data
                                    //to the file
                                    int bytesToCopy = Math.Min(bytesRead, bytesLeftToReceive);

                                    // write to file
                                    filewriteStream.Write(receiveBuffer, 0, bytesToCopy);

                                    //update our tracker.
                                    bytesLeftToReceive -= bytesToCopy;
                                }
                            }
                            //alerting the changes to db on rich text box
                            logs.AppendText("File: " + downloadSelfText.Text + " has been downloaded\n");
                        }
                    }
                    if (incomingMessage[0] == '8')
                    {
                        if (incomingMessage[1] == '0')
                        {
                            logs.AppendText("File Copied Sucesfully\n");
                        }
                        if (incomingMessage[1] == '1')
                        {
                            logs.AppendText("File Not Copied There Is An Error\n");
                        }
                    }
                    if (incomingMessage[0] == '9')
                    {
                        if(incomingMessage[1] == '0')
                        {
                            logs.AppendText("File Deleted Sucesfully\n");
                        }
                        if(incomingMessage[1] == '1')
                        {
                            logs.AppendText("File Not Deleted There Is An Error\n");
                        }
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
            requestPublicListButton.Enabled = false;
            madePublicButton.Enabled = false;
            madePublicTextBox.Enabled = false;
            downloadButton.Enabled = false;
            downloadTextBox.Enabled = false;
            requestPersonalButton.Enabled = false;
            downloadSelfButton.Enabled = false;
            downloadSelfText.Enabled = false;
            delete_button.Enabled = false;
            delete_text.Enabled = false;
            download_location.Enabled = false;
            copy_button.Enabled = false;
            copy_text.Enabled = false;
            logs.AppendText("Disconnected from server \n");
        }

        private void text_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void F_Load(object sender, EventArgs e)
        {

        }

        private void madePublicButton_Click(object sender, EventArgs e)
        {
            try {
                string toSend = "3"+ madePublicTextBox.Text + "," + text_name.Text;
                
                
                byte[] buffer = new byte[2048];
                buffer = Encoding.Default.GetBytes(toSend);
                clientSocket.Send(buffer);

            } catch { }
        }
        private void requestPublicListButton_Click(object sender, EventArgs e)
        {
            try {
                string toSend = "4";
                byte[] buffer = new byte[64];
                buffer = Encoding.Default.GetBytes(toSend);
                clientSocket.Send(buffer);

            
            
            }catch(Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

      
        private void downloadButton_Click(object sender, EventArgs e)
        {
            try
            {
                string toSend = "5" + downloadTextBox.Text + "|" + text_name.Text;
                Byte[] buffer = new byte[256];
                buffer = Encoding.Default.GetBytes(toSend);
                clientSocket.Send(buffer);
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        private void downloadTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void requestPersonalButton_Click(object sender, EventArgs e)
        {
            try
            {
                string toSend = "6" + text_name.Text;
                Byte[] buffer = new byte[64];
                buffer = Encoding.Default.GetBytes(toSend);
                clientSocket.Send(buffer);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void downloadSelfButton_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = downloadSelfText.Text;
                string toSend = "7";
                if(fileName.IndexOf("-") == -1)
                {
                    toSend = toSend + fileName.Substring(0, fileName.Length - 4) +"-"+ text_name.Text + ".txt";
                    Console.WriteLine(toSend);
                }
                else
                {
                    toSend = toSend + fileName.Substring(0, fileName.IndexOf("-")+1) + text_name.Text + fileName.Substring(fileName.IndexOf("-"), 3)+ ".txt";
                    Console.WriteLine(toSend);
                }
                Byte[] buffer = new byte[64];
                buffer = Encoding.Default.GetBytes(toSend);
                clientSocket.Send(buffer);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            string fileName = delete_text.Text;
            if (fileName.Contains('-'))
            {
                string fileNumber = fileName.Substring(fileName.Length -2, 2);
                string finalFileName = '9' + fileName.Substring(0, fileName.Length - 2) + username + '-' + fileNumber + ".txt";
                //Console.WriteLine(finalFileName);


                if (connected)
                {

                    Byte[] buffer = new Byte[64];
                    buffer = Encoding.Default.GetBytes(finalFileName);

                    clientSocket.Send(buffer);
                }

            }
            else
            {
                string finalFileName = '9' + fileName + '-' + username + ".txt";
                //Console.WriteLine(finalFileName);


                if (connected)
                {

                    Byte[] buffer = new Byte[64];
                    buffer = Encoding.Default.GetBytes(finalFileName);

                    clientSocket.Send(buffer);
                }

            }

        }

        private void delete_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void download_location_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                downloadDirectory = folderBrowserDialog1.SelectedPath;
            }
        }

        private void copy_button_Click(object sender, EventArgs e)
        {
            string filename = copy_text.Text;
            string message = '8' + filename + '-' + username;

            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);


        } 
    }
    public class Message
    {
        public String type { get; set; }
        public String content { get; set; }

    }
}
