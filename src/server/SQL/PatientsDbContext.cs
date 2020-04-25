using Microsoft.EntityFrameworkCore;
using QNomy.Data;
using System.Threading;
using System.Threading.Tasks;

namespace QNomy.SQL
{
    /// <summary>
    /// <c>PatientsDbContext</c>
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class PatientsDbContext : DbContext, IPatientsDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        public PatientsDbContext(DbContextOptions<PatientsDbContext> dbContextOptions) : base(dbContextOptions)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        /// Gets or sets the patients.
        /// </summary>
        /// <value>
        /// The patients.
        /// </value>
        public DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// Saves the changes to database
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync(true, CancellationToken.None);
        }
    }
}
