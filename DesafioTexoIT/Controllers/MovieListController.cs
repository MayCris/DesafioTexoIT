using DesafioTexoIT.Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Models;

namespace DesafioTexoIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieListController : ControllerBase
    {
        MovieListNegocio negocio = new MovieListNegocio();
        public MovieListController()
        {

        }

        [HttpGet]
        public Retorno Get()
        {
            ApiContext dbContext = LerArquivo();
            return negocio.Listar();
        }

        private ApiContext LerArquivo()
        {
            var collection = new ServiceCollection();
            ApiContext.ConfigureServices(collection);
            var serviceProvider = collection.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<ApiContext>();

            StreamReader rd = new StreamReader(@"C:/Temp/movielist.csv", Encoding.Default, true);
            negocio = new MovieListNegocio(dbContext);

            string linha = null;

            // ler o conteudo da linha e add na list 
            while ((linha = rd.ReadLine()) != null)
            {
                if (!linha.ToString().Contains("year"))
                {
                    negocio.CarregarDados(linha);
                }
            }

            dbContext.SaveChanges();
            return dbContext;
        }
    }
}