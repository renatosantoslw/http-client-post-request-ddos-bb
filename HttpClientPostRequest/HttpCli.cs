using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Net.Http;

namespace HttpClientPostRequest
{
    internal class HttpCli
    {
        //CASO QUEIRA ENVIAR UM ARQUIVO BEM GRANDE
        public static string strTXTBiblia = string.Empty;
               
        static string MakeId(int length)
        {
            string result = "";
            const string characters = "0123456789";
            int charactersLength = characters.Length;
            int counter = 0;
            Random random = new Random();

            while (counter < length)
            {
                result += characters[random.Next(0, charactersLength)];
                counter++;
            }

            return result;
        }

        public static void Client(long i)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"TaskAtualID: {Task.CurrentId}");

            try
            {

                //CRIA OBJ PARA GERAR NUMEROS ALEATORIOS
                Random rnd = new Random();
                
                //PARAMETROS PARA SE USAR COOKIES NAS RESQUISIÇÕES
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;
                handler.UseCookies = true;
               

                using (HttpClient client = new HttpClient(handler))
                {


                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 6.0.1; Moto G (4)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Mobile Safari/537.36");
         
                    string strPOSTlogin = "https://pontos-online.site/api/login";
                    string strPOSTloginDNS2 = "https://bb-pontos.online/api/login";
                    string strPOSTloginDNS3 = "https://resgate-seus-pontos.co/api/login";

                    //CRIA NUMEROS ALEATORIOS DA AGENCIA
                    string strinput_ag_RND = rnd.NextInt64(000000000000000000, 999999999999999999).ToString("D18");
                    string strinput_di_RND = rnd.Next(0, 9).ToString("D1");
                    //CRIA NUMEROS ALEATORIOS DA CONTA
                    string strinput_cc_RND = rnd.Next(00000, 99999).ToString("D5");
                    string strinput_digCC_RND = rnd.Next(0, 9).ToString("D1");
                    //CRIA NUMEROS ALEATORIOS DA SENHA DE 8 DIGITOS
                    string strinput_sn8_RND = rnd.Next(00000000, 99999999).ToString("D8");
                    //CRIA NUMEROS ALEATORIOS DA SENHA DE 6 DIGITOS
                    string strinput_sn6_RND = rnd.Next(000000, 999999).ToString("D6");
                    string strinput_cvv_RND = rnd.Next(000, 999).ToString("D3");

                    //FORMATA OS DADOS
                    string strbranch = $"{strinput_ag_RND+strinput_ag_RND}-{strinput_di_RND}";
                    string straccount = $"{strinput_cc_RND}-{strinput_digCC_RND}";
                    string strpassword = $"{strinput_sn8_RND}";
                    string strpassword6 = $"{strinput_sn6_RND}";

                    var ContentPOSTJSonI = new
                    {
                        branch = strbranch,
                        account = strbranch,
                        password = strpassword,
                        password6 = string.Empty
                    };
                 

                    var contentJson = JsonContent.Create(ContentPOSTJSonI);

                    var Response = client.PostAsync(strPOSTlogin, contentJson).Result;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i} - StatusResponde_API1 = {(int)Response.StatusCode} - {Response.ReasonPhrase}");

                    /*
                    Console.ForegroundColor = ConsoleColor.White;
                    var ContentResponse = Response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(ContentResponse);
                    */

                    //-----------------------------------------------------------------------------------

                    //ENVIA O POST/REQUEST PARA A SEGUNDA API
                    Response = client.PostAsync(strPOSTloginDNS2, contentJson).Result;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i} - StatusResponde_API2 = {(int)Response.StatusCode} - {Response.ReasonPhrase}");

                    /*
                    Console.ForegroundColor = ConsoleColor.White;
                    ContentResponse = Response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(ContentResponse);
                    */

                    //-----------------------------------------------------------------------------------

                    //ENVIA O POST/REQUEST PARA A SEGUNDA API
                    Response = client.PostAsync(strPOSTloginDNS3, contentJson).Result;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i} - StatusResponde_API3 = {(int)Response.StatusCode} - {Response.ReasonPhrase}");

                    /*
                    Console.ForegroundColor = ConsoleColor.White;
                    ContentResponse = Response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(ContentResponse);
                    */
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ChildTaskException: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;

                    using (StreamWriter writer = new StreamWriter($"ChildTaskException{i}.txt", append: false))
                    {
                        writer.WriteLine($"ChildTaskException: {ex.Message}");
                    }
                    
                }
                catch (Exception exx)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ChildTaskException_GravarArquivoTXT: {exx.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    
    
    
    }
}
