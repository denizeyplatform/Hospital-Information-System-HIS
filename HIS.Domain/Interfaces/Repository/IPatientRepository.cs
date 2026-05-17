using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Domain.Interfaces.Repository
{
    public interface IPatientRepository
    {
        //Task create(Patient patient);
        // Task<Patient> getById(Guid id);
        // Task<List<Patient>> getAll();
        // Task update(Patient patient);
        // Task delete(Guid id);

        Task<Patient?> GetByMRNAsync(string mrn);

        Task<Patient?> GetByNationalIdentityAsync(string nationalIdentity);

        Task<bool> ExistsByNationalIdentityAsync(string nationalIdentity);

        Task<bool> ExistsDuplicateActivePatientAsync(string nationalIdentity, string phoneNumber);

        Task<bool> HasActiveAppointmentsAsync(Guid patientId);

        //---------------------

        Task AddAsync(Patient patient);

        Task<Patient?> GetByIdAsync(Guid patientId);

        void Update(Patient patient);

        //4 upload medical record function
        Task UploadMedicalDocumentAsync(Guid patientId,DocumentRecord document,CancellationToken cancellationToken = default);
  
        
        Task<IReadOnlyCollection<DocumentRecord>> GetPatientDocumentsAsync(
            Guid patientId,CancellationToken cancellationToken = default);
     
        Task AddMedicalHistoryEntryAsync(Guid patientId,MedicalRecord medicalRecord,
          CancellationToken cancellationToken = default);
        
        Task<IReadOnlyCollection<Patient>>
        SearchPatientsAsync(
            string? name,
            string? nationalIdentity,
            string? mrn,
            string? phoneNumber,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

       
        Task DeactivatePatientAsync(Guid patientId,
        CancellationToken cancellationToken = default);


    }
}
