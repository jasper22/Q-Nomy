using QNomy.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNomy.Data
{
    public class PatientsFactory
    {
        private IRepository dbContext;

        public PatientsFactory(IRepository repository)
        {
            this.dbContext = repository;
        }

        public async Task<IPatient> CreateNewPatient(string name)
        {
            var time = CalculateNewTime();

            return new Patient
            {
                Handled = false,
                Name = name,
                Time = await time
            };
        }


        private async Task<DateTime> CalculateNewTime()
        {
            var lastCustomer = await this.dbContext.GetLastPatient();

            if (lastCustomer == null)
            {
                // Still no patients in database
                // return current date
                return DateTime.Now;
            }

            return lastCustomer.Time.AddMinutes(15);
        }
    }
}
