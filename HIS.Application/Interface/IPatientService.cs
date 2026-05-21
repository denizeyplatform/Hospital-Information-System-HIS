using HIS.Application.DTO;
using HIS.Application.DTO.Common;
using HIS.Application.DTO.Documents;
using HIS.Application.DTO.MedicalHistory;
using HIS.Application.DTO.Patient;
using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Interface
{
    public interface IPatientService
    {


        //1
        Task<Guid> CreateAsync(CreatePatientRequest request);

        //2
        Task<PatientDetailsDto?> GetByIdAsync(Guid patientId);

       

        //Task<bool> ExistsDuplicateActivePatientAsync(string nationalIdentity, string phoneNumber);

        //Task<bool> HasActiveAppointmentsAsync(Guid patientId);

        //3
        Task UpdatePatientAsync(Guid patientId, UpdatePatientRequest request, CancellationToken cancellationToken = default);

        //4 upload medical record function
        Task UploadDocumentAsync(Guid patientId,UploadMedicalDocumentRequest request,
               CancellationToken cancellationToken = default);

        
        Task<IReadOnlyCollection<DocumentDto>> GetPatientDocumentsAsync(Guid patientId,
            CancellationToken cancellationToken = default);

        Task AddMedicalHistoryAsync(Guid patientId, MedicalRecord record);


        Task<PagedResult<PatientDto>> SearchAsync(
                PatientSearchRequest request,
           CancellationToken cancellationToken = default);
      
            Task DeactivatePatientAsync(
                    Guid patientId,
                    DeactivatePatientRequest request,
                    CancellationToken cancellationToken = default);
    }
}
