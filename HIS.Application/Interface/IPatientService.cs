using HIS.Application.DTO;
using HIS.Application.DTO.Documents;
using HIS.Application.DTO.MedicalHistory;
using HIS.Application.DTO.Patient;
using HIS.Domain.Aggregates.PatientAggregate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Interface
{
    public interface IPatientService
    {
        //Task AddPatientAsync(PatientDto patientDto);
        //Task<PatientDto> GetPatientByIdAsync(Guid id);
        //Task<List<PatientDto>> GetAllPatientsAsync();
        //Task UpdatePatientAsync(PatientDto patientDto);
        //Task DeletePatientAsync(Guid id);

        // ----------------------------------------

        //1
        Task AddAsync(PatientDto patient, CancellationToken cancellationToken);

        //2
        Task<PatientDto?> GetByIdAsync(Guid patientId);

        Task<PatientDto?> GetByMRNAsync(string mrn);

        Task<PatientDto?> GetByNationalIdentityAsync(string nationalIdentity);

        Task<bool> ExistsByNationalIdentityAsync(string nationalIdentity);

        Task<bool> ExistsDuplicateActivePatientAsync(string nationalIdentity, string phoneNumber);

        Task<bool> HasActiveAppointmentsAsync(Guid patientId);

        //3
        void Update(Patient patient);

        //4 upload medical record function
        Task UploadMedicalDocumentAsync(Guid patientId,UploadMedicalDocumentRequest request,
               CancellationToken cancellationToken = default);

        
        Task<IReadOnlyCollection<DocumentDto>> GetPatientDocumentsAsync(Guid patientId,
            CancellationToken cancellationToken = default);
        
        Task AddMedicalHistoryEntryAsync(
             Guid patientId,
             AddMedicalHistoryRequest request,
             CancellationToken cancellationToken = default);
        
        Task<HIS.Application.DTO.Common.PagedResult<PatientSummaryDto>> SearchPatientsAsync(
           PatientSearchRequest request,
           CancellationToken cancellationToken = default);
      
            Task DeactivatePatientAsync(
                    Guid patientId,
                    DeactivatePatientRequest request,
                    CancellationToken cancellationToken = default);
    }
}
