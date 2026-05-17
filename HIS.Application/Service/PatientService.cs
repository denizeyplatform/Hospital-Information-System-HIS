using HIS.Application.DTO;
using HIS.Application.DTO.Common;
using HIS.Application.DTO.Documents;
using HIS.Application.DTO.MedicalHistory;
using HIS.Application.DTO.Patient;
using HIS.Application.Interface;
using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Service
{
    public class PatientService : IPatientService
    {
        public IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Task AddAsync(PatientDto patient)
        {
            // automapper
            // validation
            // exception handling
            // conditions
            // call repository
            throw new NotImplementedException();
        }

        public Task AddAsync(PatientDto patient, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IPatientService.AddAsync(PatientDto patient, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IPatientService.AddMedicalHistoryEntryAsync(Guid patientId, AddMedicalHistoryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IPatientService.DeactivatePatientAsync(Guid patientId, DeactivatePatientRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPatientService.ExistsByNationalIdentityAsync(string nationalIdentity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPatientService.ExistsDuplicateActivePatientAsync(string nationalIdentity, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        Task<PatientDto?> IPatientService.GetByIdAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        Task<PatientDto?> IPatientService.GetByMRNAsync(string mrn)
        {
            throw new NotImplementedException();
        }

        Task<PatientDto?> IPatientService.GetByNationalIdentityAsync(string nationalIdentity)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyCollection<DocumentDto>> IPatientService.GetPatientDocumentsAsync(Guid patientId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPatientService.HasActiveAppointmentsAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        Task<PagedResult<PatientSummaryDto>> IPatientService.SearchPatientsAsync(PatientSearchRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IPatientService.Update(Patient patient)
        {
            throw new NotImplementedException();
        }

        Task IPatientService.UploadMedicalDocumentAsync(Guid patientId, UploadMedicalDocumentRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
