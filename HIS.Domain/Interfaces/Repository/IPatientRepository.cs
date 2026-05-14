using HIS.Domain.Aggregates.PatientAggregate.Entities;
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



        //---------------------


        Task AddAsync(Patient patient);

        Task<Patient?> GetByIdAsync(Guid patientId);

        Task<Patient?> GetByMRNAsync(string mrn);

        Task<Patient?> GetByNationalIdentityAsync(string nationalIdentity);

        Task<bool> ExistsByNationalIdentityAsync(string nationalIdentity);

        Task<bool> ExistsDuplicateActivePatientAsync(string nationalIdentity,string phoneNumber);

        Task<bool> HasActiveAppointmentsAsync(Guid patientId);

        void Update(Patient patient);
    }
}
