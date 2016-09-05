using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace WebServer_Console
{
    public class cLoop
    {
        public void loop()
        {
            while (true)
            {
                Console.WriteLine("Thread Corriendo");
            }
        }
    }
    class Server
    {
        private const int port = 8080;
        private const string host = "localhost:";


        static void Main(string[] args)
        {
            try
            {
                cLoop oLoop = new cLoop();
                TcpClient oClient = new TcpClient();
                TcpListener oListener = new TcpListener(port);
                Thread oThread = new Thread( new ThreadStart(oLoop.loop));

                oClient = oListener.AcceptTcpClient();
                oListener.Start();

                if (oClient.GetStream().CanRead)
                {
                    NetworkStream ns = oClient.GetStream();
                    byte[] bytes = new byte[1024];
                    int bytesRead = ns.Read(bytes, 0, bytes.Length);
                    Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRead));

                    oThread.Start();

                }
                else
                {
                    Console.WriteLine("No se puede leer la data");
                }


                oThread.Abort();
                oListener.Stop();
                oClient.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
 }

