using Microsoft.AspNetCore.Mvc;
using Moq;
using QNomy.Controllers;
using QNomy.Data;
using QNomy.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QNomy.Test.Controllers
{
    public class PatientsController_XUnit
    {
        [Fact]
        public async Task Should_not_throw_on_actual_results()
        {
            var repositoryMock = new Mock<IRepository>();

            repositoryMock.Setup(r => r.GetAllPatients(1, int.MaxValue)).ReturnsAsync(GetPatients(10));

            var contoller = new PatientsController(repositoryMock.Object);

            var result = await contoller.GetPatients(1, int.MaxValue);

            Assert.NotNull(((OkObjectResult)result.Result).Value);

            var patientsList = ((OkObjectResult)result.Result).Value as PatientsCollection;

            Assert.Equal(10, patientsList.Data.Count());
        }

        [Fact]
        public async Task Should_not_throw_when_no_records()
        {
            var repositoryMock = new Mock<IRepository>();

            repositoryMock.Setup(r => r.GetAllPatients(1, int.MaxValue)).ReturnsAsync(Enumerable.Empty<IPatient>());

            var contoller = new PatientsController(repositoryMock.Object);

            var result = await contoller.GetPatients(1, int.MaxValue);

            Assert.NotNull(((OkObjectResult)result.Result).Value);

            var patientsList = ((OkObjectResult)result.Result).Value as PatientsCollection;

            Assert.Equal(0, patientsList.Data.Count());
        }

        //[Fact]
        //public async Task Should_change_status()
        //{
        //    var repositoryMock = new Mock<IRepository>();
        //    repositoryMock.Setup(r => r.GetAllPatients(1, int.MaxValue)).ReturnsAsync(GetPatients(2));
        //    var contoller = new PatientsController(repositoryMock.Object);
        //    var result = await contoller.GetPatients(1, int.MaxValue);
        //    Assert.NotNull(((OkObjectResult)result.Result).Value);
        //    var patientsList = ((OkObjectResult)result.Result).Value as PatientsCollection;
        //    Assert.Equal(2, patientsList.Data.Count());
        //    var updateResult = await contoller.HandlePatient(patientsList.Data.First() as Patient);
        //    result = await contoller.GetPatients(1, int.MaxValue);
        //    patientsList = ((OkObjectResult)result.Result).Value as PatientsCollection;
        //    Assert.Equal(1, patientsList.Data.Count());
        //}

        /// <summary>
        /// Gets the collection of <see cref="Patient"/> objects with property 'Handled' equal to false
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private static IEnumerable<IPatient> GetPatients(int count)
        {
            while(count > 0)
            {
                yield return new Patient
                {
                    Handled = false,
                    Name = Guid.NewGuid().ToString("N"),
                    TicketNumber = count,
                    Time = DateTime.Now
                };

                count--;
            }
        }
    }
}
