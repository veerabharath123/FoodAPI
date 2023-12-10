using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Database
{
    public static class DbResgistrations
    {
        public static void Registor(this IServiceCollection services, IConfiguration Configuration)
        {
            var CurrentDb = Configuration.GetSection("CurrentDb").Value;
            services.AddDbContext<FoodDbContext>(x => x.UseSqlServer(Configuration.GetSection("ConnectionStrings").GetSection(CurrentDb!).Value).EnableSensitiveDataLogging());
        }
    }
}
