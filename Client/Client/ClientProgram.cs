using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Client
{
    public class ClientProgram
    {
        public static void lineBreak()
        {
            Console.WriteLine("");
        }

        public static void Main(string[] args)
        {
            string menuNum = "";

            //START WHILE LOOP
            while (menuNum != "2")
            {
                Console.WriteLine("1 - Recieve Message");
                Console.WriteLine("2 - Send Message");
                Console.WriteLine("3 - Exit");
                menuNum = Console.ReadLine();
                lineBreak();

                //CONNECT TO SERVER
                if (menuNum == "1")
                {
                    using (NamedPipeClientStream clientPipe = new NamedPipeClientStream(".", "serverName", PipeDirection.InOut))
                    {
                        Console.WriteLine("Connecting to pipe...");
                        clientPipe.Connect();
                        Console.WriteLine("Connected to {0} pipe server(s)!", clientPipe.NumberOfServerInstances);
                        Console.WriteLine("Waiting for message...");
                        lineBreak();

                        using (StreamReader sr = new StreamReader(clientPipe))
                        {
                            string txt;
                            while ((txt = sr.ReadLine()) != null)
                            {
                                Console.WriteLine("Message recieved: {0}", txt);
                            }
                        }
                    }
                    lineBreak();
                    menuNum = "0";
                }

                //Send Message
                else if (menuNum == "2")
                {
                    Console.WriteLine("Starting server...");

                    using (NamedPipeServerStream serverPipe = new NamedPipeServerStream("serverName", PipeDirection.InOut))
                    {

                        Console.WriteLine("Pipe server object created!");
                        Console.WriteLine("Waiting for connection...");
                        serverPipe.WaitForConnection(); //wait for client to connect
                        Console.WriteLine("Client connected to server!");
                        Console.Write("Enter message: ");

                        try
                        {
                            using (StreamWriter sw = new StreamWriter(serverPipe)) //Create new streamwriter object
                            {
                                sw.AutoFlush = true;
                                sw.Write(Console.ReadLine()); //Assign user text to streamwriter object
                                lineBreak();
                                Console.WriteLine("Message Sent!");
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("ERROR: {0}", e.Message); //Catch error with server
                        }
                    }
                    lineBreak();
                    menuNum = "0";
                }

                //EXIT
                else if (menuNum == "3")
                {
                    System.Environment.Exit(1);
                }

                //CATCH INCORRECT INPUT
                else
                {
                    Console.WriteLine("INCORRECT INPUT!");
                }
            }
            //END WHILE LOOP

            System.Environment.Exit(1);
        }
    }
}

