using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QNomy.Data;
using QNomy.SQL;
using QNomy.SQL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNomy.Controllers
{
    /// <summary>
    /// <c>PatientsController</c>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    public class PatientsController : ControllerBase
    {
        private readonly IRepository dbContext;

        private ILogger<PatientsController> log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public PatientsController(IRepository repository, ILogger<PatientsController> logger = null)
        {
            this.dbContext = repository;

            this.log = logger ?? LoggerFactory.Create((configuration) =>
            {
                configuration.AddDebug();
            }).CreateLogger<PatientsController>();
        }

        /// <summary>
        /// Gets the all patients that is not handled
        /// </summary>
        /// <returns>Collection of <see cref="Patient"/> object if possible</returns>
        [HttpGet]
        public async Task<ActionResult<PatientsCollection>> GetPatients([FromQuery(Name = "index")]int? pageIndex, [FromQuery(Name = "size")] int? pageSize)
        {
            pageIndex ??= 1;
            pageSize ??= 10;

            try
            {
                var totalRows = await this.dbContext.TotalNumberOfRecords();

                var pagesCount = Math.Ceiling((double) totalRows / (int) pageSize);

                var result = new PatientsCollection
                {
                    TotalCount = totalRows,
                    PagesCount = Convert.ToInt32(pagesCount),
                    Data       = await this.dbContext.GetAllPatients(pageIndex.Value, pageSize.Value).ConfigureAwait(false)
                };
                
                this.log.LogDebug($"Returning a set of {result.Data.Count()} patients");

                return Ok(result);
            }
            catch(GeneralDbException dbException)
            {
                this.log.LogError(dbException, dbException.Message);

                return StatusCode(500, dbException);
            }
        }

        /// <summary>
        /// Add new patient to database
        /// </summary>
        /// <param name="patientName">Name of the patient.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] NewPatientRequest patientRequest)
        {
            if (patientRequest == null)
            {
                return BadRequest();
            }

            try
            {
                var factory = new PatientsFactory(this.dbContext);

                var newPatient = await factory.CreateNewPatient(patientRequest.PatientName);

                var addedPatient = await this.dbContext.AddPatient(newPatient);

                this.log.LogInformation($"New patient sucsesfully created with name: {addedPatient.Name}, ticket number = {addedPatient.TicketNumber} and visit time: {addedPatient.Time}");
                
                return CreatedAtAction(nameof(AddPatient), addedPatient);
            }
            catch(GeneralDbException expPost)
            {
                this.log.LogError($"Exception occurred while trying to add new patient !", expPost.Message);

                return StatusCode(500, expPost);
            }
        }

        /// <summary>
        /// Handles the pation.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> HandlePatient(Patient patient)
        {
            if (patient == null)
            {
                return BadRequest();
            }

            try
            {
                await this.dbContext.SetPatientHandled(patient);

                return NoContent();
            }
            catch (GeneralDbException expDb)
            {
                this.log.LogError($"Exception occured while trying to update a record in database !", expDb.Message);

                return StatusCode(500, expDb);
            }
        }
    }
}
