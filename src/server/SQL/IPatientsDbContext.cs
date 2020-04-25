using Microsoft.EntityFrameworkCore;
using QNomy.Data;
using System.Threading.Tasks;

namespace QNomy.SQL
{
    /// <summary>
    /// <c>IPatientsDbContext</c>
    /// </summary>
    public interface IPatientsDbContext
    {
        /// <summary>
        /// Gets or sets the patients <see cref="DbSet{TEntity}"/>
        /// </summary>
        /// <value>
        /// The patients <see cref="DbSet{TEntity}"/>
        /// </value>
        DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// Saves the changes to database
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}