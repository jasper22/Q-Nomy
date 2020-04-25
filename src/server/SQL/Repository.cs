using Microsoft.EntityFrameworkCore;
using QNomy.Data;
using QNomy.SQL.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QNomy.SQL
{
    /// <summary>
    /// <c>Repository</c>
    /// </summary>
    public class Repository : IRepository
    {
        private IPatientsDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="patientsDbContext">The patients database context.</param>
        public Repository(IPatientsDbContext patientsDbContext)
        {
            this.dbContext = patientsDbContext;
        }

        /// <summary>
        /// Totals the number of all records in database of patients that is not handled.
        /// </summary>
        /// <returns>
        /// Totals the number of all records in database of patients that is not handled.
        /// </returns>
        public async Task<long> TotalNumberOfRecords()
        {
            return await this.dbContext.Patients.ActualPatients().CountAsync();            
        }


        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns></returns> 
        /// <exception cref="GeneralDbException"></exception>
        public async Task<IEnumerable<IPatient>> GetAllPatients(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var result = await this.dbContext.Patients.ActualPatients()
                                                          .Skip((pageIndex - 1) * pageSize)
                                                          .Take(pageSize)
                                                          .ToListAsync();

                return result.AssureNotNull();
            }
            catch(Exception ex)
            {
                throw new GeneralDbException(ApplicationMessages.GeneralDbExceptionMessage(), ex);
            }
        }

        /// <summary>
        /// Adds the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        /// <exception cref="QNomy.SQL.Exceptions.GeneralDbException"></exception>
        public async Task<IPatient> AddPatient(IPatient patient)
        {
            try
            {
                var result = await this.dbContext.Patients.AddAsync(patient as Patient);

                await this.dbContext.SaveChangesAsync();

                return result.Entity;
            }
            catch(Exception exp)
            {
                throw new GeneralDbException(ApplicationMessages.GeneralDbExceptionMessage(), exp);
            }
        }

        /// <summary>
        /// Gets the last patient that was inserted
        /// </summary>
        /// <returns>
        /// Fulle constructed <see cref="Patient" /> object
        /// </returns>
        public async Task<IPatient> GetLastPatient()
        {
            try
            {
                var result = await this.dbContext.Patients.ActualPatients()
                                                          .OrderByDescending(patient => patient.TicketNumber)
                                                          .FirstOrDefaultAsync();
                return result;
            }
            catch(Exception exp)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the patient to status handled.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <exception cref="QNomy.SQL.Exceptions.GeneralDbException">
        /// </exception>
        public async Task SetPatientHandled(IPatient patient)
        {
            try
            {
                var currentPatient = await this.dbContext.Patients.FindAsync(patient.TicketNumber);

                if (currentPatient == null)
                {
                    throw new GeneralDbException(ApplicationMessages.PatientIsNotFound(patient));
                }

                currentPatient.Handled = true;

                this.dbContext.Patients.Update(currentPatient);

                await this.dbContext.SaveChangesAsync();
            }
            catch(GeneralDbException expDb)
            {
                // It was thrown above in 'not found scenario'
                throw;
            }
            catch(Exception exp)
            {
                throw new GeneralDbException(ApplicationMessages.GeneralDbExceptionMessage(), exp);
            }
        }
    }
}
