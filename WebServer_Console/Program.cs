using System;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace WebServer_Console
{

    //Clase que maneja el loop donde corre el listener
    public class cLoop
    {
        int port = 8080;
        string host = "localhost:";
        StreamContent streamContent;


        //Metodo que retorna el verbo de la petición al servidor
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

        //Metodo que imprime en consola las credenciales de autenticación.
        public void _GetAuth(Stream request)
        {
            byte[] byteData;
            string decodified;
            string[] splitString;
            string sAux="";
            bool b = true;
            int iAux;
            string base64;

            while(b)
            {
                iAux = request.ReadByte();
                if (iAux == '\n')
                {
                    break;
                }

                if (iAux == '\r')
                {
                    continue;
                }
                if (iAux == -1)
                {
                    Thread.Sleep(1);
                    continue;
                };
                sAux += Convert.ToChar(iAux);
            }

            base64 = sAux.Split(' ')[1]; 

            byteData = Convert.FromBase64String(sAux); 

            decodified = Encoding.UTF8.GetString(byteData);

            splitString = decodified.Split(':');

            if (splitString.Length < 2)
            {
                Console.WriteLine("Error en petición");
            }



            Console.WriteLine(splitString[0]);
            Console.WriteLine(splitString[1]);
        }

        /// Método que imprime en consola la extensión del archivo buscado en la petición.
        public void _GetExt(Stream request)
        {
            byte[] byteData;
            string decodified;
            string[] splitString;
            string aux;
            string[] extension;

            string base64 = request.ToString();

            aux = base64.Split(' ')[1]; //Se divide el request en string

            byteData = Convert.FromBase64String(aux);

            decodified = Encoding.UTF8.GetString(byteData);

            extension = decodified.Split('.');

            Console.WriteLine("."+extension[1]);

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

                _GetAuth(stream);
                Console.WriteLine(GetVerb(stream));
                


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

