using ApiFuncionarios.Infra.Data.Contexts;
using ApiFuncionarios.Infra.Data.Interfaces;
using ApiFuncionarios.Infra.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApiFuncionarios.Tests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(Assembly.Load("ApiFuncionarios.Services")).AddControllersAsServices();
            // leitura da connectionstring
            string connectionString = Configuration.GetConnectionString("BDApiFuncionarios");

            //configuração da classe SqlServecContext do projeto Infra.Data para funcionamento correto do Entity Framework
            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<IFuncionarioRepository, FuncionarioRepository>();

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
