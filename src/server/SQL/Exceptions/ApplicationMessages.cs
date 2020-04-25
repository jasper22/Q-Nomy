using QNomy.Data;

namespace QNomy.SQL.Exceptions
{
    /// <summary>
    /// <c>ApplicationMessages</c>
    /// </summary>
    public static class ApplicationMessages
    {
        /// <summary>
        /// Generals the database exception message.
        /// </summary>
        /// <returns></returns>
        public static string GeneralDbExceptionMessage()
        {
            return Properties.Resources.GeneralDbExceptionMessage;
        }

        /// <summary>
        /// Patients the is not found in database
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static string PatientIsNotFound(IPatient patient)
        {
            return string.Format(Properties.Resources.PatientNotFoundInDbException, patient.ToString());
        }
    }
}
