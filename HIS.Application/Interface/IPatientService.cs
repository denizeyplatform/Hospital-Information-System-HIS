using HIS.Application.DTO;
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
        Task AddAsync(PatientDto patient);

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
        //5 get patient documents function
        //6 add medical history function
        //7 search patients by name, NID, MRN, phone)
        //8 deactivate patient function
    }
}
