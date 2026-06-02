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

        public PatientRepository()
        {
            
        }
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Patient patient)
        {
            
           var p = Patient.Create(
                patient.FullName, 
                patient.NationalIdentity, 
                patient.PhoneNumber, 
                patient.Address,
                patient.DateOfBirth,
                patient.Gender);
            await _context.Patients.AddAsync(p);
            await _context.SaveChangesAsync();
        }



        public async Task<bool> ExistsDuplicateActivePatientAsync(
                  string nationalIdentity,
                  string phoneNumber
                  )
        {
            return await _context.Patients
                .AsNoTracking()
                .AnyAsync(
                    x =>
                        x.IsActive &&
                        x.NationalIdentity.Value == nationalIdentity &&
                        x.PhoneNumber.Value == phoneNumber
                  );
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            return await _context.Patients
                .Include(x => x.Documents)
                .Include(x => x.MedicalRecords)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> HasActiveAppointmentsAsync(Guid patientId)
        {
            //return await _context.Appointments
            //    .AnyAsync(x => x.PatientId == patientId && x.IsActive);
       
            throw new NotImplementedException();
        }


        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<DocumentRecord>> GetPatientDocumentsAsync(Guid patientId,
                        CancellationToken cancellationToken = default)
        {
            var patientExists = await _context.Patients
                .AnyAsync(
                    x => x.Id == patientId,
                    cancellationToken);

            if (!patientExists)
                throw new Exception("Patient not found.");


            // concat full url from storage path and return document records
            return await _context.Documents.AsNoTracking()
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.UploadedAt)
                .ToListAsync(cancellationToken);
        }
      
        public async Task AddMedicalHistoryEntryAsync(Guid patientId, MedicalRecord medicalRecord,
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
            //_context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Patient>> SearchPatientsAsync(string? name,string? nationalIdentity,
       string? mrn, string? phoneNumber, int pageNumber, int pageSize)
        {
            IQueryable<Patient> query = _context.Patients.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x =>
                    (x.FullName.FirstName + " " + x.FullName.LastName).Contains(name));

            if (!string.IsNullOrWhiteSpace(nationalIdentity))
                query = query.Where(x => x.NationalIdentity.Value == nationalIdentity);

            if (!string.IsNullOrWhiteSpace(mrn))
                query = query.Where(x => x.MRN.Value == mrn);

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(x => x.PhoneNumber.Value == phoneNumber);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
            //_context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UploadMedicalDocumentAsync(
           Guid patientId,
           DocumentRecord document,
           CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(document);

            var patient = await _context.Patients
                .Include(x => x.Documents)
                .FirstOrDefaultAsync(
                    x => x.Id == patientId,
                    cancellationToken);

            if (patient is null)
                throw new KeyNotFoundException("Patient not found.");

            if (!patient.IsActive)
            {
                throw new InvalidOperationException(
                    "Cannot upload documents to inactive patient.");
            }

            // external storage upload here
            // antivirus scanning here
            // generate secure file url here

            patient.AttachDocument(document);

            await _context.SaveChangesAsync(cancellationToken);
        }

      
    }
}
