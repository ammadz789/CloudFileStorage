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
using System.IO;

namespace server
{

  
    public partial class Form1 : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        DB database = new DB();
        int bufferLength = 1024;
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
            byte[] success = new byte[16];

            if (!System.IO.File.Exists(@"C:\Temp\proj_DB.txt")) { using (System.IO.File.Create(@"C:\Temp\proj_DB.txt")) ; }
            if (!System.IO.File.Exists(@"C:\Temp\public_DB.txt")) { using (System.IO.File.Create(@"C:\Temp\public_DB.txt")) ; }
            if (!System.IO.File.Exists(@"C:\Temp\private_DB.txt")) { using (System.IO.File.Create(@"C:\Temp\private_DB.txt")) ; }
            if (!System.IO.File.Exists(@"C:\Temp\uploadTime_DB.txt")) { using (System.IO.File.Create(@"C:\Temp\uploadTime_DB.txt")) ; }

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
                            string message = "1";
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
                        // taking the incoming message
                        string fileText = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        String fileName = fileText.Substring(0, fileText.IndexOf("\n"));
                        //String fileContent = fileText.Substring(fileText.IndexOf("\n"), fileText.Length - fileName.Length);
                        

                        int index = fileName.LastIndexOf("\\");
                        fileName = fileName.Substring(index + 1, fileName.Length - index - 1);
                        Console.WriteLine(fileName);
                        fileName = fileName.Substring(0, fileName.IndexOf('.'));
                        fileName = fileName + "-" + usernameGeneral + ".txt";
                        
                        // Checking DB file for uniqueness adding 0 or 1 to end of the filenames to make them public/private
                        try
                        {
                            int privacyCheck = 0; // 0 means private 1 means public
                            int number = 0;                           
                            
                            string[] dbLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                            string[] publicLines = System.IO.File.ReadAllLines(@"C:\Temp\public_DB.txt");
                            foreach (string line in dbLines)
                            {
                                if (line == fileName)// If file is in the database
                                {                                   
                                    
                                    foreach(string line2 in publicLines)
                                    {
                                        if(line2 == fileName)
                                        {
                                            privacyCheck = 1;                                            
                                        }
                                    }

                                    string uniqueName = fileName.Substring(0, fileName.IndexOf('.'));
                                    string increment = fileName.Substring(fileName.Length - 6);
                                    increment = increment.Substring(1, 1);
                                    bool isParsable = Int32.TryParse(increment, out number);

                                    if (isParsable)
                                    {
                                        number++;
                                        fileName = uniqueName.Substring(0, uniqueName.Length - 2) + "0" + number.ToString() + ".txt";
                                    }
                                    else
                                    {
                                        fileName = uniqueName + "-01.txt";
                                    }                            
                                }
                               
                            }                          
                                                                             
                            //Writing to database file and first file is uploaded as private by default
                            using (System.IO.StreamWriter w = System.IO.File.AppendText(@"C:\Temp\proj_DB.txt")){ w.WriteLine(fileName);}
                            if (privacyCheck == 0) { using (System.IO.StreamWriter w = System.IO.File.AppendText(@"C:\Temp\private_DB.txt")) { w.WriteLine(fileName); } }
                            else { using (System.IO.StreamWriter w = System.IO.File.AppendText(@"C:\Temp\public_DB.txt")) { w.WriteLine(fileName); } }

                            
                            byte[] bsize = new byte[2048];
                            thisClient.Receive(bsize);
                            int size = BitConverter.ToInt32(bsize, 0);
                            
                            //store upload time for the file
                            string localDate = DateTime.Now.ToString("h:mm:ss tt");
                            using (System.IO.StreamWriter w = System.IO.File.AppendText(@"C:\Temp\uploadTime_DB.txt")) { w.WriteLine(fileName+"|"+localDate+ "|"+size); }

                            if (privacyCheck == 1)
                            {
                                fileName = "Public Files\\" + fileName;
                            }
                            
                            using (var filewriteStream = System.IO.File.Create(directory + "\\" + fileName))
                            {
                                
                                var receiveBuffer = new byte[2048];
                                var bytesLeftToReceive = size;

                                while (bytesLeftToReceive > 0)
                                {
                                    //receive
                                    var bytesRead = thisClient.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
                                    if (bytesRead == 0)
                                    {
                                        logs.AppendText("A client has disconnected\n");
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
                            logs.AppendText("A client uploaded a file: " + fileName + "\n");
                            
                        }

                        catch (Exception Ex)
                        {
                            logs.AppendText("Exception Occured" + Ex.ToString() + "\n");
                            
                        }                        
                    }
                    if(incomingMessage[0] == '3') //request for file to made public
                    {
                        int exists2 = 1; // 1 = file exists 0 = not exists 
                        try
                        {
                            int exists = 0;
                            //receive the fileName to made public
                            string targetPath = directory + "\\Public Files";
                            string sourcePath = directory;

                            string[] privateLines = System.IO.File.ReadAllLines(@"C:\Temp\private_DB.txt");
                            Console.WriteLine(incomingMessage);
                            string fileName = incomingMessage;//.Substring(1, incomingMessage.IndexOf('|')-2);
                            string owner = incomingMessage;//.Substring(incomingMessage.IndexOf('|'), incomingMessage.Length-2);
                            fileName = fileName.Substring(1, fileName.IndexOf(',') - 1);
                            Console.WriteLine(fileName);
                            
                            //storing ownerless fileName for listing purposes
                            string orgFileName = fileName; 

                            //owner = owner.Substring(owner.IndexOf(','), owner.Length - 2);
                            int indexVal = owner.IndexOf(',')+1;
                            int indexLen = owner.Length;
                            int sizeval = indexLen - indexVal;
                            owner = owner.Substring(indexVal,sizeval);
                            
                            if(fileName.IndexOf('-') == -1)
                            {
                                fileName = fileName.Substring(0,fileName.Length -4) + "-" + owner + ".txt";
                            }
                            else
                            {
                                string tmp = fileName.Substring(0, fileName.IndexOf('-')+1) + owner + fileName.Substring(fileName.IndexOf('-'), 7);
                                fileName = tmp;
                            }
                            Console.WriteLine(fileName);

                            //checking if file exists in the database
                            string[] DBLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                            foreach (string DBline in DBLines)
                            {
                                if (DBline == fileName)
                                {
                                    exists = 1;
                                    break;
                                }
                            }
                                                                                    
                            // if file is not found send error code to the client 
                            if(exists == 0)
                            {
                                exists2 = 0;
                                string errorSend = "40";
                                Byte[] errBuffer = new byte[16];
                                errBuffer = Encoding.Default.GetBytes(errorSend);
                                thisClient.Send(errBuffer);
                            
                                throw new ArgumentException("-");
                            }

                            // check if the given file name is already public 
                            exists = 2; // 2 --> file exists and already been made public
                            foreach (string prLine in privateLines)
                            {
                                if (prLine == fileName)
                                {
                                    exists = 1;
                                }
                            }
                            if(exists == 2)
                            {
                                exists2 = 2;
                                string errorSend = "41";
                                Byte[] errBuffer = new byte[16];
                                errBuffer = Encoding.Default.GetBytes(errorSend);
                                thisClient.Send(errBuffer);
                                throw new ArgumentException("-");
                            }

                            //if public file list doesnot exists than create
                            if (!System.IO.File.Exists(@"C:\Temp\publicFileList.txt")) { using (System.IO.File.Create(@"C:\Temp\publicFileList.txt")) ; }

                            using (System.IO.StreamWriter w = new System.IO.StreamWriter(@"C:\Temp\private_DB.txt"))
                            {
                                foreach (string line in privateLines)
                                {
                                    if (line == fileName)
                                    {
                                        using (System.IO.StreamWriter w2 = System.IO.File.AppendText(@"C:\Temp\public_DB.txt")) { w2.WriteLine(fileName); }
                                    }
                                    else
                                    {
                                        w.WriteLine(line);
                                    }
                                }
                            }
                            logs.AppendText("A client made public file: " + fileName + "\n");
                            
                            //get file size
                            System.IO.FileInfo info = new System.IO.FileInfo(directory+"\\"+fileName);
                            long fileSize = info.Length;

                            //get upload time
                            string uploadTime = DateTime.Now.ToString("h:mm:ss tt"); ;
                            string[] uploadLines = System.IO.File.ReadAllLines(@"C:\Temp\uploadTime_DB.txt");
                            foreach(string timeLine in uploadLines)
                            {
                                if(timeLine.IndexOf(fileName) != -1)
                                {
                                    uploadTime = timeLine.Substring(timeLine.IndexOf("|")+1, timeLine.Length-1 - timeLine.IndexOf("|"));
                                }
                            }
                            Console.WriteLine(uploadTime);
                            
                            
                            //write the owner/time/filename/size public list folder if not exists
                           
                            using (System.IO.StreamWriter w2 = System.IO.File.AppendText(@"C:\Temp\publicFileList.txt")) { w2.WriteLine(owner+"|"+ orgFileName + "|" + fileSize + " Bytes|" + uploadTime); }
                            
                            //send success message
                            string errorSend2 = "42";
                            Byte[] errBuffer2 = new byte[16];
                            errBuffer2 = Encoding.Default.GetBytes(errorSend2);
                            thisClient.Send(errBuffer2);

                            //Path for the files
                            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);

                            //create directory
                            System.IO.Directory.CreateDirectory(targetPath);
                            
                            System.IO.File.Copy(sourceFile, destFile, true);                           
                            
                            System.IO.File.Delete(sourceFile);

                        }
                        catch(Exception Ex)
                        {
                            if (!terminating)
                            {
                                if (exists2 == 0)
                                {
                                    logs.AppendText("File not found in the DB\n");
                                }
                                if (exists2 == 2)
                                {
                                    logs.AppendText("File already is public\n");
                                }                               
                            }

                        }
                    }
                    if(incomingMessage[0] == '4') //request for public file list
                    {
                        if (System.IO.File.Exists(@"C:\Temp\publicFileList.txt") == false) { logs.AppendText("No public files available!\n");
                            string toSend = "21";
                            Byte[] tempBuffer = new Byte[64];
                            tempBuffer = Encoding.Default.GetBytes(toSend);
                            thisClient.Send(tempBuffer);
                        }
                        else
                        {
                            string[] publicLines = System.IO.File.ReadAllLines(@"C:\Temp\publicFileList.txt");
                            string toSend = "20";
                            foreach (string line in publicLines)
                            {
                                toSend = toSend + line + "\n";
                            }
                            Console.WriteLine(toSend);
                            Byte[] tempBuffer = new Byte[4096];
                            tempBuffer = Encoding.Default.GetBytes(toSend);
                            thisClient.Send(tempBuffer);
                        }
                    }

                    if(incomingMessage[0] == '5') //request for download a file
                    {
                        logs.AppendText("client requested download\n");
                        string fileName = incomingMessage.Substring(1, incomingMessage.IndexOf('|') - 1);
                        string[] publicLines = System.IO.File.ReadAllLines(@"C:\Temp\public_DB.txt");
                        string[] DBLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                        string toSend = "30";
                        foreach(string line in DBLines)
                        {
                            if(line == fileName) // if requested file is in DB
                            {
                                toSend = "31";
                                foreach(string line2 in publicLines) 
                                {
                                    if(line2 == fileName) // if requested file is public
                                    {
                                        toSend = "32";
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        // 30--> file not found in server, 31--> file is not public, 32--> file can be downloaded public!
                        // send file download info to client
                        Byte[] downbuffer = new byte[64];
                        downbuffer = Encoding.Default.GetBytes(toSend);
                        thisClient.Send(downbuffer);


                        if (toSend == "32") // requested file is public and can be downloaded
                        {
                            // read directly from the file to reduce memory usage.
                            var fileName2 = directory + "\\Public Files\\" + fileName;
                            using (var file = System.IO.File.OpenRead(fileName2))
                            {
                                var sendBuffer = new byte[2048];
                                var fileSize = BitConverter.GetBytes(file.Length);
                                thisClient.Send(fileSize, fileSize.Length, SocketFlags.None);
                                Thread.Sleep(1000);

                                var bytesLeftToTransmit = BitConverter.ToInt32(fileSize,0);
                                while (bytesLeftToTransmit > 0)
                                {
                                    var dataToSend = file.Read(sendBuffer, 0, sendBuffer.Length);
                                    bytesLeftToTransmit -= dataToSend;

                                    //loop until the socket have sent everything in the buffer.
                                    var offset = 0;
                                    while (dataToSend > 0)
                                    {
                                        var bytesSent = thisClient.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                                        dataToSend -= bytesSent;
                                        offset += bytesSent;
                                    }
                                }
                            }
                        }                     
                    }
                    if(incomingMessage[0] == '6') //Client requesting for his/her own files
                    {
                        string owner = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        //string[] DBLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                        string[] sizeTimeLines = System.IO.File.ReadAllLines(@"C:\Temp\uploadTime_DB.txt");
                        
                        string toSend = "5";
                        foreach(string line in sizeTimeLines)
                        {
                            if(line.IndexOf(owner) != -1)
                            {
                                if (line.IndexOf("-",line.IndexOf("-")+1) != -1)
                                {
                                    toSend = toSend + line.Substring(0, line.IndexOf(owner) - 1) + line.Substring(line.IndexOf("0")-1,3) + ".txt" + line.Substring(line.IndexOf("|"), line.Length - line.IndexOf("|")) + " Bytes\n";
                                    Console.WriteLine(toSend);
                                }
                                else
                                {
                                    toSend = toSend + line.Substring(0, line.IndexOf(owner) - 1) + ".txt" + line.Substring(line.IndexOf("|"), line.Length - line.IndexOf("|")) + " Bytes\n";
                                    Console.WriteLine(toSend);
                                    Console.WriteLine(line);
                                }
                            }
                        }
                        Byte[] tempBuffer = new Byte[2048];
                        tempBuffer = Encoding.Default.GetBytes(toSend);
                        thisClient.Send(tempBuffer);
                    }
                    if (incomingMessage[0] == '7') //Client requesting for downlaad his/her own files
                    {
                        int privacyCheck = 2; // 0 = private 1 = public 2 = file not found
                        string fileName = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        Console.WriteLine(fileName);
                        string[] DBLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                        string[] publicLines = System.IO.File.ReadAllLines(@"C:\Temp\public_DB.txt");
                        foreach (string line in DBLines)
                        {
                            if (line == fileName)
                            {
                                privacyCheck = 0;
                                foreach(string pLine in publicLines)
                                {
                                    if(line == fileName)
                                    {
                                        privacyCheck = 1;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        if (privacyCheck == 2)
                        {
                            Byte[] tempBuffer = new Byte[64];
                            tempBuffer = Encoding.Default.GetBytes("60");
                            thisClient.Send(tempBuffer);
                        }
                        else
                        {
                            Byte[] tempBuffer = new Byte[64];
                            tempBuffer = Encoding.Default.GetBytes("61");
                            thisClient.Send(tempBuffer);
                            if (privacyCheck == 1)//file is in the public folder
                            {
                                var fileName2 = directory + "\\Public Files\\" + fileName;
                                using (var file = System.IO.File.OpenRead(fileName2))
                                {
                                    var sendBuffer = new byte[2048];
                                    var fileSize = BitConverter.GetBytes(file.Length);
                                    thisClient.Send(fileSize, fileSize.Length, SocketFlags.None);
                                    Thread.Sleep(1000);

                                    var bytesLeftToTransmit = BitConverter.ToInt32(fileSize, 0);
                                    while (bytesLeftToTransmit > 0)
                                    {
                                        var dataToSend = file.Read(sendBuffer, 0, sendBuffer.Length);
                                        bytesLeftToTransmit -= dataToSend;

                                        //loop until the socket have sent everything in the buffer.
                                        var offset = 0;
                                        while (dataToSend > 0)
                                        {
                                            var bytesSent = thisClient.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                                            dataToSend -= bytesSent;
                                            offset += bytesSent;
                                        }
                                    }
                                }
                            }
                            else // file is in private folder
                            {
                                var fileName2 = directory + "\\" + fileName;
                                using (var file = System.IO.File.OpenRead(fileName2))
                                {
                                    var sendBuffer = new byte[2048];
                                    var fileSize = BitConverter.GetBytes(file.Length);
                                    thisClient.Send(fileSize, fileSize.Length, SocketFlags.None);
                                    Thread.Sleep(1000);

                                    var bytesLeftToTransmit = BitConverter.ToInt32(fileSize, 0);
                                    while (bytesLeftToTransmit > 0)
                                    {
                                        var dataToSend = file.Read(sendBuffer, 0, sendBuffer.Length);
                                        bytesLeftToTransmit -= dataToSend;

                                        //loop until the socket have sent everything in the buffer.
                                        var offset = 0;
                                        while (dataToSend > 0)
                                        {
                                            var bytesSent = thisClient.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                                            dataToSend -= bytesSent;
                                            offset += bytesSent;
                                        }
                                    }
                                }
                            }
                        }
                    }
                   
                    //Copy Code
                    if(incomingMessage[0] == '8')
                    {
                        string fileName = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        string[] DBLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                        string[] PublicDBLines = System.IO.File.ReadAllLines(@"C:\Temp\private_DB.txt");

                        int counter = 0;
                        foreach (string line in DBLines)
                        {
                            if (line.Contains(fileName))
                            {
                                counter++;
                            }
                        }
                        if(counter == 0)
                        {
                            //File not found
                            //Send negative message to server
                            logs.AppendText("Requsted file could not be copied\n");


                            string message = "81";
                            Byte[] tempBuffer2 = new Byte[64];
                            tempBuffer2 = Encoding.Default.GetBytes(message);
                            thisClient.Send(tempBuffer2);
                        }
                        else
                        {
                            string realFileName = fileName + ".txt";
                            string copyFileName = fileName + "-0" + counter.ToString() + ".txt";
                            string[] lines;
                            //File Copy part
                            if (File.Exists(Path.Combine(directory, realFileName)))
                            {
                                lines = System.IO.File.ReadAllLines(Path.Combine(directory,realFileName));

                            }
                            else
                            {
                                lines = System.IO.File.ReadAllLines(Path.Combine(Path.Combine(directory, "Public Files"), realFileName));
                            }

                            System.IO.File.WriteAllLines(Path.Combine(directory, copyFileName), lines);






                            //Send Positive message to client

                            logs.AppendText("File Copied\n");


                            string message = "80";
                            Byte[] tempBuffer2 = new Byte[64];
                            tempBuffer2 = Encoding.Default.GetBytes(message);
                            thisClient.Send(tempBuffer2);

                            //Add To DB Part
                            using (StreamWriter dbWriter = File.AppendText(@"C:\Temp\proj_DB.txt"))
                            {
                                dbWriter.WriteLine(copyFileName);
                            }
                            using (StreamWriter privatedbWriter = File.AppendText(@"C:\Temp\private_DB.txt"))
                            {
                                privatedbWriter.WriteLine(copyFileName);
                            }




                        }
                    }

                    //Delete Code
                    if (incomingMessage[0] == '9')
                    {
                        // Files to be deleted    
                        string fileName = incomingMessage.Substring(1, incomingMessage.Length - 1);
                        string[] DBLines = System.IO.File.ReadAllLines(@"C:\Temp\proj_DB.txt");
                        bool fileExists = DBLines.Where(val => val == fileName).Any();
                        if (fileExists)
                        {

                            try
                            {

                                if (File.Exists(Path.Combine(directory, fileName)))
                                {
                                    File.Delete(Path.Combine(directory, fileName));

                                }       
                                else
                                {
                                    File.Delete(Path.Combine(Path.Combine(directory, "Public Files"), fileName));
                                }
                           


                                logs.AppendText(fileName + " Deleted\n");

                                string message = "90";
                                Byte[] tempBuffer2 = new Byte[64];
                                tempBuffer2 = Encoding.Default.GetBytes(message);
                                thisClient.Send(tempBuffer2);




                            }
                            catch (IOException ioExp)
                            {
                                Console.WriteLine(ioExp.Message);
                            }



                            //Deleting the file from the database
                            string[] PublicDBLines = System.IO.File.ReadAllLines(@"C:\Temp\private_DB.txt");
                            string[] PrivateDBLines = System.IO.File.ReadAllLines(@"C:\Temp\public_DB.txt");
                            DBLines = DBLines.Where(val => val != fileName).ToArray();
                            PublicDBLines = PublicDBLines.Where(val => val != fileName).ToArray();
                            PrivateDBLines = PrivateDBLines.Where(val => val != fileName).ToArray();


                            using (System.IO.StreamWriter w = new System.IO.StreamWriter(@"C:\Temp\proj_DB.txt"))
                            {
                                foreach (string line in DBLines)
                                {
                                        w.WriteLine(line);
                                }
                            }    using (System.IO.StreamWriter w = new System.IO.StreamWriter(@"C:\Temp\public_DB.txt"))
                            {
                                foreach (string line in PublicDBLines)
                                {
                                        w.WriteLine(line);
                                }
                            }    using (System.IO.StreamWriter w = new System.IO.StreamWriter(@"C:\Temp\private_DB.txt"))
                            {
                                foreach (string line in PrivateDBLines)
                                {
                                        w.WriteLine(line);
                                }
                            }


                        }
                        else
                        {
                            logs.AppendText("File Not Found \n");
                            string message = "91";
                            Byte[] tempBuffer2 = new Byte[64];
                            tempBuffer2 = Encoding.Default.GetBytes(message);
                            thisClient.Send(tempBuffer2);

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

        public List<FileClass> fileList = new List<FileClass>();
       


    }
    public class FileClass
    {
        public FileClass(string fileName, string fileContent)
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
                    FileClass tempFile = new FileClass(fileName, fileContent);
                    node.fileList.Add(tempFile);
                }
            }
        }
    }

     
 
}
