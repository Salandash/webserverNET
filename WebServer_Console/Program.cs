using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Web;


namespace WebServer_Console
{

    //Clase que maneja el loop donde corre el listener
    public class cLoop
    {
        int port = 8080;
        string host = "localhost:";

        public string GetVerb(Stream request)
        {
            bool b = true;
            int aux;
            string rstring= "";

            while(b)
            {
                aux = request.ReadByte();

                if (aux == '\n')
                {
                    break;
                }
                if (aux == ' ')
                {
                    break;
                }
                
                rstring += Convert.ToChar(aux);
            }

            return rstring;
        }

        public void GetAuth(Stream request)
        {
            HttpContext context = HttpContext.Current;

            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string usepass = authHeader.Substring("Basic ".Length).Trim();

                int seperatorIndex = usepass.IndexOf(':');

                string user = usepass.Substring(0, seperatorIndex);
                string pass = usepass.Substring(seperatorIndex + 1);

                Console.WriteLine("Username = " + user);
                Console.WriteLine("Password = " + pass);
            }
            else
            {
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }
   


        }

        public void loop()
        {
            TcpListener oListener = new TcpListener(port);
            TcpClient oClient = new TcpClient();
            oListener.Start();

            while (true)
            {
                oClient = oListener.AcceptTcpClient();

                Stream stream = new BufferedStream(oClient.GetStream());

                Console.WriteLine(GetVerb(stream));
                GetAuth(stream);


                oClient.Close();
            }

        }
    }

    //Clase servidor
    public class Server
    {
        

        public Server() { }

        static void Main(string[] args)
        {
            try
            {
                cLoop oLoop = new cLoop();
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

