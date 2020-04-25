using QNomy.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QNomy.SQL
{
    /// <summary>
    /// <c>IRepository</c>
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Totals the number of all records in database of patients that is not handled.
        /// </summary>
        /// <returns>Totals the number of all records in database of patients that is not handled.</returns>
        Task<long> TotalNumberOfRecords();

        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IPatient>> GetAllPatients(int pageIndex = 1, int pageSize = 10);

        /// <summary>
        /// Adds the patient.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Fully constrcuted <see cref="IPatient"/> object</returns>
        Task<IPatient> AddPatient(IPatient name);

        /// <summary>
        /// Gets the last patient that was inserted
        /// </summary>
        /// <returns>Fulle constructed <see cref="Patient"/> object</returns>
        Task<IPatient> GetLastPatient();

        /// <summary>
        /// Sets the patient to status handled.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        Task SetPatientHandled(IPatient patient);
    }
}
