using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Presestance.Repository
{
    public class PatientRepository : IPatientRepository
    {
        public readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNationalIdentityAsync(string nationalIdentity)
        {
            var exists = await _context.Patients.AnyAsync(p => p.NationalIdentity.Value == nationalIdentity);
            return exists;
        }

        public async Task<bool> ExistsDuplicateActivePatientAsync(string nationalIdentity, 
            string phoneNumber)
        {
            var exists = await _context.Patients.AnyAsync(p =>
                p.NationalIdentity.Value == nationalIdentity &&
                p.PhoneNumber.Value == phoneNumber &&
                p.IsActive);

            return exists;
        }

        public Task<Patient?> GetByIdAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<Patient?> GetByMRNAsync(string mrn)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.MRN.Value == mrn);
            return patient;
        }

        public async Task<Patient?> GetByNationalIdentityAsync(string nationalIdentity)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.NationalIdentity.Value == nationalIdentity);
            return patient;
        }

        public  Task<bool> HasActiveAppointmentsAsync(Guid patientId)
        {
            //var hasActiveAppointments 
            //    = await _context.Appointments..AnyAsync(
            //        a => a.PatientId == patientId && a.IsActive);
            //return hasActiveAppointments;
            throw new NotImplementedException();
        }

        public void Update(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
