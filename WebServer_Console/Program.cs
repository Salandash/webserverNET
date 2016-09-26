using System;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebServer_Framework;

namespace WebServer_Console
{

    //Clase que maneja el loop donde corre el listener
    public class cLoop
    {
        int port;
        string host;
        public cLoop(int p)
        {
            port = p;
            host = "localhost";
        }

        public void loop()
        {
            TcpListener oListener = new TcpListener(port);
            TcpClient oClient = new TcpClient();
            oListener.Start();
            Console.WriteLine("Listening on" + host + " " + port);
            
            while (true)
            {
                oClient = oListener.AcceptTcpClient();
                Stream stream = new BufferedStream(oClient.GetStream());
                StreamWriter resStream = new StreamWriter(oClient.GetStream());
                cRequest req = new cRequest(stream);


                req._printVerb();
                req._printUrl();
                req._printAuth();

                if(req.bWantFile() == true)
                {
                    Console.WriteLine("Fetching the file manager...");

                    if(req._fileManager() == "image")
                    {
                        
                    }
                    else if(req._fileManager() == "secret")
                    {
                        
                    }
                    else
                    {
                        resStream.Write("HTTP/1.0 401 Unauthorized");
                        resStream.Write(Environment.NewLine);
                        resStream.Write(Environment.NewLine);

                        resStream.Write("ERROR 401: The request requires valid user authentication.");
                    }
                }
                else
                {
                    resStream.Write("HTTP/1.0 404 Not Found");
                    resStream.Write(Environment.NewLine);
                    resStream.Write(Environment.NewLine);

                    resStream.Write("ERROR 404: The server has not found anything matching the Request-URL.");
                }

                oClient.Close();
            }

        }


    }

    //Clase servidor
    public class Server
    {
        int port;
        string path;
        public Server() { }

        static void Main(string[] args)
        {
            Server server = new Server();
            if(args.Length == 0)
            {
               server.port = 8080;
               server.path = "localhost:";
            }
            else
            {
                //Validar puerto ingresado
                if (Int32.TryParse(args[0], out server.port))
                    Console.WriteLine("Valid port");
                else
                {
                    Console.WriteLine("Invalid port. Will be using default port.");
                }

                server.path = args[1];

            }


            try
            {
                cLoop oLoop = new cLoop(server.port, server.host);
                Thread oThread = new Thread( new ThreadStart(oLoop.loop));

                oThread.Start();
                                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
 }

