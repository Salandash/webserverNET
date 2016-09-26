using System;
using System.IO;


namespace WebServer_Framework
{
    public class cResponse
    {
        string code;
        string text;
        string description;
       

        public cResponse()
        {
            code = "200";
            text = "OK";
            description = "Your request has succeeded";
        }

        //Métodoq ue imprime en consola el tipo de respuesta que será enviada al cliente.
        public cResponse(string codigo)
        {
            switch (codigo)
            {
                case "200":
                    code = codigo;
                    text = "OK";
                    description = "Your request has succeeded";
                    break;
                case "400":
                    code = codigo;
                    text = "Bad Request";
                    description = "The request could not be understood by the server due to malformed syntax";
                    break;
                case "401":
                    code = codigo;
                    text = "Unauthorized";
                    description = "The request requires valid user authentication.";
                    break;
                case "404":
                    code = codigo;
                    text = "Not Found";
                    description = "The server has not found anything matching the Request-URL";
                    break;
                default:
                    break;

            }
        }

        public void _printResponse()
        {
            Console.WriteLine();
            Console.WriteLine(code + " " + text);
            Console.WriteLine(description);
        }
    }
}
