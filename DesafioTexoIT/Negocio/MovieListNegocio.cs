using Microsoft.EntityFrameworkCore;
using Models;

namespace DesafioTexoIT.Negocio
{
    public class MovieListNegocio
    {
        List<MovieList> lstMovieList = new List<MovieList>();
        List<Producers> lstProducers = new List<Producers>();
        List<Studios> lstStudios = new List<Studios>();
        List<MovieXStudio> lstMovieXStudio = new List<MovieXStudio>();
        List<MovieProducers> lstMovieProducers = new List<MovieProducers>();
        string[] colunaseparada = null;
        ApiContext dbContext;

        public MovieListNegocio() { }
        public MovieListNegocio(ApiContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void CarregarDados(string linha)
        {
            colunaseparada = linha.Split(';');

            GravarStudios();
            GravarProdutores();
            GravarMovieList();
        }

        private void GravarStudios()
        {
            //+ de um Studio no mesmo Filme
            if (colunaseparada[2].ToString().Contains(','))
            {
                string[] splitStudios = colunaseparada[2].ToString().Split(',');
                foreach (var item in splitStudios)
                {
                    MovieXStudio itemMovieStudio = new MovieXStudio();

                    Studios itemStudio = new Studios();
                    bool adicionar = (lstStudios.Count == 0) ||
                        ((lstStudios.Count > 0) && (lstStudios.Where(s => s.Studio.Equals(item.ToString())).Count() == 0));

                    if (adicionar)
                    {
                        itemStudio.Id = lstStudios.Count + 1;
                        itemStudio.Studio = item.ToString();
                        lstStudios.Add(itemStudio);

                        //Adiciona na lista de associação Studio X Movie
                        lstMovieXStudio.Add(new MovieXStudio { IdStudio = itemStudio.Id, IdMovie = 0, Id = lstMovieXStudio.Count() + 1 });
                        dbContext.Studios.Add(itemStudio);
                    }
                    else
                    {
                        //Adiciona na lista de associação Studio X Movie
                        lstMovieXStudio.Add(new MovieXStudio
                        {
                            IdStudio = lstStudios.FirstOrDefault(s => s.Studio.Equals(item.ToString())).Id,
                            IdMovie = 0,
                            Id = lstMovieXStudio.Count() + 1
                        });
                    }
                }
            }
            else
            {
                Studios itemStudio = new Studios();
                itemStudio.Id = lstStudios.Count + 1;
                itemStudio.Studio = colunaseparada[2].ToString();
                lstStudios.Add(itemStudio);

                //Adiciona na lista de associação Studio X Movie
                lstMovieXStudio.Add(new MovieXStudio { IdStudio = itemStudio.Id, IdMovie = 0, Id = lstMovieXStudio.Count() + 1 });
                dbContext.Studios.Add(itemStudio);
            }
        }

        private void GravarProdutores()
        {
            if (colunaseparada[3].ToString().Contains(',') || colunaseparada[3].ToString().ToLower().Contains(" and "))
            {
                List<string> lst = new List<string>();
                string[] splitProducers = colunaseparada[3].ToString().Split(',');

                for (int i = 0; i < splitProducers.Length; i++)
                {
                    string[] splitProducersAnd = splitProducers[i].ToString().Split(" and ");
                    for (int j = 0; j < splitProducersAnd.Length; j++)
                    {
                        lst.Add(splitProducersAnd[j].ToString().Trim());
                    }
                }

                foreach (var item in lst)
                {
                    Producers producers = new Producers();

                    bool adicionar = (lstProducers.Count == 0) ||
                                    ((lstProducers.Count > 0) && (lstProducers.Where(s => s.Nome.Equals(item.ToString())).Count() == 0));

                    if (adicionar)
                    {
                        producers.Id = lstProducers.Count + 1;
                        producers.Nome = item.ToString();
                        lstProducers.Add(producers);

                        //Adiciona na lista de associação Movie X Producers
                        lstMovieProducers.Add(new MovieProducers { IdProducer = producers.Id, IdMovie = 0, Id = lstMovieProducers.Count() + 1 });
                        dbContext.Producers.Add(producers);
                    }
                    else
                    {
                        //Adiciona na lista de associação Movie X Producers
                        lstMovieProducers.Add(new MovieProducers
                        {
                            IdProducer = lstProducers.FirstOrDefault(s => s.Nome.Equals(item.ToString())).Id,
                            IdMovie = 0,
                            Id = lstMovieProducers.Count() + 1
                        });
                    }
                }
            }
            else
            {
                bool adicionar = (lstProducers.Count == 0) ||
                                    ((lstProducers.Count > 0) && (lstProducers.Where(s => s.Nome.Equals(colunaseparada[3].ToString())).Count() == 0));

                if (adicionar)
                {
                    Producers itemProducers = new Producers();
                    itemProducers.Id = lstProducers.Count + 1;
                    itemProducers.Nome = colunaseparada[3].ToString();
                    lstProducers.Add(itemProducers);

                    lstMovieProducers.Add(new MovieProducers { IdProducer = itemProducers.Id, IdMovie = 0, Id = lstMovieProducers.Count() + 1 });
                    dbContext.Producers.Add(itemProducers);
                }
                else
                {
                    lstMovieProducers.Add(new MovieProducers
                    {
                        IdProducer = lstProducers.FirstOrDefault(s => s.Nome.Equals(colunaseparada[3].ToString())).Id,
                        IdMovie = 0,
                        Id = lstMovieProducers.Count() + 1
                    });
                }
            }
        }

        private void GravarMovieList()
        {
            MovieList movie = new MovieList();
            movie.Year = int.Parse(colunaseparada[0].ToString());
            movie.Title = colunaseparada[1].ToString();
            movie.Winner = string.IsNullOrEmpty(colunaseparada[4]) ? false : (colunaseparada[4] == "yes");

            lstMovieList.Add(movie);
            dbContext.MovieList.Add(movie);

            GravarMovieStudios(movie.Id);
            GravarMovieProducers(movie.Id);
        }

        public void GravarMovieStudios(int idMovie)
        {
            foreach (MovieXStudio item in lstMovieXStudio.Where(m => m.IdMovie == 0).ToList())
            {
                item.IdMovie = idMovie;
                dbContext.MovieXStudio.Add(item);
            }
        }

        public void GravarMovieProducers(int idMovie)
        {
            foreach (MovieProducers item in lstMovieProducers.Where(m => m.IdMovie == 0).ToList())
            {
                item.IdMovie = idMovie;
                dbContext.MovieProducers.Add(item);
            }
        }

        public Retorno Listar()
        {
            int[] IdProducers = lstMovieProducers.Select(x => x.IdProducer).Distinct().ToArray();

            Retorno retorno = new Retorno();
            retorno.max = new List<DadosRetorno>();
            retorno.min = new List<DadosRetorno>();

            DadosRetorno min = new DadosRetorno();
            DadosRetorno max = new DadosRetorno();

            List<MovieProducers> lstMoviesProducer = new List<MovieProducers>();
            string Producer = "";

            for (int i = 0; i < IdProducers.Length; i++)
            {
                List<int> lstYear = new List<int>();
                Producer = lstProducers.Where(x => x.Id == IdProducers[i]).FirstOrDefault().Nome.ToString();

                foreach (var item in lstMovieProducers.Where(x => x.IdProducer == IdProducers[i]).ToList())
                {
                    lstYear.Add(lstMovieList.Where(x => x.Id == item.IdMovie).FirstOrDefault().Year);
                }

                lstYear.Order();

                retorno.min.AddRange(ListarMin(lstYear, Producer));
                retorno.max.AddRange(ListarMax(lstYear, Producer));
            }

            Retorno ret = new Retorno();
            ret.min = new List<DadosRetorno>();
            ret.min.AddRange(retorno.min.OrderBy(x => x.interval).Where(x => x.interval > 0).Take(5));

            ret.max = new List<DadosRetorno>();
            ret.max.AddRange(retorno.max.OrderByDescending(x => x.interval).Take(5));


            return ret;
        }

        private List<DadosRetorno> ListarMin(List<int> lstYear, string Producer)
        {
            List<DadosRetorno> lstMin = new List<DadosRetorno>();
            DadosRetorno min = new DadosRetorno();

            if (lstYear.Count > 2)
            {
                List<DadosRetorno> lstControleMin = new List<DadosRetorno>();
                for (int j = 0; j < lstYear.Count(); j++)
                {
                    DadosRetorno controle = new DadosRetorno();
                    controle.previousWin = j == 0 ? lstYear[j] : lstYear[j - 1];
                    controle.followingWin = lstYear[j];
                    controle.interval = controle.followingWin - controle.previousWin;

                    lstControleMin.Add(controle);
                }

                min = lstControleMin.Where(x => x.interval == (lstControleMin.Select(x => x.interval).Min())).FirstOrDefault();
                min.producer = Producer;


                if (min.interval < (min.followingWin - min.previousWin))
                {
                    lstMin.Add(min);
                }
            }
            else
            {
                lstMin.Add(new DadosRetorno
                {
                    producer = Producer,
                    previousWin = lstYear.Min(),
                    followingWin = lstYear.Max(),
                    interval = (lstYear.Max() - lstYear.Min())
                });
            }

            return lstMin;
        }

        private List<DadosRetorno> ListarMax(List<int> lstYear, string Producer)
        {
            List<DadosRetorno> lstMax = new List<DadosRetorno>();
            DadosRetorno max = new DadosRetorno();

            if (lstYear.Count > 2)
            {
                List<DadosRetorno> lstControleMin = new List<DadosRetorno>();
                for (int j = 0; j < lstYear.Count(); j++)
                {
                    DadosRetorno controle = new DadosRetorno();
                    controle.previousWin = j == 0 ? lstYear[j] : lstYear[j - 1];
                    controle.followingWin = lstYear[j];
                    controle.interval = controle.followingWin - controle.previousWin;

                    lstControleMin.Add(controle);
                }

                max = lstControleMin.Where(x => x.interval == (lstControleMin.Select(x => x.interval).Max())).FirstOrDefault();
                max.producer = Producer;


                if (max.interval < (max.followingWin - max.previousWin))
                {
                   lstMax.Add(max);
                }
            }
            else
            {
                lstMax.Add(new DadosRetorno
                {
                    producer = Producer,
                    previousWin = lstYear.Min(),
                    followingWin = lstYear.Max(),
                    interval = (lstYear.Max() - lstYear.Min())
                });                
            }

            return lstMax;
        }
    }
}
