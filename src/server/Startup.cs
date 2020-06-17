using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QNomy.SQL;

namespace QNomy
{
    /// <summary>
    /// <c>Startup</c>
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPatientsDbContext, PatientsDbContext>();
            services.AddScoped<IRepository, Repository>();

            services.AddControllers();

            services.AddCors(options =>
            {
                // Disable CORS for this project
                options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()
                                                         .AllowAnyMethod()
                                                         .AllowAnyHeader());
            });

            services.AddDbContext<PatientsDbContext>(opt =>
            {
                Initialize(opt);
            });

            services.AddApplicationInsightsTelemetry();
        }

        private void Initialize(DbContextOptionsBuilder opt)
        {
            var dbType = Environment.GetEnvironmentVariable("DATABASE_TYPE") ?? string.Empty;
            var dbServerAddress = Environment.GetEnvironmentVariable("DATABASE_SERVER") ?? string.Empty;
            var dbUserName = Environment.GetEnvironmentVariable("DB_USER") ?? string.Empty;
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? string.Empty;

            string connectionStringBase = string.Empty;

            switch(dbType)
            {
                case "POSTGRES":
                    connectionStringBase = $"User ID={dbUserName};Password={dbPassword};Host={dbServerAddress};Port=5432;Pooling=true;MinPoolSize=5;MaxPoolSize=50;";
                    opt.UseNpgsql(connectionStringBase);
                    break;
                    
                case "MYSQL":
                    connectionStringBase = $"User Id={dbUserName};Password={dbPassword};Server={dbServerAddress};";
                    opt.UseSqlServer(connectionStringBase);
                    break;

                default:
                    throw new ApplicationException("Database type is not defined in environment variables !");
                    break;
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
