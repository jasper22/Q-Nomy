using QNomy.Data;
using System.Collections.Generic;

namespace QNomy.Controllers
{
    /// <summary>
    /// <c>PatientsCollection</c>
    /// </summary>
    public class PatientsCollection
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable<IPatient> Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public long TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pages count.
        /// </summary>
        /// <value>
        /// The pages count.
        /// </value>
        public int PagesCount
        {
            get;
            set;
        }
    }
}
