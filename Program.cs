using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        const string baseUrl = "https://swapi.co/api/";

        static void Main(string[] args)
        {
            string planets = "planets/?page=";
            CallRestMethod( baseUrl + planets);
        }

        public static void CallRestMethod(string url)
        {
            bool exceptionthrown = false;
            int page = 1;

            while (!exceptionthrown)
            {
                string pageStr = page.ToString();
                Uri planetsUri = new Uri(url + pageStr);

                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(planetsUri);

                    // request is instantiated, now we call the get responce method on this object...
                    HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

                    //instantiate an instance of stream reader, which is passed the responce object instance 
                    StreamReader responceStream = new StreamReader(responce.GetResponseStream(), Encoding.UTF8);

                    //read formatted json responce 
                    JObject workingResponce = JObject.Parse(responceStream.ReadToEnd());

                    foreach (JObject value in workingResponce["results"])
                    {
                        Console.WriteLine("Planet Name: ");
                        Console.WriteLine(value["name"]);
                        Console.WriteLine("Films: ");
                        JToken checkPage = workingResponce["next"];
                        var checkType = value["films"] as JArray;

                        if (checkType.Count < 1)
                        {
                            Console.WriteLine("no films for this category");
                            Console.WriteLine("\n");
                        }
                        else
                        {
                            foreach (JToken val in value["films"])
                            {
                                String strVal = Convert.ToString(val);
                                Uri requestFilm = new Uri(strVal);
                                CallFilmRequest(requestFilm);
                            }
                        }
                        Console.WriteLine("\n");

                        if (checkPage.Type == JTokenType.Null)
                        {
                            Console.WriteLine("You have reached the end");
                            Environment.Exit(0);
                        }
                    }

                    page += 1;
                    Console.WriteLine("moving to next page in the API:");
                    Console.WriteLine(page);
                    Console.WriteLine("\n");
                }
                catch (Exception)
                {
                    exceptionthrown = true;
                    throw;
                }
            }
        }
 
    public static void CallFilmRequest(Uri url)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            // request is instantiated, now we call the get responce method on this object...
            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

            //instantiate an instance of stream reader, which is passed the responce object instance 
            StreamReader responceStream = new StreamReader(responce.GetResponseStream(), Encoding.UTF8);

            //read formatted json responce 
            JObject workingResponce = JObject.Parse(responceStream.ReadToEnd());
            Console.WriteLine(workingResponce["title"]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
 }
}