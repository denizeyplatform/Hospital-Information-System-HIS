using HIS.Domain.Aggregates.PatientAggregate.DomainEvents;
using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using HIS.Domain.Aggregates.PatientAggregate.Enums;
using HIS.Domain.Aggregates.PatientAggregate.ValueObject;
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
          var p =  Patient.Create(patient.FullName,
                   patient.NationalIdentity,
                   patient.PhoneNumber,
                   patient.Address,
                   patient.DateOfBirth,
                   patient.Gender,
                   patient.InsurancePolicy);
            await _context.Patients.AddAsync(p);
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

        public async Task Update(Patient patient)
        {
            //patient.UpdatePersonalInfo(patient.FullName, patient.PhoneNumber, patient.Address, patient.Gender);

            patient.UpdatePersonalInfo(patient.FullName, patient.PhoneNumber, patient.Address, patient.Gender);
             _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<DocumentRecord>> GetPatientDocumentsAsync(
                        Guid patientId,
                        CancellationToken cancellationToken = default)
        {
            var patientExists = await _context.Patients
                .AnyAsync(
                    x => x.Id == patientId,
                    cancellationToken);

            if (!patientExists)
                throw new Exception("Patient not found.");

            return await _context.Documents
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.UploadedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddMedicalHistoryEntryAsync(
                Guid patientId,
                MedicalRecord medicalRecord,
                CancellationToken cancellationToken = default)
        {
            var patient = await _context.Patients
                .Include(x => x.MedicalRecords)
                .FirstOrDefaultAsync(
                    x => x.Id == patientId,
                    cancellationToken);

            if (patient is null)
                throw new Exception("Patient not found.");

            if (!patient.IsActive)
                throw new Exception(
                    "Cannot add medical history to inactive patient.");

            patient.AddMedicalHistoryEntry(medicalRecord);
        }

        public async Task<IReadOnlyCollection<Patient>>
        SearchPatientsAsync(
            string? name,
            string? nationalIdentity,
            string? mrn,
            string? phoneNumber,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(nationalIdentity) &&
                string.IsNullOrWhiteSpace(mrn) &&
                string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new Exception(
                    "At least one search criteria required.");
            }

            if (pageSize > 100)
                pageSize = 100;

            IQueryable<Patient> query =
                _context.Patients.AsQueryable();

           

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x =>
                    (x.FullName.FirstName + " " +
                     x.FullName.LastName)
                    .Contains(name));
            }


            if (!string.IsNullOrWhiteSpace(nationalIdentity))
            {
                query = query.Where(x =>
                    x.NationalIdentity.Value ==
                    nationalIdentity);
            }

           

            if (!string.IsNullOrWhiteSpace(mrn))
            {
                query = query.Where(x =>
                    x.MRN.Value == mrn);
            }

          

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                query = query.Where(x =>
                    x.PhoneNumber.Value == phoneNumber);
            }

            return await query
                .OrderBy(x => x.FullName.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task DeactivatePatientAsync(
                    Guid patientId,
                    CancellationToken cancellationToken = default)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(
                    x => x.Id == patientId,
                    cancellationToken);

            if (patient is null)
                throw new Exception("Patient not found.");

            var hasActiveAppointments =
                await HasActiveAppointmentsAsync(patient.Id);

            patient.Deactivate(hasActiveAppointments);
        }

        public Task UploadMedicalDocumentAsync(Guid patientId, DocumentRecord document, CancellationToken cancellationToken = default)
        {
            // scan docment vires
            // upload document to storage
            // save document url to database
            throw new NotImplementedException();
        }

        void IPatientRepository.Update(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
