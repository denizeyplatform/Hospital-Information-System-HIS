using HIS.Application.DTO;
using HIS.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Service
{
    public class PatientService : IPatientService
    {
        public Task AddPatientAsync(PatientDto patientDto)
        {
            throw new NotImplementedException();
        }

        public Task DeletePatientAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientDto>> GetAllPatientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PatientDto> GetPatientByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePatientAsync(PatientDto patientDto)
        {
            throw new NotImplementedException();
        }
    }
}
