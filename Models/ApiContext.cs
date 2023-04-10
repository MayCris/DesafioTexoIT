using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
          : base(options)
        { }
        public DbSet<MovieList> MovieList { get; set; }
        public DbSet<Studios> Studios { get; set; }
        public DbSet<MovieXStudio> MovieXStudio { get; set; }
        public DbSet<Producers> Producers { get; set; }
        public DbSet<MovieProducers> MovieProducers { get; set; }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("MovieList"));
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Studios"));
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("MovieXStudio"));
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Producers"));
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("MovieProducers"));
        }
    }
}