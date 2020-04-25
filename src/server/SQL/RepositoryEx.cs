using Microsoft.EntityFrameworkCore;
using QNomy.Data;
using System.Collections.Generic;
using System.Linq;

namespace QNomy.SQL
{
    /// <summary>
    /// <c>Repository</c> class extensions
    /// </summary>
    public static class RepositoryEx
    {
        /// <summary>
        /// Actuals the patients - that's it - patients that is not handled
        /// </summary>
        /// <param name="dbSet">The database set.</param>
        /// <returns></returns>
        public static IQueryable<IPatient> ActualPatients(this DbSet<Patient> dbSet)
        {
            return dbSet.Where(patient => patient.Handled == false);
        }

        /// <summary>
        /// Assures that input enumerable is not null.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The provided enumerable if it's not null or empty sequence</returns>
        public static IEnumerable<IPatient> AssureNotNull(this IEnumerable<IPatient> source)
        {
            return source ?? Enumerable.Empty<IPatient>();
        }
    }
}
