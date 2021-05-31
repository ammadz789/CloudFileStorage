using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{

  
    public partial class Form1 : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        DB database = new DB();
        int bufferLength = 67108864;
        string directory;

        bool terminating = false;
        bool listening = false;
        

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;

            if(Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(5);

                listening = true;
                button_listen.Enabled = false;
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }

        private void Accept()
        {
            while(listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();

                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }

        private void Receive(Socket thisClient) // updated
        {
            bool connected = true;
            string usernameGeneral = "";
            if (!System.IO.File.Exists(@"C:\proj_DB.txt")) { using (System.IO.File.Create(@"C:\proj_DB.txt")); }

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[bufferLength];
                    thisClient.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    
                    // To check if the type of the comming message is an username
                    if(incomingMessage[0] == '0')
                    {
                        string username = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        if (database.addNewSocketWithUsername(username, thisClient))
                        {
                            
                            logs.AppendText("A client with the username: " + username + " is connected to the server \n");
                            usernameGeneral = username;
                        }
                        else
                        {
                            logs.AppendText("Username: " + username + " already exists in server \n");
                            string message = "001";
                            Byte[] tempBuffer2 = new Byte[64];
                            tempBuffer2 = Encoding.Default.GetBytes(message);
                            thisClient.Send(tempBuffer2);
                        }
                    }
                    if (incomingMessage[0] == '1')
                    {
                        string username = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        if (database.removeSocketWithUsername(username))
                        {
                            logs.AppendText("Username: " + username + " disconnected from the server \n");
                        }
                        else
                        {
                            Console.WriteLine("There is a problem. Username: " + username + " not found in the server \n");
                        }
                        
                    }
                    if(incomingMessage[0] == '2')
                    {

                        string fileText = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        String fileName = fileText.Substring(0, fileText.IndexOf("\n"));
                        String fileContent = fileText.Substring(fileText.IndexOf("\n"), fileText.Length - fileName.Length);


                        int index = fileName.LastIndexOf("\\");
                        fileName = fileName.Substring(index + 1, fileName.Length - index - 1);
                        Console.WriteLine(fileName);
                        fileName = fileName.Substring(0, fileName.IndexOf('.'));
                        fileName = fileName + "-" + usernameGeneral + ".txt";
                        
                        // Checking DB file for uniqueness
                        try
                        {
                            int number = 0;
                            string line;
                            int counter = 0;
                            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\proj_DB.txt");

                            while ((line = file.ReadLine()) != null)
                            {
                                if (line == fileName)
                                {
                                    string uniqueName = fileName.Substring(0, fileName.IndexOf('.'));
                                    string increment = fileName.Substring(fileName.Length - 6);
                                    increment = increment.Substring(1,1);
                                    bool isParsable = Int32.TryParse(increment, out number);
                                    
                                    if (isParsable)
                                    {
                                        number++;
                                        fileName = uniqueName.Substring(0,uniqueName.Length-2) + "0" + number.ToString() + ".txt";
                                        
                                       
                                    }
                                    else
                                    {
                                        fileName = uniqueName + "-01.txt";
                                       
                                    }
                                }
                                counter++;
                            }
                            file.Close();                                                    
                            using (System.IO.StreamWriter w = System.IO.File.AppendText(@"C:\proj_DB.txt")){ w.WriteLine(fileName);}
                            System.IO.File.WriteAllText(directory + "\\" + fileName, fileContent);
                            logs.AppendText("A client uploaded a file: " + fileName + "\n");
                            
                        }
                        catch (Exception Ex)
                        {
                            logs.AppendText("Exception Occured" + Ex.ToString() + "\n");
                            
                        }
                        
                    }

                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("A client has disconnected\n");
                    }
                    thisClient.Close();

                    database.removeBySocket(thisClient);
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }



        private void logs_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            logs.AppendText(database.printAllUsers());
        }

        private void directoryChoose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                directory = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class DBNode
    {

        public DBNode(string usernameInput, Socket socketInput)
        {
            this.socket = socketInput;
            this.username = usernameInput;
        }
        public string username { get; set; }
        public Socket socket { get; set; }

        public List<File> fileList = new List<File>();
       


    }
    public class File
    {
        public File(string fileName, string fileContent)
        {
            this.fileName = fileName;
            this.fileContent = fileContent;
        }
        public string fileName { get; set; }
        public string fileContent { get; set; }
    }
    public class DB
    {
        public List<DBNode> dbList = new List<DBNode>();

        private bool dbContains(string tempString) {
            foreach(DBNode node in dbList)
            {
                if(tempString == node.username)
                {
                    return true;
                }
            }
            return false;
        }

        public bool addNewSocketWithUsername(string username, Socket socket)
        {


            //If a username already exists in our server
            if (this.dbContains(username))
            {
                return false;
            }

            else
            {
                DBNode tempNode = new DBNode(username, socket);
                this.dbList.Add(tempNode);
                Console.WriteLine("New node added to the db");
                return true;
            }
        }

        public bool removeSocketWithUsername(string username)
        {
            foreach (DBNode node in dbList)
            {
                if (username == node.username)
                {
                    this.dbList.Remove(node);
                    Console.WriteLine(username + " -- Entry removed from db");
                    return true;
                }
            }
            return false;
        }

        public bool removeBySocket(Socket socket)
        {
            foreach (DBNode node in dbList)
            {
                if (socket == node.socket)
                {
                    this.dbList.Remove(node);
                    return true;
                }
            }
            return false;
        }

        public string printAllUsers()
        {
            string logString = "";

            if (!dbList.Any())
            {
                logString = "No user exists in the server \n";
                return logString;
            }

            foreach (DBNode node in dbList)
            {
                logString += node.username + "\n";
            }
            return logString;
        }

        public void addFileToUser(string username,string fileName, string fileContent)
        {
            foreach (DBNode node in dbList)
            {
                if (username == node.username)
                {
                    File tempFile = new File(fileName, fileContent);
                    node.fileList.Add(tempFile);
                }
            }
        }
    }

     
 
}
