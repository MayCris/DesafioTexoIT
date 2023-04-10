using ConsoleDesafioTexoIT;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando a leitura do arquivo.");

        LerArquivo();

        Console.WriteLine("Leitura finalizada com Sucesso.");
    }

    public static void LerArquivo()
    {
        Service service = new Service();


        //StreamReader rd = new StreamReader(@"C:/Users/HOME/Desktop/Texo IT/movielist.csv", Encoding.Default, true);

        //string linha = null;
        //string[] colunaseparada = null;

        //// ler o conteudo da linha e add na list 
        //while ((linha = rd.ReadLine()) != null)
        //{
        //    if (!linha.ToString().Contains("year"))
        //    {
        //        colunaseparada = linha.Split(';');

               // MovieList movie = new MovieList();
                //movie.Year = int.Parse(colunaseparada[0].ToString());
                //movie.Title = colunaseparada[1].ToString();
                ////movie.Studios = colunaseparada[2].ToString();
                ////movie.Producers = colunaseparada[3].ToString();
                //movie.Winner = string.IsNullOrEmpty(colunaseparada[4]) ? false : (colunaseparada[4] == "yes");

                //Chamar API para inclusão
        //    }
        //}

        service.Get();
    }
}