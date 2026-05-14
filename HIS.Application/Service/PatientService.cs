using HIS.Application.DTO;
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

        public Task<bool> ExistsByNationalIdentityAsync(string nationalIdentity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsDuplicateActivePatientAsync(string nationalIdentity, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<PatientDto?> GetByIdAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public Task<PatientDto?> GetByMRNAsync(string mrn)
        {
            throw new NotImplementedException();
        }

        public Task<PatientDto?> GetByNationalIdentityAsync(string nationalIdentity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasActiveAppointmentsAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public void Update(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
