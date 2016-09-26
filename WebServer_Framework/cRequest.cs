using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace WebServer_Framework
{
    public class cRequest
    {
        Stream rawRequest;
        string strRequest;
        string username;
        string password;
        string strUrl;
        cResponse res;

        public cRequest(Stream req)
        {
            rawRequest = req;
            strRequest = requestToString(rawRequest);
            username = "William";
            password = "RaulMiraEtoImprime";
            res = new cResponse();
        }

        //Método que verifica si la petición pide por los archivos image.jpg o secret.txt.
        public bool bWantFile()
        {
            switch(strUrl)
            {
                case "/image.jpg":
                    return true;
                case "/secret.txt":
                    return true;
                default:
                    return false;
            }

        }
        
        //Método que administra la petición de los documentos image.jpg y secret.txt
        public string _fileManager()
        {
            if(strUrl == "/image.jpg")
            {
                return "image";
            }
            else
            {
                if(username == "William" && password == "RaulMiraEtoImprime")
                {
                    return "secret";
                }

                return "no";


            }
        }

        //Metodo que imprime en consola las credenciales de autenticación.
        public void _printAuth()
        {
            Hashtable reqHash = new Hashtable();
            string id;          //Identificador dentro del Hashtable
            string value;       //Valor del elemento indexado.
            string user =null;  //Nombre de usuario capturado en la petición.
            string pass =null;  //Clave de acceso capturada en la petición.
            string sAux;         //Variable auxiliar para manejar el while.
            int token;          //Elemento de división entre el ID y el VALUE.


            //Crear un HastTable para indexar la información del request
            while ((sAux= requestToString(rawRequest)) != null)
            {
                if (sAux == "")
                {
                    break;
                }
                    
                token = sAux.IndexOf(':');
                id = sAux.Substring(0, token++);

                while ( token< sAux.Length && sAux[token] == ' ')
                {
                    token++;
                }
                    
                value = sAux.Substring(token, sAux.Length - token);
                reqHash[id] = value;

            }

            
            if(reqHash.ContainsKey("Authorization"))
            {
                byte[] base64 = Convert.FromBase64String(reqHash["Authorization"].ToString().Split(' ')[1]);
                user = Encoding.UTF8.GetString(base64).Split(':')[0];
                pass = Encoding.UTF8.GetString(base64).Split(':')[1];


                username = user;
                password = pass;
                Console.WriteLine("Username: "+user);
                Console.WriteLine("Password: "+pass);
            }
            else
            {
                Console.WriteLine("NO AUTHENTICATION");
            }

        }

        /// Método que imprime en consola la url de la petición.
        public void _printUrl()
        {
            string url = strRequest.Split(' ')[1];
            strUrl = url;
            Console.WriteLine("URL: /" + url);
        }

        //Metodo que imprime en consola el verbo de la petición.
        public void _printVerb()
        {


            string verb = strRequest.Split(' ')[0];

            if (verb != "GET" && verb != "PUT" && verb != "POST" && verb != "DELETE")
            {
                Console.WriteLine("NOT A VALID VERB");
            }
            else
            {
                Console.WriteLine("VERB: " + verb);
            }

        }

        public string requestToString(Stream req)
        {
            string strReq = "";
            bool b = true;
            int aux;

            while (b)
            {
                aux = req.ReadByte();

                if (aux == '\n')
                {
                    break;
                }
                if (aux == '\r')
                {
                    continue;
                }
                if(aux == -1)
                {
                    Thread.Sleep(1);
                    continue;
                }

                strReq += Convert.ToChar(aux);


            }
            return strReq;

        }
    }
}

