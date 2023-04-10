using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDesafioTexoIT
{
    public class Service
    {
        private string urlApi = "http://localhost:7167/api";
        public MovieList Get()
        {
            MovieList movieList = new MovieList();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(urlApi).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    movieList = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieList>(result);
                }
            }
            catch
            {

            }

            return movieList;
        }
    }
}
