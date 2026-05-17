using HIS.Application.DTO;
using HIS.Application.DTO.Common;
using HIS.Application.DTO.Documents;
using HIS.Application.DTO.MedicalHistory;
using HIS.Application.DTO.Patient;
using HIS.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIS.API.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public sealed class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // =========================================================
        // 1. Create Patient
        // POST: api/patients
        // =========================================================

        //[HttpPost]
        //[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> CreatePatient(
        //    [FromBody] PatientDto request,
        //    CancellationToken cancellationToken)
        //{
        //    var patientId =
        //        await _patientService.AddAsync(request);

        //    return CreatedAtAction(
        //        nameof(GetPatientById),
        //        new { patientId },
        //        patientId);
        //}

        // =========================================================
        // 2. Get Patient By Id
        // GET: api/patients/{patientId}
        // =========================================================

        //[HttpGet("{patientId:guid}")]
        //[ProducesResponseType(
        //    typeof(PatientDetailsDto),
        //    StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetPatientById(
        //    Guid patientId,
        //    CancellationToken cancellationToken)
        //{
        //    var patient =
        //        await _patientService.GetPatientByIdAsync(
        //            patientId,
        //            cancellationToken);

        //    if (patient is null)
        //        return NotFound();

        //    return Ok(patient);
        //}

        // =========================================================
        // 3. Update Patient
        // PUT: api/patients/{patientId}
        // =========================================================

        //[HttpPut("{patientId:guid}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> UpdatePatient(
        //    Guid patientId,
        //    [FromBody] UpdatePatientRequest request,
        //    CancellationToken cancellationToken)
        //{
        //    await _patientService.UpdatePatientAsync(
        //        patientId,
        //        request,
        //        cancellationToken);

        //    return NoContent();
        //}

        // =========================================================
        // 4. Upload Medical Document
        // POST: api/patients/{patientId}/documents
        // =========================================================

        [HttpPost("{patientId:guid}/documents")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> UploadMedicalDocument(
            Guid patientId,
            [FromForm] UploadMedicalDocumentRequest request,
            CancellationToken cancellationToken)
        {
            await _patientService.UploadMedicalDocumentAsync(
                patientId,
                request,
                cancellationToken);

            return NoContent();
        }

        // =========================================================
        // 5. Get Patient Documents
        // GET: api/patients/{patientId}/documents
        // =========================================================

        [HttpGet("{patientId:guid}/documents")]
        [ProducesResponseType(
            typeof(IReadOnlyCollection<DocumentDto>),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientDocuments(
            Guid patientId,
            CancellationToken cancellationToken)
        {
            var documents =
                await _patientService.GetPatientDocumentsAsync(
                    patientId,
                    cancellationToken);

            return Ok(documents);
        }

        // =========================================================
        // 6. Add Medical History Entry
        // POST: api/patients/{patientId}/medical-history
        // =========================================================

        [HttpPost("{patientId:guid}/medical-history")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMedicalHistoryEntry(
            Guid patientId,
            [FromBody] AddMedicalHistoryRequest request,
            CancellationToken cancellationToken)
        {
            await _patientService.AddMedicalHistoryEntryAsync(
                patientId,
                request,
                cancellationToken);

            return NoContent();
        }

        // =========================================================
        // 7. Search Patients
        // GET: api/patients/search
        // =========================================================

        [HttpGet("search")]
        [ProducesResponseType(
            typeof(PagedResult<PatientSummaryDto>),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPatients(
            [FromQuery] PatientSearchRequest request,
            CancellationToken cancellationToken)
        {
            var result =
                await _patientService.SearchPatientsAsync(
                    request,
                    cancellationToken);

            return Ok(result);
        }

        // =========================================================
        // 8. Deactivate Patient
        // PATCH: api/patients/{patientId}/deactivate
        // =========================================================

        [HttpPatch("{patientId:guid}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeactivatePatient(
            Guid patientId,
            [FromBody] DeactivatePatientRequest request,
            CancellationToken cancellationToken)
        {
            await _patientService.DeactivatePatientAsync(
                patientId,
                request,
                cancellationToken);

            return NoContent();
        }
    }
}
