using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QNomy;

namespace QNomy.SQL 
{
    ///<summary>
    /// <c>DatabaseMigration</c>
    ///</summary>
    public static class DatabaseMigration
    {
        /// <summary>
        /// Migrate/update database
        /// </summary>
        /// <param name="webHost"></param>
        public static IHost MigrateDatabase<T>(this IHost webHost) where T: DbContext
        {
            var services = webHost.Services;

            var logger = services.GetService(typeof(ILogger<Program>)) as ILogger<Program>;

            try
            {
                logger.LogInformation($"Starting to migrate the database");

                var db = services.GetService(typeof(T)) as DbContext;

                db.Database.Migrate();
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
            }

            return webHost;            
        }
    }
}
