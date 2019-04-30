using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Translater.ServiceReference1;

namespace Translater
{
    class Program
    {
        static private string username = "Username"; // Smartweb username
        static private string password = "Password"; // Smartweb password
        static private string apikey = "ApiKey"; // Google translate API key
        static void Main(string[] args)
        {
            string path = "";
            string q = "";
            string target = "";
            string source = "";
            string format = "";
            string model = "";
            string key = "";
            string responseString1 = "";
            bool translatechecked = false;
            string translateTest = "0"; // Category ID brugt til oversættelse hvis man vælger test, standard er 0
            bool testchecker = false;
            bool testchecked = false;
            bool fullenglishcheck = false;
            bool fullenglish = false;

            var Client = new WebServicePortClient();

            /* Opret forbindelse til en løsning */
            Client.Solution_Connect(username, password);

            /* Sæt ønskede felter for Produkt-objektet */
            Client.Product_SetFields("Id,Title");
            Product product;

            while (translatechecked == false)
            {
                Console.WriteLine("Hvílket kategori skal have oversættelser (standard 0).");
                translateTest = Console.ReadLine();
                    Console.WriteLine("Du skrev "+translateTest);
                    translatechecked = true;
            }
            while (testchecker == false)
            {
                Console.WriteLine("Er dette en test? Ja/Nej");
                string updateTest = Console.ReadLine();

                // Hvis bruger taster Nej
                if (updateTest == "Nej")
                {
                    testchecked = true;
                    Console.WriteLine("Du skrev Nej");
                    testchecker = true;
                }
                // Hvis bruger taster Ja
                if (updateTest == "Ja")
                {
                    testchecked = false;
                    Console.WriteLine("Du skrev Ja");
                    testchecker = true;
                }
            }
            while (fullenglishcheck == false)
            {
                Console.WriteLine("Skal alle navne være kopi af original?");
                string updateTest = Console.ReadLine();

                // Hvis bruger taster Nej
                if (updateTest == "Nej")
                {
                    fullenglish = false;
                    Console.WriteLine("Du skrev Nej");
                    fullenglishcheck = true;
                }
                // Hvis bruger taster Ja
                if (updateTest == "Ja")
                {
                    fullenglish = true;
                    Console.WriteLine("Du skrev Ja");
                    fullenglishcheck = true;
                }
            }



            Product[] products = Client.Product_GetByCategory(int.Parse(translateTest));

            foreach (var prod in products) {
                Console.WriteLine(prod.Id);
                Console.WriteLine(prod.Title);

                // DK title tages over til andre sproglag som også skal være dansk
                Client.Solution_SetLanguage("DK1");
                product = Client.Product_GetById(prod.Id);
                if (product.Title == null || product.Title == "" || !(product.Title == ""))
                {
                    Console.WriteLine(prod.Title);
                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = prod.Title;
                        Client.Product_Update(langproduct);
                    }
                }

                Client.Solution_SetLanguage("DK2");
                product = Client.Product_GetById(prod.Id);
                if (product.Title == null || product.Title == "" || !(product.Title == ""))
                {
                    Console.WriteLine(prod.Title);
                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = prod.Title;
                        Client.Product_Update(langproduct);
                    }
                }

                Client.Solution_SetLanguage("DK3");
                product = Client.Product_GetById(prod.Id);
                if (product.Title == null || product.Title == "" || !(product.Title == ""))
                {
                    Console.WriteLine(prod.Title);
                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = prod.Title;
                        Client.Product_Update(langproduct);
                    }
                }

                Client.Solution_SetLanguage("DK4");
                product = Client.Product_GetById(prod.Id);
                if (product.Title == null || product.Title == "" || !(product.Title == ""))
                {
                    Console.WriteLine(prod.Title);
                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = prod.Title;
                        Client.Product_Update(langproduct);
                    }
                }
                Client.Solution_SetLanguage("DK5");
                product = Client.Product_GetById(prod.Id);
                if (product.Title == null || product.Title == "" || !(product.Title == ""))
                {
                    Console.WriteLine(prod.Title);
                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = prod.Title;
                        Client.Product_Update(langproduct);
                    }
                }



                if (!fullenglish) {

                // English 
                // Parameters
                q = "q=" + prod.Title.ToString();
                target = "target=en";
                source = "source=da";
                format = "format=text";
                model = "model=base";
                key = "key="+apikey;

                path = "https://translation.googleapis.com/language/translate/v2?" + q + "&" + target + "&" + source + "&" + format + "&" + key
                //+ "&" + model
                ;
                Client.Solution_SetLanguage("UK");
                product = Client.Product_GetById(prod.Id);
                    // Kun oversæt hvis titel er tom
                    if (product.Title == null || product.Title == "")
                {

                        // Google translate Http request
                        responseString1 = "";

                        HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(path);
                        request1.Method = WebRequestMethods.Http.Get;

                        using (var response1 = request1.GetResponse())
                        using (var responseStream1 = response1.GetResponseStream())
                        using (var reader1 = new StreamReader(responseStream1))
                        {
                            responseString1 = reader1.ReadToEnd();
                        }

                        // Parse request
                JObject JResponse = JObject.Parse(responseString1);

                Console.WriteLine(JResponse["data"]["translations"][0]["translatedText"].ToString());

                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = JResponse["data"]["translations"][0]["translatedText"].ToString();
                        Client.Product_Update(langproduct);
                    }

                }

                // German
                // Parameters
                q = "q=" + prod.Title.ToString();
                target = "target=de";
                source = "source=da";
                format = "format=text";
                model = "model=base";
                key = "key=" + apikey;

                path = "https://translation.googleapis.com/language/translate/v2?" + q + "&" + target + "&" + source + "&" + format + "&" + key
                //+ "&" + model
                ;
                Client.Solution_SetLanguage("DE");
                product = Client.Product_GetById(prod.Id);

                    // Kun oversæt hvis titel er tom
                    if (product.Title == null || product.Title == "")
                {
                        // Google translate Http request
                        responseString1 = "";

                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(path);
                    request1.Method = WebRequestMethods.Http.Get;

                    using (var response1 = request1.GetResponse())
                    using (var responseStream1 = response1.GetResponseStream())
                    using (var reader1 = new StreamReader(responseStream1))
                    {
                        responseString1 = reader1.ReadToEnd();
                    }

                        // Parse request
                        JObject JResponse = JObject.Parse(responseString1);

                    Console.WriteLine(JResponse["data"]["translations"][0]["translatedText"].ToString());

                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = JResponse["data"]["translations"][0]["translatedText"].ToString();
                        Client.Product_Update(langproduct);
                    }

                }

                // Swedish
                // Parameters
                q = "q=" + prod.Title.ToString();
                target = "target=sv";
                source = "source=da";
                format = "format=text";
                model = "model=base";
                key = "key=" + apikey;

                path = "https://translation.googleapis.com/language/translate/v2?" + q + "&" + target + "&" + source + "&" + format + "&" + key
                //+ "&" + model
                ;
                Client.Solution_SetLanguage("SE");
                product = Client.Product_GetById(prod.Id);
                
                // Kun oversæt hvis titel er tom
                if (product.Title == null || product.Title == "")
                {
                        // Google translate Http request
                        responseString1 = "";
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(path);
                    request1.Method = WebRequestMethods.Http.Get;

                    using (var response1 = request1.GetResponse())
                    using (var responseStream1 = response1.GetResponseStream())
                    using (var reader1 = new StreamReader(responseStream1))
                    {
                        responseString1 = reader1.ReadToEnd();
                    }

                        // Parse request
                        JObject JResponse = JObject.Parse(responseString1);

                    Console.WriteLine(JResponse["data"]["translations"][0]["translatedText"].ToString());

                    if (testchecked) { 
                    ProductUpdate langproduct = new ProductUpdate();
                    langproduct.Id = prod.Id;
                    langproduct.IdSpecified = true;
                    langproduct.Title = JResponse["data"]["translations"][0]["translatedText"].ToString();
                    Client.Product_Update(langproduct);
                    }
                }

                // Norwegian
                // Parameters
                q = "q=" + prod.Title.ToString();
                target = "target=no";
                source = "source=da";
                format = "format=text";
                model = "model=base";
                key = "key=" + apikey;

                path = "https://translation.googleapis.com/language/translate/v2?" + q + "&" + target + "&" + source + "&" + format + "&" + key
                //+ "&" + model
                ;
                Client.Solution_SetLanguage("NO");
                product = Client.Product_GetById(prod.Id);

                // Kun oversæt hvis titel er tom
                if (product.Title == null || product.Title == "" || !(product.Title == ""))
                {

                    // Google translate Http request
                    responseString1 = "";

                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(path);
                    request1.Method = WebRequestMethods.Http.Get;

                    using (var response1 = request1.GetResponse())
                    using (var responseStream1 = response1.GetResponseStream())
                    using (var reader1 = new StreamReader(responseStream1))
                    {
                        responseString1 = reader1.ReadToEnd();
                    }

                        // Parse request
                        JObject JResponse = JObject.Parse(responseString1);

                    Console.WriteLine(JResponse["data"]["translations"][0]["translatedText"].ToString());

                    if (testchecked)
                    {
                        ProductUpdate langproduct = new ProductUpdate();
                        langproduct.Id = prod.Id;
                        langproduct.IdSpecified = true;
                        langproduct.Title = JResponse["data"]["translations"][0]["translatedText"].ToString();
                        Client.Product_Update(langproduct);
                    }
                }
                Console.WriteLine("_______________________________________");
                }else
                {
                    // Hvis hovedsproglag titel skal bruges i stedet for google translate

                    // English
                    Client.Solution_SetLanguage("UK");
                    product = Client.Product_GetById(prod.Id);

                    if (product.Title == null || product.Title == "" || !(product.Title == ""))
                    {
                        Console.WriteLine(prod.Title);
                        if (testchecked)
                        {
                            ProductUpdate langproduct = new ProductUpdate();
                            langproduct.Id = prod.Id;
                            langproduct.IdSpecified = true;
                            langproduct.Title = prod.Title;
                            Client.Product_Update(langproduct);
                        }
                    }
                    // German
                    Client.Solution_SetLanguage("DE");
                    product = Client.Product_GetById(prod.Id);

                    if (product.Title == null || product.Title == "" || !(product.Title == ""))
                    {
                        Console.WriteLine(prod.Title);
                        if (testchecked)
                        {
                            ProductUpdate langproduct = new ProductUpdate();
                            langproduct.Id = prod.Id;
                            langproduct.IdSpecified = true;
                            langproduct.Title = prod.Title;
                            Client.Product_Update(langproduct);
                        }
                    }
                    // Swedish
                    Client.Solution_SetLanguage("SE");
                    product = Client.Product_GetById(prod.Id);

                    if (product.Title == null || product.Title == "" || !(product.Title == ""))
                    {
                        Console.WriteLine(prod.Title);
                        if (testchecked)
                        {
                            ProductUpdate langproduct = new ProductUpdate();
                            langproduct.Id = prod.Id;
                            langproduct.IdSpecified = true;
                            langproduct.Title = prod.Title;
                            Client.Product_Update(langproduct);
                        }
                    }
                    // Norwegian
                    Client.Solution_SetLanguage("NO");
                    product = Client.Product_GetById(prod.Id);

                    if (product.Title == null || product.Title == "" || !(product.Title == ""))
                    {
                        Console.WriteLine(prod.Title);
                        if (testchecked)
                        {
                            ProductUpdate langproduct = new ProductUpdate();
                            langproduct.Id = prod.Id;
                            langproduct.IdSpecified = true;
                            langproduct.Title = prod.Title;
                            Client.Product_Update(langproduct);
                        }
                    }
                }

            }
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
