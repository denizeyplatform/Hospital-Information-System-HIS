using HIS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Interface
{
    public interface IPatientService
    {
        Task AddPatientAsync(PatientDto patientDto);
        Task<PatientDto> GetPatientByIdAsync(Guid id);
        Task<List<PatientDto>> GetAllPatientsAsync();
        Task UpdatePatientAsync(PatientDto patientDto);
        Task DeletePatientAsync(Guid id);
    }
}
